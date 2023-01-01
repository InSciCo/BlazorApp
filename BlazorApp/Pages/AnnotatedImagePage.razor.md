
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
