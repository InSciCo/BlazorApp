using Blazorise.AnnotatedImage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BlazorApp.AnnotatedImagePage;

namespace BlazorApp;

public class SceneItemPlus : ISceneItemPlus
{
    public string Name { get; set; }
    public string? Note { get; set; }
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

    //rednering info
    public string? Id { get; set; }
    public double Order { get; set; }
    public double Scale { get; set; }
    public string? Source { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
    public bool Selected { get; set; }

    public bool Equals(SceneItem? other)
    {
        throw new NotImplementedException();
    }
    public bool Equals(SceneItemPlus? other)
    {
        throw new NotImplementedException();
    }
}