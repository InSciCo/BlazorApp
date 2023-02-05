using Blazorise.AnnotatedImage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp;

public static class SceneGraphExtensions
{
    public static void SceneToAnnotation(this ISceneGraph sceneGraph, string id, AnnotatedImage annotatedImage)
    {

    }
    public static void AnnotationToScene(this ISceneGraph sceneGraph, string id, AnnotatedImage annotatedImage)
    {
        var cameraDistance = sceneGraph.Depth;
        var cameraHeight = sceneGraph.CameraHeight;
        double xPx = annotatedImage.ImgElementWidth;
        double yPx = annotatedImage.ImgElementHeight;
        var baseboadRatio = 0.45;
        if (sceneGraph?.SceneItems?.TryGetValue(id, out ISceneItem? sceneItem) ?? false)
        {
            //SCREEN(x,y) to SCENE<i,j,k>

            //SCREEN (0,0) top left
            //x faces right
            //y faces down

            //SCENE<0,0,0>
            //maxima defined by camera's visiblity
            //j facing right of camera, zero at midpoint (jMin > values > jMax)
            //i facing same direction as camera, zero at back wall (cameraDistance < values < 0)
            //k facing up, zero at floor (kMax > values > 0)

            //Px SCREEN coords
            var x = sceneItem.X; //
            var y = sceneItem.Y; //

            //Ft SCENE coords
            //set maxima
            var kMax = 10.0; // todo - calc, off screen (maybe use centerpoint?)
            var kMin = 0.0; // cord enforced, floor
            var iMax = 0.0; // cord enforced, back wall
            var iMin = -cameraDistance; // coord enforced, camera feet (need  disallow offscreen, too close)
            //baseboard centerpoint to right screen edge, ft length
            //tan(x) = opp / adj
            //tan(fov/2) = baseboard_leg / cameradistance
            //baseboard_leg =  cameradistance * tan(fov/2)
            var jMax = cameraDistance * Math.Tan(0.9861);
            var jMin = -jMax;

            //Assign J
            //need object distance from centerline in ft
            //% of img px as % of baseboard length
            // - jMax to make zero center
            double j = (x / xPx) * (2 * jMax) - jMax;

            //Assign I
            /*
            consider the ray created from camera to object
            if this ray was projected through the floor to the plane of the back wall
            it creates a triangle

            Scene Values
            triangle 1
            F camera
            L screen centerpoint
            A baseboard centerpoint
            triangle 2
            F camera
            L screen centerpoint 
            K intersection of camera-object ray and backwall plane 
            triangle 3
            B object distance from backboard along floor centerline
            A baseboard centerpoint
            K intersection of camera-object ray and backwall plane 
            
            Screen Values
            c screen centerpoint position
            a baseboard position
            b object position
             */
            double c = yPx / 2;
            double a = (1 - baseboadRatio) * yPx;
            double b = y;
            var ab = b - a;
            var ca = a - c;
            var FL = cameraDistance;
            var LA = cameraHeight;
            var AK = ab / ca * LA;
            var BA = (FL * AK) / (LA + AK);

            double i = BA;

            //Assign K
            //up down, unit vector looking up, zero at floor (all values >0)
            double k = 0.0; //assume floor (todo, limit height?)

            sceneItem.ScenePos.X = i;
            sceneItem.ScenePos.Y = j;
            sceneItem.ScenePos.Z = k;
            return;
        }
        else
        {
            throw new Exception($"Can't find key 'id' in SceneGraph.SceneItems, or SceneGraph was null");
        }
    }
} 