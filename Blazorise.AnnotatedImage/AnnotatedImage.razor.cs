#region Using directives
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SkiaSharp;
using Blazorise.Utilities;
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
            if (JSModule == null)
                JSModule = new JSAnnotatedImageModule(JSRuntime!, VersionProvider!);
            await base.OnParametersSetAsync();
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            // read the current width and height of the img element
            CanvasRect = await JSModule!.GetBoundingClientRect(imgRef);
            ImgElementHeight = CanvasRect.Height;
            ImgElementWidth = CanvasRect.Width;
			ImgElementHeight = await JSModule!.GetImgHeight(imgRef);
			ImgElementWidth = await JSModule!.GetImgWidth(imgRef);
        }
        //public async Task<string> GetMergedEncodedImage()
        //{

        //    foreach (var annotation in Annotations)
        //        await JSModule!.CreateMergeCanvas(annotation.ImageAnnotation!.ImgRef);
        //    var imgdata = await JSModule!.GetMergeImageURL();
        //    return imgdata;
        //}
        //public async Task<string> GetMergedEncodedImage()
        //{
        //    await JSModule!.CreateMergeCanvas(imgRef);
        //    foreach (var annotation in Annotations)
        //    {
        //        await JSModule!.AddAnnotation(
        //            annotation.ImageAnnotation!.ImgRef,
        //            annotation.AnnotationData!.X,
        //            annotation.AnnotationData!.Y,
        //            annotation.AnnotationData!.Width,
        //            annotation.AnnotationData!.Height);
        //        var yada = await JSModule!.GetBase64Image(annotation.ImageAnnotation!.ImgRef);
        //        Console.WriteLine(yada);
        //    }
        //    var imgdata = await JSModule!.GetMergeImageURL();
        //    return imgdata;
        //}

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

                foreach (var annotation in Annotations)
                {
                    var annotationImage = await GetImage(annotation.ImageAnnotation!.ImgRef);
                    // Scale the annotation and draw it into the canvas using skia ScalePixels 
                    // Note: This is the primary reason we use the skia lib. PNG images are scaled without 
                    // losing transparency. There are js solutions to do this scalingm, but they very slow
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

        public Task AddAnnotation(IImageAnnotationData item)
        {
            var newAnnotation = new Annotation() { AnnotationData= item };
            Annotations.Add(newAnnotation);
            InvokeAsync(StateHasChanged);
            return Task.CompletedTask;
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
        [Parameter] public List<Annotation> Annotations { get; set; } = new();
        public BoundingClientRect CanvasRect { get; private set; }
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
