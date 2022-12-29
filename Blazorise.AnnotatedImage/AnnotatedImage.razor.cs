#region Using directives
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SkiaSharp;
using Blazorise.Utilities;
#endregion

namespace Blazorise.AnnotatedImage
{


    public partial class AnnotatedImage : BaseComponent, IAsyncDisposable
    {
        #region Members 
        private SKBitmap? bitmap;
        private ElementReference hiddenImage;
        private string hiddenImageSrc = string.Empty;  
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
        public async Task<SKImage> GetImage()
        {
            var encodedString = await JSModule!.GetBase64Image(imgRef);
            return SKImage.FromEncodedData(Convert.FromBase64String(encodedString));  
        }
        public Task AddAnnotation(IImageAnnotationData item)
        {
            Annotations.Add(item);
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
        [Parameter] public List<IImageAnnotationData> Annotations { get; set; } = new();
        public BoundingClientRect CanvasRect { get; private set; }
        public ElementReference imgRef;
        public ElementReference AnnotatedImageRef;
       
        /// <summary>
        /// Width of original image
        /// </summary>
        public double ImageWidth { get; private set; }
        /// <summary>
        ///  Height of original image
        /// </summary>
        public double ImageHeight { get; private set; }
        public double ImgElementWidth { get; private set; }
        public double ImgElementHeight { get; private set; }  
        public double Scale { get; private set; }
        #endregion
    }
}
