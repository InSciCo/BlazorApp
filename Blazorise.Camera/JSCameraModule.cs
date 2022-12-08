#region Using directives
using System.Threading.Tasks;
using Blazorise.Modules;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
#endregion

namespace Blazorise.Camera;

/// <summary>
/// Contracts for the Camera JS module.
/// </summary>
public class JSCameraModule : BaseJSModule
{
	#region Constructors

	/// <summary>
	/// Default module constructor.
	/// </summary>
	/// <param name="jsRuntime">JavaScript runtime instance.</param>
	/// <param name="versionProvider">Version provider.</param>
	public JSCameraModule(IJSRuntime jsRuntime, IVersionProvider versionProvider) : base(jsRuntime, versionProvider)
	{
	}

	#endregion

	#region Methods

	/// <summary>
	/// Initializes the new Camera within the JS module.
	/// </summary>
	/// <param name="elementRef">Reference to the rendered element.</param>
	/// <param name="elementId">ID of the rendered element.</param>
	/// <param name="options">Additional options for the tooltip initialization.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	public virtual ValueTask Initialize(ElementReference elementRef, ElementReference canvasRef, bool mirrorImage)
		=> InvokeSafeVoidAsync("initialize", elementRef, canvasRef, mirrorImage);

	/// <summary>
	/// Take picture and return base64 encoded string.
	/// </summary>
	/// <returns>A task that represents the asynchronous operation.</returns>
	public virtual ValueTask<string> TakePicture()
		=> InvokeSafeAsync<string>("takepicture");

	#endregion

	#region Properties

	/// <inheritdoc/>
	public override string ModuleFileName => $"./_content/Blazorise.Camera/blazorise.camera.js?v={VersionProvider.Version}";

	#endregion
}
