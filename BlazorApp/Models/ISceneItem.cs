using Blazorise.AnnotatedImage;

namespace BlazorApp
{
    public interface ISceneItem : IImageAnnotationData
    {
        double HeightOffFloor { get; set; }
        ScenePos ScenePos { get; set; }

    }
}