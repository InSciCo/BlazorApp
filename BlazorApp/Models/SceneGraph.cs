using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BlazorApp.AnnotatedImagePage;

namespace BlazorApp;

public class SceneGraph : ISceneGraph
{
    public double CameraHeight { get; set; }
    public string Source { get; set; } = string.Empty;  
    public double Depth { get; set; } // Distance Camera to back wall
    public double ImageRatio { get; set; } // 16:9 etc. 
    public Dictionary<string, ISceneItem> SceneItems { get; set; } = new();
}