
#region Using directives
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Blazorise.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
#endregion

namespace Blazorise.Camera;

public partial class Camera : BaseComponent, IAsyncDisposable
{

	private ElementReference  _canvasRef;

	#region Methods

	/// <inheritdoc />
	public override async Task SetParametersAsync(ParameterView parameters)
	{
		if(Rendered)
		{

		}

		await base.SetParametersAsync(parameters);	
	}

	/// <inheritdoc/>
	protected override Task OnInitializedAsync()
	{
		if (JSModule == null)
		{
			JSModule = new JSCameraModule(JSRuntime, VersionProvider);
		}

		return base.OnInitializedAsync();
	}


	/// <inheritdoc />
	protected override async Task OnFirstAfterRenderAsync()
	{
		await JSModule.Initialize(ElementRef, _canvasRef, MirrorImage);
	}

	/// <inheritdoc/>
	protected override async ValueTask DisposeAsync(bool disposing)
	{
		if (disposing && Rendered)
		{
			await JSModule.SafeDisposeAsync();
		}

		await base.DisposeAsync(disposing);
	}


	public async ValueTask<string> TakePicture()
	{
		return await JSModule.TakePicture();
	}

	#endregion

	#region Properties 
	/// <summary>
	/// Gets or sets the JSCameraModule instance.
	/// </summary>
	protected JSCameraModule JSModule { get; private set; }

	/// <summary>
	/// Gets or sets the JS runtime.
	/// </summary>
	[Inject] private IJSRuntime JSRuntime { get; set; }

	/// <summary>
	/// Gets or sets the version provider.
	/// </summary>
	[Inject] private IVersionProvider VersionProvider { get; set; }

	/// <summary>
	/// Image alt text.
	/// </summary>
	[Parameter] public string Alt { get; set; }

	[Parameter] public bool MirrorImage { get; set; }


	#endregion
}
