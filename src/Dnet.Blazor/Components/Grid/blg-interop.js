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

    function addTouchListeners(elementRef, scrollElementRef, dotNetReference) {
        let startX;

        elementRef.addEventListener('touchstart', function(e) {
            const touch = e.touches[0];
            startX = touch.clientX; // Guarda la posición inicial del toque
        }, false);

        elementRef.addEventListener('touchmove', function(e) {
            const touch = e.touches[0];
            const deltaX = touch.clientX - startX;

            scrollElementRef.scrollLeft -= deltaX;

            // Llama a un método en tu componente Blazor con el deltaX
            dotNetReference.invokeMethodAsync('OnTouchMove', deltaX);

            // Previene el desplazamiento predeterminado para que no interfiera con el desplazamiento de la página
            e.preventDefault();
        }, false);
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