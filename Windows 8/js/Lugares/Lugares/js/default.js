// For an introduction to the Grid template, see the following documentation:
// http://go.microsoft.com/fwlink/?LinkID=232446
(function () {
    "use strict";

    var app = WinJS.Application;
    var activation = Windows.ApplicationModel.Activation;
    var nav = WinJS.Navigation;
    WinJS.strictProcessing();

    app.addEventListener("activated", function (args) {
        if (args.detail.kind === activation.ActivationKind.launch) {
            if (args.detail.previousExecutionState !== activation.ApplicationExecutionState.terminated) {
                // TODO: This application has been newly launched. Initialize
                // your application here.
            } else {
                // TODO: This application has been reactivated from suspension.
                // Restore application state here.
            }

            if (app.sessionState.history) {
                nav.history = app.sessionState.history;
            }
            args.setPromise(WinJS.UI.processAll().then(function () {
                if (nav.location) {
                    nav.history.current.initialPlaceholder = true;
                    return nav.navigate(nav.location, nav.state);
                } else {
                    return nav.navigate(Application.navigator.home);
                }
            }));


            document.getElementById("markItem").onclick = markItem;

            document.getElementById("labelSizeZoomIn").onclick = increaseLabelSize;
            document.getElementById("labelSizeZoomOut").onclick = decreaseLabelSize;
            document.getElementById("textSizeZoomIn").onclick = increaseTextSize;
            document.getElementById("textSizeZoomOut").onclick = decreaseTextSize;
        }
    });

    app.oncheckpoint = function (args) {
        // TODO: This application is about to be suspended. Save any state
        // that needs to persist across suspensions here. If you need to 
        // complete an asynchronous operation before your application is 
        // suspended, call args.setPromise().
        app.sessionState.history = nav.history;
    };

    app.start();
})();

function increaseLabelSize() {
    var titles = document.getElementsByClassName("item-title");
    var subtitles = document.getElementsByClassName("item-subtitle");
    var i;
    for (i = 0; i < titles.length; i++) {
        var prevTitleAttributes = titles[i].getAttribute("style");
        var prevSubtitleAttributes = subtitles[i].getAttribute("style");
        if (prevTitleAttributes != null)
            titles[i].setAttribute("style", prevTitleAttributes + "font-size:20px");
        else
            titles[i].setAttribute("style", "font-size:20px");
        if (prevSubtitleAttributes != null)
            subtitles[i].setAttribute("style", prevSubtitleAttributes + "font-size: 14px");
        else
            subtitles[i].setAttribute("style", "font-size: 14px");
    }
};

function decreaseLabelSize() {
    var titles = document.getElementsByClassName("item-title");
    var subtitles = document.getElementsByClassName("item-subtitle");
    var i;
    for (i = 0; i < titles.length; i++) {
        var prevTitleAttributes = titles[i].getAttribute("style");
        var prevSubtitleAttributes = subtitles[i].getAttribute("style");
        if (prevTitleAttributes != null)
            titles[i].setAttribute("style", prevTitleAttributes + "font-size:14.7px");
        else
            titles[i].setAttribute("style", "font-size:14.7px");
        if (prevSubtitleAttributes != null)
            subtitles[i].setAttribute("style", prevSubtitleAttributes + "font-size: 12px");
        else
            subtitles[i].setAttribute("style", "font-size: 12px");
    }
};

function increaseTextSize() {
    var content = document.getElementsByClassName("item-content");
    content[0].setAttribute("style", "font-size:20px");
};

function decreaseTextSize() {
    var content = document.getElementsByClassName("item-content");
    content[0].setAttribute("style", "font-size:14.6px");
};

function markItem() {
    var titles = document.getElementsByClassName("item-title");
    var subtitles = document.getElementsByClassName("item-subtitle");
    var listView = document.querySelector(".groupeditemslist").winControl;
    var items = listView.selection.getItems();
    var i;
    for (i = 0; i < items._value.length; i++) {
        var key = parseFloat(items._value[i].key);
        if (titles[0].innerHTML != "") {
            if (key == 0) continue;
            key--;
        }

        var prevTitleAttributes = titles[key].getAttribute("style");
        if (prevTitleAttributes != null)
            titles[key].setAttribute("style", prevTitleAttributes + "color:yellow");
        else
            titles[key].setAttribute("style", "color:yellow");
        
        var prevSubtitleAttributes = subtitles[key].getAttribute("style");
        if (prevSubtitleAttributes != null)
            subtitles[key].setAttribute("style", prevSubtitleAttributes + "color:yellow");
        else
            subtitles[key].setAttribute("style", "color:yellow");
    }
    listView.selection.clear();
}
