using Blazorise.AnnotatedImage;

namespace BlazorApp
{
    public interface ISceneItem : IImageAnnotationData
    {
        ScenePos ScenePos { get; set; }

    }
}