#region Using directives
using System.Threading.Tasks;
using Blazorise.Modules;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
#endregion

namespace Blazorise.AnnotatedImage;

public class JSAnnotatedImageModule : BaseJSModule
{
    #region Constructors
    /// <summary>
    /// Default module constructor.
    /// </summary>
    /// <param name="jsRuntime">JavaScript runtime instance.</param>
    /// <param name="versionProvider">Version provider.</param>
    public JSAnnotatedImageModule(IJSRuntime jsRuntime, IVersionProvider versionProvider) : base(jsRuntime, versionProvider)
    {
    }
    #endregion

    #region Methods
    public virtual ValueTask Initialize()
        => InvokeSafeVoidAsync("initialize");
    public virtual ValueTask SetPointerCapture(ElementReference elementRef, long pointerId)
        => InvokeSafeVoidAsync("setPointerCapture",elementRef,pointerId);
    #endregion

    #region Properties

    /// <inheritdoc/>
    public override string ModuleFileName => $"./_content/Blazorise.AnnotatedImage/blazorise.annotatedimage.js?v={VersionProvider.Version}";
    //public override string ModuleFileName => $"./_content/Blazorise.AnnotatedImage/blazorise.imageannotations.js";

    #endregion

}
