using Blazorise.AnnotatedImage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp;

public static class SceneGraphExtensions
{
    public static void SceneToAnnotation(this ISceneGraph sceneGraph, string id)
    {
        if (sceneGraph?.SceneItems?.TryGetValue(id, out ISceneItem? sceneItem) ?? false)
        {
            sceneItem.ScenePos.X = sceneItem.X;
            sceneItem.ScenePos.Y = sceneItem.Y;
            return;
        }
        else
        {
            throw new Exception($"Can't find key 'id' in SceneGraph.SceneItems, or SceneGraph was null");
        }
    }
    public static void AnnotationToScene(this SceneGraph sceneGraph, string id, AnnotatedImage annotatedImage)
    {
        if (!sceneGraph.SceneItems.TryGetValue(id, out ISceneItem? sceneItem)
            || !annotatedImage.Annotations.TryGetValue(id, out IImageAnnotationData? annotationData))
            return;

        sceneItem.ScenePos.X = annotationData.X;
        sceneItem.ScenePos.Y = annotationData.Y;   
    }
} 