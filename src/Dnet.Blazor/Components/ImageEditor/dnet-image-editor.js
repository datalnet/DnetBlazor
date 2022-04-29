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

        const mousedown$ = Rx.fromEvent(draggedContainerElement, 'mousedown');
        const mousemove$ = Rx.fromEvent(boardArea, 'mousemove');
        const mouseup$ = Rx.fromEvent(document.body, 'mouseup').pipe(Rx.operators.take(1));

        const mousedrag$ = mousedown$.pipe(

            Rx.operators.switchMap((mousedownEvent) => {

                const startX = mousedownEvent.clientX;
                const startY = mousedownEvent.clientY;

                this._dragEndSub = mouseup$.subscribe((mouseupEvent) => {

                    targetLeft = targetLeft + mouseupEvent.clientX - startX;
                    targetTop = targetTop + mouseupEvent.clientY - startY;

                    if (targetLeft < 0) targetLeft = 0;
                    if (targetTop < 0) targetTop = 0;
                    if (targetLeft + targetWidth > areaWidth) targetLeft = areaWidth - targetWidth;
                    if (targetTop + targetHeight > areaHeight) targetTop = areaHeight - targetHeight;

                    dotNetHelper.invokeMethodAsync('OnDragEnd', { height: targetHeight, width: targetWidth, left: targetLeft, top:targetTop });
                });

                dotNetHelper.invokeMethodAsync('OnDragStart');

                return mousemove$.pipe(

                    Rx.operators.map((mouseMoveEvent) => {

                        mouseMoveEvent.preventDefault();

                        let left = targetLeft + mouseMoveEvent.clientX - startX;
                        let top = targetTop + mouseMoveEvent.clientY - startY;

                        if (left < 0) left = 0;
                        if (top < 0) top = 0;
                        if (left + targetWidth > areaWidth) left = areaWidth - targetWidth;
                        if (top + targetHeight > areaHeight) top = areaHeight - targetHeight;

                        return {
                            height: targetHeight,
                            width: targetWidth,
                            left: left,
                            top: top
                        };

                    }),

                    Rx.operators.takeUntil(mouseup$)
                );
            }));

        this._dragSub = mousedrag$.subscribe((pos) => {
            dotNetHelper.invokeMethodAsync('OnDrag', pos);
        });
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
        }
    };
})();