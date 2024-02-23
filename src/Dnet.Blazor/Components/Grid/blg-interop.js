window.blginterop = (function () {

    const observersByDotNetId = {};



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

    function addTouchListeners(elementRef, scrollElementRef, dotNetReference) {

        let startX;

        let startY;

        elementRef.addEventListener('touchstart', function (e) {

            const touch = e.touches[0];

            startX = touch.clientX;

            startY = e.touches[0].clientY;

            console.log("Aqui empieza", startX);

        }, { passive: true });

        elementRef.addEventListener('touchmove', function (e) {
            const touch = e.touches[0]; // Obtiene la primera posición táctil
            console.log("touch.clientX", touch.clientX);

            const deltaX = touch.clientX - startX; // Calcula el cambio en la posición X desde el toque inicial
            console.log("deltaX", deltaX); // Sería bueno también imprimir deltaX para depurar

            const deltaY = touch.clientY - startY; // Calcula el cambio en Y

            const absDeltaX = Math.abs(deltaX);
            const absDeltaY = Math.abs(deltaY);

            const umbral = 10; // Umbral para diferenciar entre movimientos leves y significativos

            if (Math.abs(deltaX) > Math.abs(deltaY)) {
                if (Math.abs(deltaX) > umbral) {
                    // El movimiento es principalmente horizontal
                    console.log('Movimiento principalmente horizontal');

                    var maxScrollLeft = scrollElementRef.scrollWidth - scrollElementRef.clientWidth; // Calcula el máximo scrollLeft
                    console.log("maxScrollLeft", maxScrollLeft);

                    var elementScrollLeft = scrollElementRef.scrollLeft; // Obtiene el scrollLeft actual del elemento
                    console.log("elementScrollLeft", elementScrollLeft);

                    // Comprueba si se intenta desplazar más allá del inicio o el final y previene el desplazamiento del contenido
                    if ((elementScrollLeft === 0 && deltaX > 0) || (elementScrollLeft >= maxScrollLeft && deltaX < 0)) {
                        return; // Detiene la ejecución adicional para evitar ajustar scrollLeft innecesariamente
                    }

                    scrollElementRef.scrollLeft -= deltaX; // Actualiza el scrollLeft del elemento basado en el movimiento táctil

                    // Llama a un método .NET si es necesario. Asegúrate de que dotNetReference está definido y es válido.
                    dotNetReference.invokeMethodAsync('OnTouchMove', deltaX);
                }
            } else {
                if (Math.abs(deltaY) > umbral) {
                    // Movimiento vertical
                    console.log('Movimiento vertical');
                    // Aquí puedes agregar tu lógica para manejar movimientos verticales
                    if (deltaY < 0) {
                        console.log('Movimiento hacia arriba');
                    } else {
                        console.log('Movimiento hacia abajo');
                    }
                }
            }

            // Considera el movimiento como diagonal si no es claramente horizontal o vertical
            if (Math.abs(deltaX) > umbral && Math.abs(deltaY) > umbral) {
                console.log('Movimiento diagonal');
                // Aquí puedes agregar tu lógica para manejar movimientos diagonales
            }

            // if (absDeltaX > absDeltaY) {
            //     // El movimiento es principalmente horizontal
            //     console.log('Movimiento principalmente horizontal');

            //     var maxScrollLeft = scrollElementRef.scrollWidth - scrollElementRef.clientWidth; // Calcula el máximo scrollLeft
            //     console.log("maxScrollLeft", maxScrollLeft);

            //     var elementScrollLeft = scrollElementRef.scrollLeft; // Obtiene el scrollLeft actual del elemento
            //     console.log("elementScrollLeft", elementScrollLeft);

            //     // Comprueba si se intenta desplazar más allá del inicio o el final y previene el desplazamiento del contenido
            //     if ((elementScrollLeft === 0 && deltaX > 0) || (elementScrollLeft >= maxScrollLeft && deltaX < 0)) {
            //         return; // Detiene la ejecución adicional para evitar ajustar scrollLeft innecesariamente
            //     }

            //     scrollElementRef.scrollLeft -= deltaX; // Actualiza el scrollLeft del elemento basado en el movimiento táctil

            //     // Llama a un método .NET si es necesario. Asegúrate de que dotNetReference está definido y es válido.
            //     dotNetReference.invokeMethodAsync('OnTouchMove', deltaX);

            // } else if (absDeltaY > absDeltaX) {
            //     // El movimiento es principalmente vertical
            //     console.log('Movimiento principalmente vertical');

            //     if (deltaY < 0) {
            //         console.log('Movimiento hacia arriba');
            //         // Maneja el movimiento hacia arriba
            //     } else if (deltaY > 0) {
            //         console.log('Movimiento hacia abajo');
            //         // Maneja el movimiento hacia abajo
            //         if (scrollElementRef.scrollTop === 0) {
            //             console.log('Movimiento hacia abajo es válido');
            //         }
            //     }
            // }

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