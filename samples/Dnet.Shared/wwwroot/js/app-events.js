(function () {
    window.appEvents = {

        onscroll: element => {
            element.addEventListener("onscroll", e => { console.log(e) });
        }
    };
})();
