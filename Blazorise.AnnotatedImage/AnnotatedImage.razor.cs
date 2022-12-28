#region Using directives
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using SkiaSharp;
using Blazorise.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using System.Reflection;
using Blazorise.Utilities;
#endregion

namespace Blazorise.AnnotatedImage
{
    public class SelectedItem
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Source { get; set; } = string.Empty;
        public int Width { get; set; }
        public int Height { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public partial class AnnotatedImage : BaseComponent, IAsyncDisposable
    {
        #region Members 
        private SKBitmap? bitmap;
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
            bitmap = await GetImageBitmap(Source);
            await base.OnParametersSetAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {

            await base.OnAfterRenderAsync(firstRender);
			// read the current width and height of the img element
			ImgElementHeight = await JSModule!.GetImgHeight(imgRef);
			ImgElementWidth = await JSModule!.GetImgWidth(imgRef);

        }

        public async Task<SKBitmap> GetImageBitmap(string url)
        {
            using (Stream stream = await httpClient.GetStreamAsync(url))
            using (MemoryStream memStream = new MemoryStream())
            {
                await stream.CopyToAsync(memStream);
                memStream.Seek(0, SeekOrigin.Begin);
                return SKBitmap.Decode(memStream);
            }
        }

        public Task AddSelectedItem(SelectedItem item)
        {
            SelectedItems.Add(item);
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
        [Parameter] public List<SelectedItem> SelectedItems { get; set; } = new();


        public ElementReference imgRef;
        /// <summary>
        /// Width of original image
        /// </summary>
        public int ImageWidth { get; private set; }
        /// <summary>
        ///  Height of original image
        /// </summary>
        public int ImageHeight { get; private set; }
        public int ImgElementWidth { get; private set; }
        public int ImgElementHeight { get; private set; }  
        public double Scale { get; private set; }
        #endregion
    }
}
