using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BlazorApp.AnnotatedImagePage;

namespace BlazorApp;

public class SceneGraph
{
    public string ImageURL { get; set; }
    public double ImageRatio { get; set; } // 16:9 etc. 

    // these values are in ft
    public double Depth { get; set; } // Distance Camera to back wall
    public double CameraHeight { get; set; }
    public Dictionary<string, SceneItem> SceneItems { get; set; } = new();
}

public class ScenePos
{
    // these values are in ft
    public double X { get; set; } // floor left to right
    public double Y { get; set; } // floor front to back 
    public double Z { get; set; } // floor up
}

public record SceneItem
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