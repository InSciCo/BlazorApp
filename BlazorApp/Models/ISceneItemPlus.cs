namespace BlazorApp
{
    public interface ISceneItemPlus
    {
        double Height { get; set; }
        double HeightOffFloor { get; set; }
        string? Id { get; set; }
        string ImageURL { get; set; }
        string Name { get; set; }
        string? Note { get; set; }
        double Order { get; set; }
        double Scale { get; set; }
        ScenePos ScenePos { get; set; }
        bool Selected { get; set; }
        string? Source { get; set; }
        double Width { get; set; }
        double X { get; set; }
        double Y { get; set; }

        bool Equals(object? obj);
        bool Equals(SceneItem? other);
        bool Equals(SceneItemPlus? other);
        int GetHashCode();
        string ToString();
    }
}