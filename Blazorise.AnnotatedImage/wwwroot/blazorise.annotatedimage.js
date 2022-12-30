﻿
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




