// For reference, See https://developer.mozilla.org/en-US/docs/Web/API/Media_Capture_and_Streams_API/Taking_still_photos

// |streaming| indicates whether or not we're currently streaming
// video from the camera. Obviously, we start at false.
var streaming = false;

// The various HTML elements we need to configure or control. These
// will be set by the initialize() function.
var video = null;
var canvas = null;

export function initialize(element, canvasElement, mirrorImage) {
    video = element;
    canvas = canvasElement;
    //mirror = mirrorImage;

    navigator.mediaDevices.getUserMedia({ video: true, audio: false })
        .then(function (stream) {
            video.srcObject = stream;
            video.play();
            //mirror image
            if (mirrorImage) {
                video.style.webkitTransform = "scaleX(-1)";
                video.style.transform = "scaleX(-1)";
            }
        })
        .catch(function (err) {
            console.log("An error occurred: " + err);
        });

    video.addEventListener('canplay', function (ev) {
        if (!streaming) {
            streaming = true;
        }
    }, false);
}

export function takepicture() {
    var context = canvas.getContext('2d');
    canvas.width = video.videoWidth;

    // Firefox currently has a bug where the height can't be read from
    // the video, so we will make assumptions if this happens.
    if (isNaN(video.videoHeight)) 
        canvas.height = video.videoWidth / (4 /3 );
    else 
        canvas.height = video.videoHeight;

    context.drawImage(video, 0, 0, video.videoWidth, video.videoHeight);
    var data = canvas.toDataURL('image/png');
    return data;
}

