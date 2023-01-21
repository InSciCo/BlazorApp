using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BlazorApp.AnnotatedImagePage;

namespace BlazorApp;

public record SceneItem : ISceneItem
{
    public string Name { get; set; }
    public string ImageURL { get; set; }
    // these values are in ft
    /// <summary>
    /// Width in feet of item
    /// </summary>
    public double Width { get; set; }
    /// <summary>
    /// Height in feet of item
    /// </summary>
    public double Height { get; set; }
    /// <summary>
    /// Height off floor to bottom of item
    /// </summary>
    public double HeightOffFloor { get; set; }
    /// <summary>
    /// Position of center of object in Scene
    /// </summary>
    public ScenePos ScenePos { get; set; }
}