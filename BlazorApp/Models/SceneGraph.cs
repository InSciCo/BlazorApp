using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BlazorApp.AnnotatedImagePage;

namespace BlazorApp;

public class SceneGraph : ISceneGraph
{
    public string ImageURL { get; set; }
    public double ImageRatio { get; set; } // 16:9 etc. 

    // these values are in ft
    public double Depth { get; set; } // Distance Camera to back wall
    public double CameraHeight { get; set; }
    public Dictionary<string, SceneItem> SceneItems { get; set; } = new();
}