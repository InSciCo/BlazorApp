import { ContextManager } from './CanvasContextManager';

namespace Canvas {
  const blazoriseCanvas: string = 'BlazoriseCanvas';
  // define what this extension adds to the window object inside BlazorExtensions
  const extensionObject = {
    Canvas2d: new ContextManager("2d"),
    WebGL: new ContextManager("webgl")
  };

  export function initialize(): void {
    if (typeof window !== 'undefined' && !window[blazoriseCanvas]) {
      // when the library is loaded in a browser via a <script> element, make the
      // following APIs available in global scope for invocation from JS
      window[blazoriseCanvas] = {
        ...extensionObject
      };
    } else {
      window[blazoriseCanvas] = {
        ...window[blazoriseCanvas],
        ...extensionObject
      };
    }
  }
}

Canvas.initialize();
