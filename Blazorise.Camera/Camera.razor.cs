
#region Using directives
using Blazorise.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
#endregion

namespace Blazorise.Camera;

public partial class Camera : BaseComponent, IAsyncDisposable
{

	#region Methods

	/// <inheritdoc/>
	protected override Task OnInitializedAsync()
	{
		if (JSModule == null)
			JSModule = new JSCameraModule(JSRuntime!, VersionProvider!);
		return base.OnInitializedAsync();
	}
	/// <inheritdoc />
	protected override async Task OnFirstAfterRenderAsync()
	{

		await JSModule!.Initialize(ElementRef, MirrorImage, "environment");
		JSModule.CameraInitializedHandlerEvent += JSModuleOnInitialized;
	}

	private void JSModuleOnInitialized()
	{
		CameraInitialized.InvokeAsync(this);
	}

	/// <inheritdoc/>
	protected override async ValueTask DisposeAsync(bool disposing)
	{
		if (disposing && Rendered)
		{
            JSModule!.CameraInitializedHandlerEvent -= JSModuleOnInitialized;
            await JSModule.SafeDisposeAsync();
        }
		await base.DisposeAsync(disposing);
	}
	public async ValueTask<string> TakePicture()
	{
		return await JSModule!.TakePicture();
	}


	public async ValueTask<(int Width,int Height)> GetWidthAndHeight()
	{
		return await JSModule!.GetWidthAndHeight();
	}

	#endregion

	#region Properties 
	/// <summary>
	/// Gets or sets the JSCameraModule instance.
	/// </summary>
	protected JSCameraModule? JSModule { get; private set; }

	/// <summary>
	/// Gets or sets the JS runtime.
	/// </summary>
	[Inject] private IJSRuntime? JSRuntime { get; set; }

	/// <summary>
	/// Gets or sets the version provider.
	/// </summary>
	[Inject] private IVersionProvider? VersionProvider { get; set; }

	/// <summary>
	/// Image alt text.
	/// </summary>
	[Parameter] public string Alt { get; set; } = string.Empty;

	[Parameter] public bool MirrorImage { get; set; }
	[Parameter] public EventCallback CameraInitialized { get; set; }	

	#endregion
}
