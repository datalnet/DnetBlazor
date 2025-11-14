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

                        var resultResizeData = getResizeData(resizer.resizerType, mouseupEvent.clientX, mouseupEvent.clientY, startX, startY, imgWidth, imgHeight, resizerMinWidth, resizerMinHeight);

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

                            return getResizeData(resizer.resizerType, mouseMoveEvent.clientX, mouseMoveEvent.clientY, startX, startY, imgWidth, imgHeight, resizerMinWidth, resizerMinHeight);
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

    function getResizeData(resizerType, clientX, clientY, startX, startY, imgWidth, imgHeight, resizerMinWidth, resizerMinHeight) {

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

        // Clamp values to ensure crop area stays within image bounds
        // Ensure minimum size
        width = Math.max(width, resizerMinWidth);
        height = Math.max(height, resizerMinHeight);
        
        // Ensure left boundary
        left = Math.max(0, left);
        
        // Ensure top boundary  
        top = Math.max(0, top);
        
        // Ensure right boundary (left + width <= imgWidth)
        if (left + width > imgWidth) {
            width = imgWidth - left;
        }
        
        // Ensure bottom boundary (top + height <= imgHeight)
        if (top + height > imgHeight) {
            height = imgHeight - top;
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
        
        const photon = await window.photonInit.get();
        
        // Create canvas with the original image
        var sourceCanvas = document.createElement('canvas');
        var sourceCtx = sourceCanvas.getContext('2d');
        sourceCanvas.width = imageElement.naturalWidth || imageElement.width;
        sourceCanvas.height = imageElement.naturalHeight || imageElement.height;
        sourceCtx.drawImage(imageElement, 0, 0);
        
        // Validate crop parameters
        const x1 = Math.max(0, Math.min(Math.round(cropLeft), sourceCanvas.width - 1));
        const y1 = Math.max(0, Math.min(Math.round(cropTop), sourceCanvas.height - 1));
        const w = Math.max(1, Math.min(Math.round(cropWidth), sourceCanvas.width - x1));
        const h = Math.max(1, Math.min(Math.round(cropHeight), sourceCanvas.height - y1));
        
        // Photon crop expects x1, y1, x2, y2 (NOT x, y, width, height!)
        const x2 = x1 + w;
        const y2 = y1 + h;
        
        // Convert canvas to PhotonImage
        let photonImage = photon.open_image(sourceCanvas, sourceCtx);
        
        // Crop using Photon (high quality) - parameters are (image, x1, y1, x2, y2)
        let croppedImage = photon.crop(photonImage, x1, y1, x2, y2);
        
        // Resize if needed using Photon's resize with Lanczos3 sampling
        let finalImage = croppedImage;
        if (targetWidth && targetHeight) {
            const resizeW = Math.max(1, Math.min(Math.round(targetWidth), 8192)); // Max 8K dimension
            const resizeH = Math.max(1, Math.min(Math.round(targetHeight), 8192)); // Max 8K dimension
            
            // Validate buffer size won't overflow (width * height * 4 channels < 2^31)
            const bufferSize = resizeW * resizeH * 4;
            const maxBufferSize = 2147483647; // 2^31 - 1 (safe for 32-bit WASM)
            
            if (bufferSize > maxBufferSize) {
                throw new Error(`Resize dimensions too large: ${resizeW}x${resizeH} would create ${bufferSize} byte buffer (max: ${maxBufferSize})`);
            }
            
            // Photon resize(img, width, height, sampling_filter)
            // SamplingFilter: Nearest=1, Triangle=2, CatmullRom=3, Gaussian=4, Lanczos3=5
            finalImage = photon.resize(croppedImage, resizeW, resizeH, 5); // Lanczos3 for best quality
        }
        
        // Convert PhotonImage back to canvas
        var resultCanvas = document.createElement('canvas');
        resultCanvas.width = finalImage.get_width();
        resultCanvas.height = finalImage.get_height();
        var resultCtx = resultCanvas.getContext('2d');
        
        // Put image data on canvas
        photon.putImageData(resultCanvas, resultCtx, finalImage);
        
        // Return high-quality JPEG
        return resultCanvas.toDataURL('image/jpeg', 0.95);
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