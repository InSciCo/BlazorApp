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
	/// Initialize Camera with specified options
	/// </summary>
	/// <param name="videoRef">video element reference</param>
	/// <param name="canvasRef">canvas element reference</param>
	/// <param name="mirrorImage"></param>
	/// <param name="facingMode">Must be one of: "user" | "environment"</param>
	/// <returns></returns>
	public virtual ValueTask Initialize(ElementReference videoRef, ElementReference canvasRef, bool mirrorImage, string facingMode)
		=> InvokeSafeVoidAsync("initialize", videoRef, canvasRef, mirrorImage, facingMode);

	/// <summary>
	/// Take picture and return base64 encoded string.
	/// </summary>
	/// <returns>A task that represents the asynchronous operation.</returns>
	public virtual ValueTask<string> TakePicture()
		=> InvokeSafeAsync<string>("takepicture");

	public virtual async ValueTask<(double, double)> GetWidthAndHeight()
	{
		var resultArray = await InvokeAsync<double[]>("getWidthAndHeight");
		return (resultArray[0], resultArray[1]);
	}

	#endregion

	#region Properties

	/// <inheritdoc/>
	public override string ModuleFileName => $"./_content/Blazorise.Camera/blazorise.camera.js?v={VersionProvider.Version}";

	#endregion
}
