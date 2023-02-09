using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BlazorApp.AnnotatedImagePage;

namespace BlazorApp;

public class ScenePos
{
    // these values are in ft
    public double X { get; set; } // floor left to right
    public double Y { get; set; } // floor front to back 
    public double Z { get; set; } // floor up
}