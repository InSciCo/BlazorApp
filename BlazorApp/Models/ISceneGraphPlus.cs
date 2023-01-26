namespace BlazorApp
{
    public interface ISceneGraphPlus
    {
        double CameraHeight { get; set; }
        double Depth { get; set; }
        double ImageRatio { get; set; }
        string ImageURL { get; set; }
        Dictionary<string, ISceneItemPlus> SceneItems { get; set; }
    }
}