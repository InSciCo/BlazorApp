
# AnnotationImage 
This page demonstrates creating a pseudo 3d world sceen and using 
the AnnotatedImage Component to render that sceen. We use a simple 
single vanishing point perspective and assume we are building a 
sceen inside a room where we have taken a phone of the room pointing 
directly at a wall. 

## Real World Scene
The Scenegraph is comprised of a background image and zero or more 
SceneItems. The background and SceneItems are "billboards", meaning 
they are flat images. These images are scaled and skewed to fit the real 
world measures provided. 

### Real World Coordinates
The "ground" is along the XY plane. The Z axis is positive in the up
direction. See: https://www.storyofmathematics.com/3d-coordinate-system/

### Background
Since our application is primarily used to position items in a "room", 
we make some simplifying assumptions about the world space and the 
background image. The image is assumed to have been taken pointing down the 
X axis (in a negative x axis direction) at a wall in the room.
- The wall is in the ZY plane at X=0;
- The middle of the wall is Y=0.
- A user specified Baseboard line in the background image corrosponds to Z = 0. 

### SceneScale (Background image px/ft)
SceneScale is calculated using these values:
- Screenview distance (positive X value) (see Sceneview below)
- Background Baseboard line
	- the number of pixels from the line to the bottom of the image equals the Screenview distance in feet

### Sceneview
The Sceneview shows Scenegraph on a ZY plane a positive X distance based on a 
perspective calculated on a single vanishing point (see Perspective below).
The center of the Sceneview is:
Y = 0;
Z = 5.5ft (roughly eye height)

### Perspective
Perspective is calculated using a vanishing point. This is a 
point in the ZY plane at some large negative X value. The perspective 
point is
Y= 0ft
Z= 5.5ft (roughly eye height)
X=-1000ft (we may adjust this based on testing)

SceneItem billboard images are scaled and skewed based on the vanishing point.
- The corners of each image are calculated. 
- These four corners define the transform of the image.

### SceneItems
SceneItems contain a billboard image and have a corrosponding 
Width and Height in feet. This image will be placed 
on the ZY plane at some positive X value (e.g. distance from 
wall).


### AnnotatedImage DOM Cooridinates and Scaling
The image is presented on an XY plane with Z plane pointing at 
Viewer. This means the X=0,Y=0 point is the top left of the 
DOM Imaage. We just scaling the Sceneview to the DOM view, they 
always have the same image ratio because they both use the same
background image.

Rendering:
SceneGraph background -> ScreenView -> AnnotatedImage background
SceneGraph item -> ScreenView item  -> AnnotatationData 

Moving ImageAnnotation
AnnotationData (X,Y) -> ScreenView item (X,Y) 
	- note that item movement is constrained by ClientRect in AnnotatedImage coordinates
	- This is calculated based on item's position on "floor" in SceneGraph
	- For our simple requirements, this is just the rectangle defined by the portion of the 
	background image below the Baseboard line.

#### STA Calculations
- Calculate Billboard image positions and size. 
- Calculate Annotation positions and size.
	- For each SceneItem
		- Create/Update AnnotationItem
		- Translate and Scale SceneGraph ZY plane to DOM XY plane
		- Calculate ClientRect that defines allowable movement in XY plane

#### ATS Calculations - User moves ImageAnnotation
- Moving ImageAnnotation left or right changes real 
- Calculate Billboiard image positions and size.
	- Translate DOM XY plane postion to SceneGraph X position
	- We pass in a Rect that limits the positioning of the annotation 
	  in the XY plane. 

### Notes
Background Image Acquisition
- User takes picture
- User enters distance to back wall
- User positions a horizontal line in image to match the wall floor intersection
This allows us to scale the size of the room we have and use that scale to position 
and size sceen items.

