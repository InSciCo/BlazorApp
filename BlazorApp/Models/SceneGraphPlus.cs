using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BlazorApp.AnnotatedImagePage;

namespace BlazorApp;

public class SceneGraphPlus : ISceneGraphPlus
{
    public double CameraHeight { get; set; }
    public double Depth { get; set; } // Distance Camera to back wall
    public double ImageRatio { get; set; } // 16:9 etc. 
    public string ImageURL { get; set; }
    public Dictionary<string, ISceneItemPlus> SceneItems { get; set; } = new();
}