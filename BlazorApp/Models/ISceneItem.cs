namespace BlazorApp
{
    public interface ISceneItem
    {
        double Height { get; set; }
        double HeightOffFloor { get; set; }
        string ImageURL { get; set; }
        string Name { get; set; }
        ScenePos ScenePos { get; set; }
        double Width { get; set; }

    }
}