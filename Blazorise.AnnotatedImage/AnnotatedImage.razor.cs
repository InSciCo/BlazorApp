#region Using directives
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SkiaSharp;
using Blazorise.Utilities;
using Blazorise.Extensions;
using System.Diagnostics;
#endregion

namespace Blazorise.AnnotatedImage
{
    public class Annotation
    {
        public IImageAnnotationData? AnnotationData { get; set; }    
        public ImageAnnotation? ImageAnnotation { get; set; }    
    }

    public partial class AnnotatedImage : BaseComponent, IAsyncDisposable
    {
        #region Members 
        protected Dictionary<string,Annotation> annotations = new();
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
            List<string> delList = annotations.Keys.ToList();   
            foreach(var item in ImageAnnotations.Values)
                if (delList.Contains(item.Id))
                    delList.Remove(item.Id);
                else
                    annotations.Add(item.Id, new Annotation() { AnnotationData = item });
            foreach (var item in delList)
                annotations.Remove(item);
            await base.OnParametersSetAsync();
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            // read the current width and height of the img element
            CanvasRect = await JSModule!.GetBoundingClientRect(imgRef);
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
            var backgroundImage = await GetImage(imgRef);
            var info = new SKImageInfo(backgroundImage.Width, backgroundImage.Height);
            using (var surface = SKSurface.Create(info))
            {
                var canvas = surface.Canvas;
                canvas.DrawImage(backgroundImage, new SKPoint(0, 0));

                foreach (var annotation in annotations.Values)
                {
                    var annotationImage = await GetImage(annotation.ImageAnnotation!.ImgRef);
                    // Scale the annotation and draw it into the canvas using skia ScalePixels 
                    // Note: This is the primary reason we use the skia lib. PNG images are scaled without 
                    // losing transparency. There are js solutions to do this scaling, but they very slow
                    // in comparision to using skia.
                    var width = annotation.AnnotationData!.Width * annotation.AnnotationData.Scale;
                    var height = annotation.AnnotationData!.Height * annotation.AnnotationData.Scale;
                    var x = (float)(annotation.AnnotationData!.X - (width / 2));
                    var y = (float)(annotation.AnnotationData!.Y - (height / 2));
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
        [Parameter] public string Source { get; set; } = string.Empty;

        [Parameter] public Dictionary<string,IImageAnnotationData> ImageAnnotations { get; set; } = new();
        public BoundingClientRect? CanvasRect { get; private set; }
        public ElementReference imgRef;
        public ElementReference AnnotatedImageRef;
       
        /// <summary>
        /// Width of original image
        /// </summary>
        public double ImgElementWidth { get; private set; }
        public double ImgElementHeight { get; private set; }  
        public double Scale { get; private set; }
        #endregion
    }
}
