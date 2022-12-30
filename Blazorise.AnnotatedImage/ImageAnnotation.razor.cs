#region Using directives
using Blazorise.Extensions;
using Blazorise.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Microsoft.VisualBasic;
#endregion

namespace Blazorise.AnnotatedImage;

public interface IImageAnnotationData
{
    public double X { get; set; }
    public double Y { get; set; }
    public string Source { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    public string Name { get; set; }
    public double Scale { get; set; }
}

public enum PointerState { None, Single, Double }
public partial class ImageAnnotation : BaseComponent, IAsyncDisposable
{
    #region Members
    private string containerPos => $"top:{y}px; left:{x}px; width:{imageWidth}px; height:{imageHeight}px;";
    private double pageX;
    private double pageY;
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
    private double imageHeight => ImageAnnotationData!.Scale * ImageAnnotationData.Height;
    private double imageWidth => ImageAnnotationData!.Scale * ImageAnnotationData.Width;
    private double x => ImageAnnotationData!.X - imageWidth / 2.0;
    private double y => ImageAnnotationData!.Y - imageHeight / 2.0;
    private double xCenterOffset;
    private double yCenterOffset;   
    //private ElementReference elementRef;
    private PointerState pointerState;
    #endregion

    #region Methods
    protected override void BuildClasses(ClassBuilder builder)
    {
        builder.Append("imageannotation");
        base.BuildClasses(builder);
    }
    /// <inheritdoc/>
    protected override Task OnInitializedAsync()
    {
        if (JSModule == null)
            JSModule = new JSAnnotatedImageModule(JSRuntime!, VersionProvider!);
        return base.OnInitializedAsync();
    }
    /// <inheritdoc />
    protected override async Task OnFirstAfterRenderAsync()
    {
        await JSModule!.Initialize();
        Console.WriteLine($"ImgRef:{ImgRef.Id}");
        var yada = await JSModule!.GetBase64Image(ImgRef);
        Console.WriteLine(yada.ToString());
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
        pageX = args.PageX;
        pageY = args.PageY;
        CalculateCenterOffset(args.PageX, args.PageY);
        pointerDown = true;
        scaling = false;
        lastMoveTick = DateTime.UtcNow.Ticks;
        var autoEvent = new AutoResetEvent(false);
        timer = new(OnTimedEvent!, autoEvent, scaleLag, timerInterval);
        await JSModule!.SetPointerCapture(ElementRef, args.PointerId);
    }
    private void OnTimedEvent(Object state)
    {
        if (!pointerDown)
            return;

        var elapsedTime = (DateTime.UtcNow.Ticks - lastMoveTick) / 1000;
        Console.WriteLine(elapsedTime.ToString());
        if (!scaling && (elapsedTime < scaleLag))
            return;

        scaling = true;

        ImageAnnotationData.Scale += scaleIncrement;

        if (ImageAnnotationData.Scale > scaleTop)
            ImageAnnotationData.Scale = scaleBottom;

        lastMoveTick = DateTime.UtcNow.Ticks;
        InvokeAsync(StateHasChanged);
    }
    private Task PointerMove(PointerEventArgs args)
    {
        if (!pointerDown)
            return Task.CompletedTask;

        scaling = false;
        lastMoveTick = DateTime.UtcNow.Ticks;

        CalculateMovement(args.PageX, args.PageY);

        return Task.CompletedTask;
    }
    private Task PointerUp(PointerEventArgs args)
    {
        if (!pointerDown)
            return Task.CompletedTask;

        timer?.Dispose();
        pointerDown = false;
        scaling = false;

        CalculateMovement(args.PageX, args.PageY);

        return Task.CompletedTask;
    }

    private void CalculateMovement(double x, double y)
    {

        if(CanvasRect != null)
        {
            if(x + xCenterOffset < CanvasRect.Left) x= CanvasRect.Left + xCenterOffset;
            if(x + xCenterOffset > CanvasRect.Right) x= CanvasRect.Right + xCenterOffset;     
            if(y + yCenterOffset < CanvasRect.Top) y= CanvasRect.Top + yCenterOffset;
            if(y + yCenterOffset > CanvasRect.Bottom) y= CanvasRect.Bottom + yCenterOffset; 
        }

        ImageAnnotationData.X += x - pageX;
        ImageAnnotationData.Y += y - pageY;

        pageX = x;
        pageY = y;
    }
    private async void CalculateCenterOffset(double x, double y)
    {
        var imgRect = await JSModule!.GetBoundingClientRect(ElementRef);
        xCenterOffset = x - imgRect.Left - (imgRect.Width / 2) ;
        yCenterOffset = y - imgRect.Top  - (imgRect.Height / 2) ;
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
    public ElementReference ImgRef { get; private set; }    
    [Inject] private IJSRuntime? JSRuntime { get; set; }
    /// <summary>
    /// Gets or sets the version provider.
    /// </summary>
    [Inject] private IVersionProvider? VersionProvider { get; set; }
    /// <summary>
    /// The absolute or relative URL of the image.
    /// </summary>
    [Parameter] public string Source { get; set; } = string.Empty;
    /// <summary>
    /// Alternate text for an image.
    /// </summary>
    [Parameter] public string Text { get; set; } = string.Empty;
    /// <summary>
    /// Forces an image to take up the whole width.
    /// </summary>
    [Parameter] public bool Fluid { get; set; }

    [Parameter] public IImageAnnotationData? ImageAnnotationData { get; set; } 
    [Parameter] public BoundingClientRect? CanvasRect { get; set; }  

    #endregion

}
