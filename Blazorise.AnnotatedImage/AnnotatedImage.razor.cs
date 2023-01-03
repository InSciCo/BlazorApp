﻿#region Using directives
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SkiaSharp;
using Blazorise.Utilities;
using Blazorise.Extensions;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335; 
#endregion

namespace Blazorise.AnnotatedImage
{


    public partial class AnnotatedImage : BaseComponent, IAsyncDisposable
    {
        #region Members 
        protected ElementReference backgroundImageRef;
        #endregion

        #region Methods 
        protected override void BuildClasses(ClassBuilder builder)
        {
            builder.Append("annotatedimage");
            base.BuildClasses(builder);
        }
        protected async override Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }
        protected async override Task OnParametersSetAsync()
        {
            JSModule ??= new JSAnnotatedImageModule(JSRuntime!, VersionProvider!);
            await base.OnParametersSetAsync();
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            // read the current width and height of the img element
            CanvasRect = await JSModule!.GetBoundingClientRect(backgroundImageRef);
            ImgElementHeight = CanvasRect.Height;
            ImgElementWidth = CanvasRect.Width;
        }
        protected override async Task OnFirstAfterRenderAsync()
        {
            await JSModule!.Initialize();
        }
        protected override async ValueTask DisposeAsync(bool disposing)
        {
            if (disposing && Rendered)
                await JSModule.SafeDisposeAsync();
            await base.DisposeAsync(disposing);
        }
        private async Task<SKImage> GetImage(ElementReference imgElementReference)
        {
            var encodedString = await JSModule!.GetBase64Image(imgElementReference);
            return SKImage.FromEncodedData(Convert.FromBase64String(encodedString));
        }
        public async Task<string> GetMergedEncodedImage()
        {
            var backgroundImage = await GetImage(backgroundImageRef);
            var info = new SKImageInfo(backgroundImage.Width, backgroundImage.Height);
            using (var surface = SKSurface.Create(info))
            {
                var canvas = surface.Canvas;
                canvas.DrawImage(backgroundImage, new SKPoint(0, 0));

                foreach (var annotation in Annotations.Values.OrderBy(x => x.Order))
                {
                    Console.WriteLine($"Order:{annotation.Order}");
                    var annotationImage =
                        SKImage.FromEncodedData(
                            Convert.FromBase64String(
                                await JSModule!.GetImageAnnotationDataURLById(
                                    annotation.Id)));

                    // Scale the annotation and draw it into the canvas using skia ScalePixels 
                    // Note: This is the primary reason we use the skia lib. PNG images are scaled without 
                    // losing transparency. There are js solutions to do this scaling, but they very slow
                    // in comparision to using skia.
                    var width = annotation.Width * annotation.Scale;
                    var height = annotation.Height * annotation.Scale;
                    var x = (float)(annotation.X - (width / 2));
                    var y = (float)(annotation.Y - (height / 2));
                    var x2 = (float)(x + width);
                    var y2 = (float)(y + height);
                    var sourceBitmap = SKBitmap.FromImage(annotationImage);
                    var resizeInfo = new SKImageInfo((int)width, (int)height);     
                    var resizedBitmap = new SKBitmap(resizeInfo);
                    sourceBitmap.ScalePixels(resizedBitmap, SKFilterQuality.High);
                    canvas.DrawBitmap(resizedBitmap, new SKRect(x, y, x2, y2));
                }

                var imgdata = 
                    Convert.ToBase64String(
                    surface.Snapshot()
                    .Encode(SKEncodedImageFormat.Png, 100)
                    .ToArray());

                return "data:image/png;base64," + imgdata;
            }
        }
        // Perculate from ImageAnnotation event to parent of AnnotatedImage
        protected async Task ImageAnnotationSelected(string id)
        {
            if (!MultiSelect)
                foreach (var item in Annotations.Values.Where(x => x.Id != id))
                    item.Selected = false;
            await OnImageAnnotationSelected.InvokeAsync(id);
        }
        protected async Task ImageAnnotationStartMove(string id) => await OnImageAnnotationStartMove.InvokeAsync(id);
        protected async Task ImageAnnotationMoved(string id) => await OnImageAnnotationMoved.InvokeAsync(id);
        protected async Task ImageAnnotationEndMove(string id) => await OnImageAnnotationEndMove.InvokeAsync(id);
        protected async Task ImageAnnotationUnselected(string id) => await OnImageAnnotationUnselected.InvokeAsync(id);
        #endregion

        #region Properties 
        /// <summary>
        /// Gets or sets the JSCameraModule instance.
        /// </summary>
        protected JSAnnotatedImageModule? JSModule { get; private set; }
        /// <summary>
        /// Gets or sets the JS runtime.
        /// </summary>
        [Inject] private IJSRuntime? JSRuntime { get; set; }
        /// <summary>
        /// Gets or sets the version provider.
        /// </summary>
        [Inject] private IVersionProvider? VersionProvider { get; set; }
        public BoundingClientRect? CanvasRect { get; private set; }

        [Parameter] public string Source { get; set; } = string.Empty;
        [Parameter] public bool MultiSelect { get; set; }
        [Parameter] public Dictionary<string,IImageAnnotationData> Annotations { get; set; } = new();
        [Parameter] public EventCallback<string> OnImageAnnotationSelected { get; set; }
        [Parameter] public EventCallback<string> OnImageAnnotationStartMove { get; set; }
        [Parameter] public EventCallback<string> OnImageAnnotationMoved { get; set; }
        [Parameter] public EventCallback<string> OnImageAnnotationEndMove { get; set; }
        [Parameter] public EventCallback<string> OnImageAnnotationUnselected { get; set; }

        /// <summary>
        /// Width of original image
        /// </summary>
        public double ImgElementWidth { get; private set; }
        public double ImgElementHeight { get; private set; }  
        public double Scale { get; private set; }
        #endregion
    }
}
