﻿using Blazorise.AnnotatedImage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp;

public static class SceneGraphExtensions
{
    public static void StoA(this SceneGraph sceneGraph, string id, AnnotatedImage annotatedImage)
    {
        if (!sceneGraph.SceneItems.TryGetValue(id, out SceneItem? sceneItem)
            || !annotatedImage.Annotations.TryGetValue(id, out IImageAnnotationData? annotationData))
            return;

        annotationData.X = sceneItem.ScenePos.X;
        annotationData.Y = sceneItem.ScenePos.Y;
    }
    public static void AtoS(this SceneGraph sceneGraph, string id, AnnotatedImage annotatedImage)
    {
        if (!sceneGraph.SceneItems.TryGetValue(id, out SceneItem? sceneItem)
            || !annotatedImage.Annotations.TryGetValue(id, out IImageAnnotationData? annotationData))
            return;

        sceneItem.ScenePos.X = annotationData.X;
        sceneItem.ScenePos.Y = annotationData.Y;    
    }
} 