using Blazorise.AnnotatedImage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp;

public class SceneItem : ISceneItem
{
    // IImageAnnotationData properties
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Note { get; set; } = string.Empty ;
    public string Source { get; set; } = string.Empty ;
    public ScenePos ScenePos { get; set; } = new();
    public ICanvasInfo? CanvasInfo { get; set; }
}