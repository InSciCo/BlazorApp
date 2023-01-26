﻿namespace BlazorApp
{
    public interface ISceneGraph
    {
        double CameraHeight { get; set; }
        double Depth { get; set; }
        double ImageRatio { get; set; }
        string ImageURL { get; set; }
        Dictionary<string, ISceneItem> SceneItems { get; set; }
    }
}