* FubDub
A simple image annotation app to demonstrate cross-platform Model View ViewModel (MVVM)
pattern. 

Features:
- Take a picture or select a picture from gallery

- Annotate Image 
    - Select sprite library 
        - toys
        - decorations
        - fruits & veggies
    - Drop sprite onto image 
    - Move and size sprite 
        - optional - skew?
    - Double click to open sprite properties dialog
        - Show Label
        - Label value 
        - Note - note is not shown in image
    
- Save Image 
    - select image output size 
    - saves SVG - base image and sprites (allows downstream editing)
    - saves PNG - merged image (allows easy distribution when no downstream editing is required)

Notes:
Pages and Components
    FubDubIntroPage  
        FubDubCameraPage
            Blazorise.Camera
        FubDubAnnotatePage
            Blazorise.AnnotatedImage
        FubDubSavePage 
            Blazorise.Canvas
ViewModels
    FubDubViewModel 



MAUI & WASM File Access

Requirement:
Common interface for file handling across MAUI and WASM targets

Existing Interfaces:
MAUI
	System.IO 
	Maui.Storage
		FileSystem
			FileSystem
				AppDataDirectory
				CacheDirectory 
		SecureStorage (key,value) store 

WASM 
	File System Access API 
		FileSystemHandle 
		FileSystemFileHandle 
		FileSystemDirectoryHandle 
		FileSystemAccessHandle 
		FileSystemWriteableFileStream 


File/Directory Operations 
    File Picker / Read 
    Directory Picker 

    File Create/Write 


https://github.com/jimmywarting/native-file-system-adapter
https://github.com/GoogleChromeLabs/browser-fs-access (BFS)

Idea:
Create C# lib to wrap File System Access API (using one of the two libs above)
Allow use of .NET IO and MAUI FileSystem when in MAUI app 

Interface 
    FileOpen 
        BFS -> fileOpen 
    DirectoryOpen 
        BFS -> directoryOpen 
    FileSave 
        BFS -> fileSave 






