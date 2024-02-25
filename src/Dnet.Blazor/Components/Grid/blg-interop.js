window.blginterop = (function () {

    const observersByDotNetId = {};

    let touching = false;

    let moved = false;

    let lastTapTime;

    let touchStart = null;

    function findClosestScrollContainer(element) {

        if (!element) {
            return null;
        }

        const style = getComputedStyle(element);

        if (style.overflowY !== 'visible') {
            return element;
        }

        return findClosestScrollContainer(element.parentElement);
    };

    // e1: MouseEvent | Touch, e2: MouseEvent | Touch, pixelCount: number
    function areEventsNear(e1, e2, pixelCount) {
        // by default, we wait 4 pixels before starting the drag

        if (pixelCount === 0) { return false; }

        const diffX = Math.abs(e1.clientX - e2.clientX);
        const diffY = Math.abs(e1.clientY - e2.clientY);

        return Math.max(diffX, diffY) <= pixelCount;
    }

    function checkForDoubleTap() {
        const now = new Date().getTime();

        if (lastTapTime && lastTapTime > 0) {
            // if previous tap, see if duration is short enough to be considered double tap
            const interval = now - lastTapTime;

            if (interval > 500) {
                // dispatch double tap event

                // this stops a tripple tap ending up as two double taps
                lastTapTime = null;
            } else {
                lastTapTime = now;
            }
        } else {
            lastTapTime = now;
        }
    }

    function getActiveTouch(touchList) {
        for (let i = 0; i < touchList.length; i++) {
            const matches = touchList[i].identifier === touchStart.identifier;
            if (matches) {
                return touchList[i];
            }
        }

        return null;
    }

    function addTouchListeners(elementRef, scrollElementRef, dotNetReference) {

        let startX, startY, scrollStartX;

        elementRef.addEventListener('touchstart', function (e) {

            if (touching) {
                return;
            }

            startX = e.touches[0].clientX;

            startY = e.touches[0].clientY;

            scrollStartX = scrollElementRef.scrollLeft;

            touching = true;

            touchStart = e.touches[0];

            moved = false;

            const touchStartCopy = touchStart;

            window.setTimeout(() => {

                const touchesMatch = touchStart === touchStartCopy;

                if (touching && touchesMatch && !moved) {

                    //long tap
                    moved = true;

                }
            }, 500);

        }, { passive: true });

        elementRef.addEventListener('touchmove', function (e) {

            if (!touching) {
                return;
            }

            const activetouch = getActiveTouch(e.touches);
            if (!activetouch) {
                return;
            }

            const eventIsFarAway = !areEventsNear(activetouch, touchStart, 4);

            if (eventIsFarAway) {
                moved = true;
            }

            const touch = e.touches[0]; // Obtiene la primera posición táctil

            console.log("startX", startX);

            console.log("touch.clientX", touch.clientX);

            // const deltaX = startX - touch.clientX; // Calcula el cambio en la posición X desde el toque inicial
            const deltaX = touch.clientX - startX;

            const deltaY = startY - touch.clientY; // Calcula el cambio en Y

            const umbral = 10; // Umbral para diferenciar entre movimientos leves y significativos

            if (Math.abs(deltaX) > Math.abs(deltaY)) {
                if (Math.abs(deltaX) > umbral) {
                    e.preventDefault();

                    // El movimiento es principalmente horizontal

                    var maxScrollLeft = scrollElementRef.scrollWidth - scrollElementRef.clientWidth; // Calcula el máximo scrollLeft

                    console.log("deltaX",deltaX);

                    scrollElementRef.scrollLeft = scrollStartX - deltaX;

                    // scrollElementRef.scrollBy({left: deltaX }); //deltaX positivo desplaza hacia la derecha

                    var elementScrollLeft = scrollElementRef.scrollLeft; // Obtiene el scrollLeft actual del elemento

                    console.log("elementScrollLeft", elementScrollLeft);

                    // Comprueba si se intenta desplazar más allá del inicio o el final y previene el desplazamiento del contenido
                    if ((elementScrollLeft === 0 && deltaX < 0) || (elementScrollLeft >= maxScrollLeft && deltaX > 0)) {

                        console.log("Ejecucion detenida");
                        return; // Detiene la ejecución adicional para evitar ajustar scrollLeft innecesariamente
                    }
                    else {
                        var scrollInfo = {
                            maxScrollLeft: maxScrollLeft,
                            deltaX: deltaX,
                            elementScrollLeft: elementScrollLeft
                        };

                        // Llama a un método .NET si es necesario. Asegúrate de que dotNetReference está definido y es válido.
                        

                        dotNetReference.invokeMethodAsync('OnTouchMove', scrollInfo);
                    }
                }
            } else {
                if (Math.abs(deltaY) > umbral) {
                    // Movimiento vertical
                    if (deltaY < 0) {
                        // console.log('Movimiento hacia arriba');
                        return;
                    } else {
                        // console.log('Movimiento hacia abajo');
                        return;
                    }
                }
            }

            // Considera el movimiento como diagonal si no es claramente horizontal o vertical
            if (Math.abs(deltaX) > umbral && Math.abs(deltaY) > umbral) {
                // console.log('Movimiento diagonal');
                return;
            }

        }, { passive: false });

        elementRef.addEventListener('touchend', function (e) {

            if (!touching) {
                return;
            }

            if (!moved) {
                //Tap event
                checkForDoubleTap();
            }

            // stops the tap from also been processed as a mouse click
            // if (preventMouseClick && e.cancelable) {
            //     e.preventDefault();
            // }

            touching = false;

        }, { passive: true });

    };

    return {

        addTouchListeners: function (elementRef, scrollElementRef, dotNetReference) {
            addTouchListeners(elementRef, scrollElementRef, dotNetReference);
            return true;
        },

        addWindowEventListeners: function (elementRef, dotnetClass) {

            elementRef.addEventListener("mouseleave", function () {
                dotnetClass.invokeMethodAsync('MouseLeave');
            });

            return true;
        },

        getElementScrollLeft: function (elementRef) {
            return elementRef.scrollLeft;
        },

        getBoundingClientRect: function (elementRef) {
            return elementRef.getBoundingClientRect();
        },

        getElementScrollWidth: function (elementRef) {
            let scrollWidth = elementRef.scrollWidth;

            return scrollWidth;
        },

        getHeaderWidth: function (id) {

            let parent = document.getElementById(id);
            let elements = parent.getElementsByClassName("blg-header-cell blg-header-cell-notpinned");

            let headerWidth = 0;

            for (var i = 0; i < elements.length; i++) {
                headerWidth = headerWidth + elements[i].clientWidth;
            }

            return headerWidth;
        },

        init: function (dotNetHelper, spacerBefore, spacerAfter, rootMargin = 50) {

            const scrollContainer = findClosestScrollContainer(spacerBefore);
            (scrollContainer || document.documentElement).style.overflowAnchor = 'none';

            const intersectionObserver = new window.IntersectionObserver(intersectionCallback, {
                root: scrollContainer,
                rootMargin: `${rootMargin}px`,
                threshold: 0
            });

            intersectionObserver.observe(spacerBefore);

            intersectionObserver.observe(spacerAfter);

            const mutationObserverBefore = createSpacerMutationObserver(spacerBefore);

            const mutationObserverAfter = createSpacerMutationObserver(spacerAfter);

            observersByDotNetId[dotNetHelper._id] = {
                intersectionObserver,
                mutationObserverBefore,
                mutationObserverAfter
            };

            function createSpacerMutationObserver(spacer) {
                // Without the use of thresholds, IntersectionObserver only detects binary changes in visibility,
                // so if a spacer gets resized but remains visible, no additional callbacks will occur. By unobserving
                // and reobserving spacers when they get resized, the intersection callback will re-run if they remain visible.
                const mutationObserver = new window.MutationObserver(() => {
                    intersectionObserver.unobserve(spacer);
                    intersectionObserver.observe(spacer);
                });

                mutationObserver.observe(spacer, { attributes: true });

                return mutationObserver;
            }

            function intersectionCallback(entries) {

                entries.forEach((entry) => {

                    if (!entry.isIntersecting) {
                        return;
                    }

                    const spacerBeforeRect = spacerBefore.getBoundingClientRect();
                    const spacerAfterRect = spacerAfter.getBoundingClientRect();
                    const spacerSeparation = spacerAfterRect.top - spacerBeforeRect.bottom;
                    const containerSize = entry.rootBounds.height;

                    if (entry.target === spacerBefore) {
                        dotNetHelper.invokeMethodAsync('OnSpacerBeforeVisible', entry.intersectionRect.top - entry.boundingClientRect.top, spacerSeparation, containerSize);
                    }

                    else if (entry.target === spacerAfter && spacerAfter.offsetHeight > 0) {
                        // When we first start up, both the "before" and "after" spacers will be visible, but it's only relevant to raise a
                        // single event to load the initial data. To avoid raising two events, skip the one for the "after" spacer if we know
                        // it's meaningless to talk about any overlap into it.
                        dotNetHelper.invokeMethodAsync('OnSpacerAfterVisible', entry.boundingClientRect.bottom - entry.intersectionRect.bottom, spacerSeparation, containerSize);
                    }
                });
            }
        },

        dispose: function (dotNetHelper) {

            const observers = observersByDotNetId[dotNetHelper._id];

            if (observers) {

                observers.intersectionObserver.disconnect();
                observers.mutationObserverBefore.disconnect();
                observers.mutationObserverAfter.disconnect();

                dotNetHelper.dispose();

                delete observersByDotNetId[dotNetHelper._id];
            }
        }
    };
})();