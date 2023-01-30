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
    public double Order { get; set; }
    public double Scale { get; set; }
    public string Source { get; set; } = string.Empty;
    public double Width { get; set; }
    public double Height { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
    public bool Selected { get; set; }

    // ISceneItem properties
    /// <summary>
    /// Height off floor to bottom of item
    /// </summary>
    public double HeightOffFloor { get; set; }
    /// <summary>
    /// Position of center of object in Scene
    /// </summary>
    public ScenePos ScenePos { get; set; } = new();

}