#region Using directives
using Blazorise.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
#endregion

namespace Blazorise.AnnotatedImage;

public partial class ImageAnnotation : BaseComponent, IAsyncDisposable
{
    #region Members
    private string containerPos => $"top:{y}px; left:{x}px; width:{imageWidth}px; height:{imageHeight}px;";
    private int pageX;
    private int pageY;
    private bool pointerDown;
    private long lastMoveTick;
    private double scale = 1.0;
    private double scaleTop = 2.0;
    private double scaleBottom = 0.5;
    private double scaleIncrement = 0.25;
    private System.Threading.Timer? timer;
    private long timerInterval = 400; // number of msec between calls to OnTimedEvent
    private long scaleLag = 1200; // number of msec before start of scaling
    private bool scaling;
    private int imageHeight => (int)(Scale * ImageHeight);
    private int imageWidth => (int) (Scale * ImageWidth);
    private int x => X - imageWidth / 2;
    private int y => Y - imageHeight / 2;
    private ElementReference elementRef;
    #endregion

    #region Methods
    /// <inheritdoc/>
    protected override Task OnInitializedAsync()
    {
        if (JSModule == null)
            JSModule = new JSAnnotatedImageModule(JSRuntime, VersionProvider);
        return base.OnInitializedAsync();
    }
    /// <inheritdoc />
    protected override async Task OnFirstAfterRenderAsync()
    {
        await JSModule.Initialize();
    }
    /// <inheritdoc/>
    protected override async ValueTask DisposeAsync(bool disposing)
    {
        if (disposing && Rendered)
            await JSModule.SafeDisposeAsync();
        await base.DisposeAsync(disposing);
    }
    private async void PointerDown(PointerEventArgs args)
    {
        if (pointerDown)
            return;
        pageX = (int)args.PageX;
        pageY = (int)args.PageY; 
        pointerDown = true;
        scaling = false;
        lastMoveTick = DateTime.UtcNow.Ticks;
        var autoEvent = new AutoResetEvent(false);
        timer = new(OnTimedEvent!, autoEvent, scaleLag, timerInterval);
        await JSModule.SetPointerCapture(elementRef, args.PointerId);
    }
    private void OnTimedEvent(Object state)
    {
        if(!pointerDown) 
            return;

        var elapsedTime = (DateTime.UtcNow.Ticks - lastMoveTick) / 1000 ;
        Console.WriteLine(elapsedTime.ToString());
        if (!scaling && (elapsedTime < scaleLag))
            return;

        scaling = true;

        Scale += scaleIncrement;

        if (Scale > scaleTop)
            Scale = scaleBottom;

        lastMoveTick = DateTime.UtcNow.Ticks;
        InvokeAsync(StateHasChanged);
    }
    private Task PointerMove(PointerEventArgs args)
    {
        if (!pointerDown)
            return Task.CompletedTask;

        scaling = false;
        lastMoveTick = DateTime.UtcNow.Ticks;

        var movementX = (int)args.PageX - pageX;
        pageX = (int)args.PageX;
        X += movementX;
        var movementY = (int)args.PageY - pageY;
        pageY = (int)args.PageY;
        Y += movementY;

        return Task.CompletedTask;
    }
    private Task PointerUp(PointerEventArgs args)
    {
        if (!pointerDown)
            return Task.CompletedTask;

        timer?.Dispose();
        pointerDown = false;
        scaling = false;

        var movementX = (int)args.PageX - pageX;
        pageX = (int)args.PageX;
        X += movementX;
        var movementY = (int)args.PageY - pageY;
        pageY = (int)args.PageY;
        Y += movementY;
        
        return Task.CompletedTask;
    }
    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the JSCameraModule instance.
    /// </summary>
    protected JSAnnotatedImageModule JSModule { get; private set; }
    /// <summary>
    /// Gets or sets the JS runtime.
    /// </summary>
    [Inject] private IJSRuntime JSRuntime { get; set; }
    /// <summary>
    /// Gets or sets the version provider.
    /// </summary>
    [Inject] private IVersionProvider VersionProvider { get; set; }
    /// <summary>
    /// The absolute or relative URL of the image.
    /// </summary>
    [Parameter] public string Source { get; set; }
    /// <summary>
    /// Alternate text for an image.
    /// </summary>
    [Parameter] public string Text { get; set; }
    /// <summary>
    /// Forces an image to take up the whole width.
    /// </summary>
    [Parameter] public bool Fluid { get; set; }
    [Parameter] public int X { get; set; }
    [Parameter] public int Y { get; set; }
    [Parameter] public int ImageWidth { get;set; }
    [Parameter] public int ImageHeight { get; set; }
    [Parameter] public double Scale { get; set; } = 1.0;    

    #endregion

}
