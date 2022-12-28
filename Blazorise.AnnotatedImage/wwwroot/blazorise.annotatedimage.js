
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

export function GetBoundingClientRect(element) {
    return element.GetBoundingClientRect();
}