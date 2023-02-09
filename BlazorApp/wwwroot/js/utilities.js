
window.getInnerWidth = () => window.innerWidth;

window.getInnerHeight = () => window.innerHeight;

window.getWidth = (element) => {
    return element.width;
}
window.getHeight = (element) => {
    return element.height;
}

window.utilityFunctions = {
    canvasElementToDataURL: function (element) {
        return element.toDataURL('image/png');
    }
}


window.canvasElementToDataURL = (element) => {
   return element.toDataURL('image/png');
}
