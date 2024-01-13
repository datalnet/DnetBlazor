window.admindashboardinterop = (function () {

    return {

        get: key => key in localStorage ? JSON.parse(localStorage[key]) : null,

        set: (key, value) => { localStorage[key] = JSON.stringify(value); },

        delete: key => { delete localStorage[key]; },

        getElementScrollLeft: function (elementRef) {
            if (elementRef != null) {
                return elementRef.scrollLeft;
            } else {
                return 0;
            }
        },

        getBoundingClientRect: function (elementRef) {
            if (elementRef != null) {
                return elementRef.getBoundingClientRect();
            } else {
                return null;
            }
        },

        getElementScrollWidth: function (elementRef) {
            if (elementRef != null) {
                return elementRef.scrollWidth;
            } else {
                return 0;
            }
        },

        getElementSOffsets: function (elementRef) {

            if (elementRef == null) {
                return {
                    offsetWidth: 0,
                    offsetHeight: 0,
                    offsetTop: 0,
                    offsetLeft: 0
                };
            } else {
                return {
                    offsetWidth: elementRef.offsetWidth,
                    offsetHeight: elementRef.offsetHeight,
                    offsetTop: elementRef.offsetTop,
                    offsetLeft: elementRef.offsetLeft
                };
            }
        }
    };
})();