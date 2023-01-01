
Placement Modes:
Simulated 3D Placement in a Room
Take picture of room and provide approximate RW width and depth. 
Annotate picture from library of itmes. Items have predefined RW size. 
Selecting item drops it into the center of the image. Item is sized to fit room. 
Moving item left or right doesn't change size of item. 
Moving item down increases size of item.
Moving item up decreases size of item. 


Room
.BackgroundImage taken at 16:9 ratio 1920x1080 
	.Width px - ex: 1920
	.Height px - ex:1080
.WorldSize
	.Width ft - ex: 20ft (user entered - approximate value is expected)
	.Depth ft - ex: 20ft (user entered - approximate value is expected. Default is 1/2 of Width)
	.Height ft = .BackgroundImage.Height px / .BackgroundImage.Width px * .WorldSize.Width ft = 11.25 ft
	.CenterScale = .Depth / .Width 

RoomFixture
.AnnotationImage taken at 16:9 ratio 1920x1080
	.Width px - ex: 1920
	.Height px - ex: 1080
.WorldSize
	.Width ft - ex: 2ft (user entered)
	.Height ft = .AnnotatedImage.Height / .AnnotatedImage.Width * WorldSize.Width ft = 1.125 ft
.WorldPlacement
	.DepthScale - simulate deepth of view (0.25 to 0.75). This value is unitless
	.Width ft = .WorldSize.Width ft * .DepthScale 
	.Height ft = .WorldSize.Height ft * DepthScale 
	.X ft = distance from left of BackgroundImage 
	.Y ft = distance from top of BackgroundImage 

Model Layers 
Component
	AnnotatedImage 
	ImageAnnotation 
ViewModel 
	Room 
	RoomFixture 

When we move items around in the component we need to call a method in the ViewModel to calculate the changes in size etc.

The Components are strictly 2D and know nothing about the 3D simulation. 
The AnnotatedImage.OnImageAnnotationMoved callback allows us to update the ImageAnnotationSize as it is being moved.



WorldSize ft = PictureSize px * PictureWorldScale px/ft * Scale
	- Scale is an optional value used for non-coordinate related scaling. It is unitless
PixelSize px = WorldSize ft *  WorldPixelScale ft/px 
DisplaySize px = PixelSize * DisplayScale
	- DisplayScale is PixelSize / DisplaySize (DOM scaling) it is unitless

Process
- Enter image items in WorldSize 
	- AnnotatedImage background 
		- User enters approximate width, in feet, of background image
		- PictureWorldScale px/ft calculated
	- ImageAnnotation
		- User enters approximate width, in feet, of annotation image (or height)
		- PictureWorldScale calculated 
- Position annotations relative to top left corner of AnnoatedImage background in World Units
- Render Merged Image 
	- User selects RenderWidth in pixels
	- Render WorldWidth = RenderWidth px / AnnoatedImage.PictureWorldScale px/ft
	- ImageAnnotations are scaled 






AnnotatedImage
	.OriginalSize - the original size of the background image (should be highest resolution for best results)
		.Width px
		.Height px
	.WorldScale px/ft = .OriginalSize.Width px / picturewidth ft 
		- Imagewidth entered by User (is there a cool way of doing this?)
		- this is an indication of the real world scale of the picture to Annotation Images
		- Its probably best to just let the user eyeball this. As the WorldScale is changed, all
		  the annotations will grow/shrink together.
	.WorldSize ft
		.Width ft = .OriginalSize.Width px / .WorldScale px/ft
		.Height ft = .OriginalSize.Height px / .WorldScale px/ft
	.DisplaySize - the display size of the background image as rendered in DOM
		.Width px
		.Height px
		For reference:
		.Width px = .WorldSize.Width ft / .WorldScale px/ft * .DisplayScale
		.Height px = .WorldSize.Height ft / .WorldScale px/ft * .DisplayScale
	.DisplayScale px/px = .DisplaySize.Width px / .OriginalSize.Width px


ImageAnnotation
	.OriginalSize - the original size of the image (should be highest resolution for best results)
		.Width px
		.Height px 
	.WorldScale px/ft = .OriginalSize.Width px / picturewidth ft
		- or = .OriginaSize.Height / pictureheight ft
		- Entered by user (width or height of item in ft or meters)
		- We do this so different annotation items can have different relative sizes when placed in world image
		- Image should be cropped to edges for width if width entered
		- Image should be cropped to edges for height if height entered
		- Only one of width or height can be provided
	.WorldSize ft
		.Width ft = .OriginalSize.Width px / .WorldScale px/ft 
		.Height ft = .OriginalSize.Height px / .WorldScale px/ft
	.Scale - a generic scale used to create "depth" (defaults to 1.0)
	.DisplaySize - the display size of the background image (DOM pixels)
		.Width px =  .WorldSize.Width ft / AnnotatedImage.WorldScale ft/px * AnnotatedImage.DisplayScale * .Scale 
		.Height px = .WorldSize.Height ft * AnnotatedImage.WorldScale px/ft * AnnottedImate.DisplayScale * .Scale
    .DisplayPos 
		.X px = .WorldPos.X px / AnnotatedImage.WorldScale ft/px * AnnotatedImage.DisplayScale
		.Y px = .WorldPos.Y px / AnnotatedImage.WorldScale ft/px * AnnotatedImage.DisplayScale 
	.WorldPos - the position of the ImageAnnotation center in World units
		.X ft = .DisplayPos.X px / AnnotatedImage.WorldScale px/ft
		.Y ft = .DisplayPos.Y px / AnnotatedImage.WorldScale px/ft 
	
