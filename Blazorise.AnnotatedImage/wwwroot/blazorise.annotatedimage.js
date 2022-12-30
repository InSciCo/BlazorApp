
export function initialize() {
    
}

export function setPointerCapture(element, pointerid) {
    element.setPointerCapture(pointerid);
}


export function getImgWidth(elm) {
    return elm.width;
}

export function getImgHeight(elm) {
    return elm.height;
}

export function canvasElementToDataURL(elm) {
    return elm.toDataURL('image/png');
}

export function getBoundingClientRect(elm) {
    return elm.getBoundingClientRect();
}


export function getBase64Image(img) {
    // Create an empty canvas element
    var canvas = document.createElement("canvas");
    //var canvas = new OffscreenCanvas(img.width, img.height);
    canvas.width = img.naturalWidth;
    canvas.height = img.naturalHeight;

    // Copy the image contents to the canvas
    var ctx = canvas.getContext("2d");
    ctx.drawImage(img, 0, 0);

    // Using default image/png becuase Safari doesn't suppor tthe type argument'
    var dataURL = canvas.toDataURL(); 
    return dataURL.replace(/^data:image\/(png|jpg);base64,/, "");
}

var _mergeCanvas;

export function createMergeCanvas(img) {
    _mergeCanvas = document.createElement("canvas");
    _mergeCanvas.width = img.width;
    _mergeCanvas.height = img.height;
    var ctx = _mergeCanvas.getContext("2d");
    ctx.drawImage(img, 0, 0);
}

var _annotationCanvas;

export function addAnnotation(img, x, y, width, height) {
    _annotationCanvas = document.createElement("canvas");
    _annotationCanvas.width = img.naturalWidth;  // width of original image
    _annotationCanvas.height = img.naturalHeight; // height of original image
    var annotationCtx = _annotationCanvas.getContext("2d");
    var hscale = width / img.naturalWidth;
    var vscale = height / img.naturalHeight;
    //annotationCtx.transform(hscale, 0, 0, vscale, 0, 0);
    annotationCtx.scale(hscale, vscale);
    annotationCtx.drawImage(img, 0, 0);
    //var imageData = annotationCtx.getImageData(0, 0, img.naturalWidth, img.naturalHeight);
    var imageData = annotationCtx.getImageData(0, 0,width,height);
    var ctx = _mergeCanvas.getContext("2d");
    ctx.globalCompositeOperation = "source-over";
    ctx.putImageData(imageData, x - width / 2, y - height / 2);
}

export function getMergeImageURL() {
    if (!_mergeCanvas)
        return '';
    return _mergeCanvas.toDataURL();
}



