namespace BlazorApp
{
    public interface ISceneGraph //: ISceneGraph
    {
        double CameraHeight { get; set; }
        double Depth { get; set; }
        double ImageRatio { get; set; }
        string Source { get; set; }
        Dictionary<string, ISceneItem> SceneItems { get; set; }
    }
}