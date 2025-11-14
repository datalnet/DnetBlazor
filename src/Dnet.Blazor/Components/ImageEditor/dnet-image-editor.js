window.dnetimageeditor = (function () {

    var Rx = window['rxjs'];

    var targetLeft = 0;
    var targetTop = 0;
    var targetHeight = 0;
    var targetWidth = 0;

    function initializeDragAndDrop(dotNetHelper, draggedContainerElement, boardArea, initialleft, initialtop) {

        targetLeft = initialleft;
        targetTop = initialtop;
        targetWidth = draggedContainerElement.offsetWidth;
        targetHeight = draggedContainerElement.offsetHeight;

        const areaWidth = boardArea.offsetWidth;
        const areaHeight = boardArea.offsetHeight;

        // Margen horizontal para evitar que los "puntos" de los resizers
        // se metan hacia dentro cuando el área toca los bordes.
        const resizerHorizontalMargin = 6;

        let startX = 0;
        let startY = 0;
        let startLeft = 0;
        let startTop = 0;
        let isDragging = false;
        let pendingFrame = null;
        let currentLeft = targetLeft;
        let currentTop = targetTop;

        const dummy = draggedContainerElement.nextElementSibling &&
            draggedContainerElement.nextElementSibling.classList &&
            draggedContainerElement.nextElementSibling.classList.contains('dnet-crop-box-dummy')
            ? draggedContainerElement.nextElementSibling
            : null;

        function applyTransform(left, top) {

            const clampedLeft = Math.min(
                Math.max(left, resizerHorizontalMargin),
                areaWidth - targetWidth - resizerHorizontalMargin
            );
            const clampedTop = Math.min(Math.max(top, 0), areaHeight - targetHeight);

            currentLeft = clampedLeft;
            currentTop = clampedTop;

            // Usar left/top como fuente de verdad para posición
            draggedContainerElement.style.left = clampedLeft + 'px';
            draggedContainerElement.style.top = clampedTop + 'px';

            if (dummy) {
                dummy.style.left = clampedLeft + 'px';
                dummy.style.top = clampedTop + 'px';
            }

            // Notificar a Blazor en el mismo frame (requestAnimationFrame ya limita a ~60fps)
            dotNetHelper.invokeMethodAsync('OnDrag', {
                height: targetHeight,
                width: targetWidth,
                left: clampedLeft,
                top: clampedTop
            });
        }

        function onMouseMove(e) {

            if (!isDragging) return;

            const newLeft = startLeft + (e.clientX - startX);
            const newTop = startTop + (e.clientY - startY);

            if (pendingFrame == null) {
                pendingFrame = window.requestAnimationFrame(function () {
                    pendingFrame = null;
                    applyTransform(newLeft, newTop);
                });
            }
        }

        function onMouseUp(e) {

            if (!isDragging) return;
            isDragging = false;

            document.removeEventListener('mousemove', onMouseMove);
            document.removeEventListener('mouseup', onMouseUp);

            targetLeft = currentLeft;
            targetTop = currentTop;

            dotNetHelper.invokeMethodAsync('OnDragEnd', {
                height: targetHeight,
                width: targetWidth,
                left: targetLeft,
                top: targetTop
            });
        }

        function onMouseDown(e) {

            e.preventDefault();

            isDragging = true;

            startX = e.clientX;
            startY = e.clientY;
            startLeft = targetLeft;
            startTop = targetTop;

            dotNetHelper.invokeMethodAsync('OnDragStart');

            document.addEventListener('mousemove', onMouseMove);
            document.addEventListener('mouseup', onMouseUp);
        }

        // Posición inicial
        applyTransform(targetLeft, targetTop);

        draggedContainerElement.addEventListener('mousedown', onMouseDown);
    }

    function initializeResize(dotNetHelper, resizers, initialLeft, initialTop, initialHeight, initialWidth, imgWidth, imgHeight, resizerType, resizerMinWidth, resizerMinHeight) {

        targetLeft = initialLeft;
        targetTop = initialTop;
        targetHeight = initialHeight;
        targetWidth = initialWidth;

        for (let resizer of resizers) {

            const mousedownResizer$ = Rx.fromEvent(resizer.reference, 'mousedown');
            const mousemove$ = Rx.fromEvent(document.body, 'mousemove');
            const mouseupResizer$ = Rx.fromEvent(document.body, 'mouseup').pipe(Rx.operators.take(1));

            const mousedragResizer$ = mousedownResizer$.pipe(

                Rx.operators.switchMap((mousedownEvent) => {

                    const startX = mousedownEvent.clientX;
                    const startY = mousedownEvent.clientY;

                    this._resizeEndSub = mouseupResizer$.subscribe((mouseupEvent) => {

                        var resultResizeData = getResizeData(resizer.resizerType, mouseupEvent.clientX, mouseupEvent.clientY, startX, startY);

                        targetLeft = resultResizeData.left;
                        targetTop = resultResizeData.top;
                        targetHeight = resultResizeData.height;
                        targetWidth = resultResizeData.width;

                        dotNetHelper.invokeMethodAsync('OnResizeEnd', { height: targetHeight, width: targetWidth, left: targetLeft, top: targetTop });
                    });

                    mousedownEvent.preventDefault();

                    dotNetHelper.invokeMethodAsync('OnResizeStart');

                    return mousemove$.pipe(

                        Rx.operators.map((mouseMoveEvent) => {

                            mouseMoveEvent.preventDefault();

                            return getResizeData(resizer.resizerType, mouseMoveEvent.clientX, mouseMoveEvent.clientY, startX, startY);
                        }),
                        Rx.operators.takeUntil(mouseupResizer$)
                    );
                }));

            const resizeSub = mousedragResizer$.pipe(
                Rx.operators.filter((pos) => {
                    return (
                        pos.top + pos.height) <= imgHeight &&
                        (pos.left + pos.width) <= imgWidth &&
                        pos.height > resizerMinHeight &&
                        pos.top >= 0 && pos.left >= 0;
                })).subscribe((pos) => {
                    dotNetHelper.invokeMethodAsync('OnResize', pos);
                });
        }
    }

    function getResizeData(resizerType, clientX, clientY, startX, startY) {

        let height;
        let width;
        let top;
        let left;

        switch (resizerType) {

            case "TopLeft":
                height = targetHeight + (startY - clientY);
                width = targetWidth + (startY - clientY);
                top = targetTop - (startY - clientY);
                left = targetLeft - (startY - clientY);
                break;

            case "TopRight":
                height = targetHeight + (startY - clientY);
                width = targetWidth + (startY - clientY);
                top = targetTop - (startY - clientY);
                left = targetLeft;
                break;

            case "BottomLeft":
                height = targetHeight + (startX - clientX);
                width = targetWidth + (startX - clientX);
                top = targetTop;
                left = targetLeft - (startX - clientX);
                break;

            case "BottomRight":
                height = targetHeight - (startY - clientY);
                width = targetWidth - (startY - clientY);
                top = targetTop;
                left = targetLeft;
                break;

            case "TopCenter":
                height = targetHeight + (startY - clientY);
                width = targetWidth;
                top = targetTop - (startY - clientY);
                left = targetLeft;
                break;

            case "BottomCenter":
                height = targetHeight - (startY - clientY);
                width = targetWidth;
                top = targetTop;
                left = targetLeft;
                break;

            case "RightCenter":
                height = targetHeight;
                width = targetWidth - (startX - clientX);
                top = targetTop;
                left = targetLeft;
                break;

            case "LeftCenter":
                height = targetHeight;
                width = targetWidth + (startX - clientX);
                top = targetTop;
                left = targetLeft - (startX - clientX);
                break;

            default:
                break;
        }

        return {
            height: height,
            width: width,
            top: top,
            left: left
        };
    }

    async function cropWithPhoton(imageElement, cropLeft, cropTop, cropWidth, cropHeight, targetWidth, targetHeight) {

        if (!imageElement) {
            throw new Error('Image element is required');
        }

        // Offscreen canvas for crop+resize
        var canvas = document.createElement('canvas');
        var ctx = canvas.getContext('2d');

        canvas.width = Math.max(1, Math.round(cropWidth));
        canvas.height = Math.max(1, Math.round(cropHeight));

        ctx.drawImage(
            imageElement,
            cropLeft,
            cropTop,
            cropWidth,
            cropHeight,
            0,
            0,
            canvas.width,
            canvas.height
        );

        // Nota: en esta rama tratamos el WASM como recurso estático.
        // Una integración completa con Photon requerirá inicializar explícitamente
        // el módulo wasm desde una URL conocida y usar sus helpers aquí.
        // Mientras tanto, devolvemos el recorte usando sólo canvas.

        // Resize to preview / final size usando canvas 2D
        if (targetWidth && targetHeight) {
            var resizedCanvas = document.createElement('canvas');
            var resizedCtx = resizedCanvas.getContext('2d');

            resizedCanvas.width = Math.max(1, Math.round(targetWidth));
            resizedCanvas.height = Math.max(1, Math.round(targetHeight));

            resizedCtx.drawImage(
                canvas,
                0,
                0,
                canvas.width,
                canvas.height,
                0,
                0,
                resizedCanvas.width,
                resizedCanvas.height
            );

            canvas = resizedCanvas;
        }

        // JPEG data URL with good quality
        return canvas.toDataURL('image/jpeg', 0.95);
    }

    return {

        setFocus: function (element) {

            if (element) element.focus();
        },

        getBoundingClientRect: function (elementRef) {

            const clientRect = elementRef.getBoundingClientRect();

            return clientRect;
        },

        initializeDragAndDrop: function (dotNetHelper, draggedContainerElement, boardArea, draggedData, left, top) {
            initializeDragAndDrop(dotNetHelper, draggedContainerElement, boardArea, draggedData, left, top);
        },

        initializeResize: function (dotNetHelper, resizers, initialLeft, initialTop, initialHeight, initialWidth, imgWidth, imgHeight, resizerType, resizerMinWidth, resizerMinHeight) {
            initializeResize(dotNetHelper, resizers, initialLeft, initialTop, initialHeight, initialWidth, imgWidth, imgHeight, resizerType, resizerMinWidth, resizerMinHeight);
        },

        cropWithPhoton: cropWithPhoton
    };
})();