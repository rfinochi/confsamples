(function () {
    "use strict";

    var appView = Windows.UI.ViewManagement.ApplicationView;
    var appViewState = Windows.UI.ViewManagement.ApplicationViewState;
    var nav = WinJS.Navigation;
    var ui = WinJS.UI;
    var utils = WinJS.Utilities;

    function showAppBar(currentItem) {
        // Get the app bar.
        var element = document.activeElement;
        var appbar = document.getElementById("appbar");

        // Keep the app bar open after it's shown.
        appbar.winControl.sticky = true;

        // Set the app bar context.
        showItemCommands();

        // Show the app bar.
        appbar.winControl.show();

        // Return focus to the original item which invoked the app bar.
        if (element != null) element.focus();
    }

    function hideAppBar() {
        var element = document.activeElement;
        var appbar = document.getElementById("appbar");
        appbar.winControl.sticky = false;
        appbar.winControl.hide();
        hideItemCommands();
        if (element != null) element.focus();
    }

    function showItemCommands() {
        appbar.winControl.showCommands([markItem]);
    }

    function hideItemCommands() {
        appbar.winControl.hideCommands([markItem]);
    }

    ui.Pages.define("/pages/groupedItems/groupedItems.html", {

        // This function updates the ListView with new layouts
        initializeLayout: function (listView, listViewZoomOut, semanticZoom, viewState) {
            /// <param name="listView" value="WinJS.UI.ListView.prototype" />

            if (viewState === appViewState.snapped) {
                listView.itemDataSource = Data.groups.dataSource;
                listView.groupDataSource = null;
                listView.layout = new ui.ListLayout();

                //003
                semanticZoom.zoomedOut = false;
                semanticZoom.forceLayout();
                semanticZoom.locked = true;
            } else {
                listView.itemDataSource = Data.items.dataSource;
                listView.groupDataSource = Data.groups.dataSource;
                listView.layout = new ui.GridLayout({ groupHeaderPosition: "top" });

                //003
                listViewZoomOut.itemDataSource = Data.groups.dataSource;
                listViewZoomOut.layout = new ui.GridLayout({ maxRows: 1 });
                semanticZoom.forceLayout();
                semanticZoom.locked = false;
            }
        },

        itemInvoked: function (args) {
            if (appView.value === appViewState.snapped) {
                // If the page is snapped, the user invoked a group.
                var group = Data.groups.getAt(args.detail.itemIndex);
                nav.navigate("/pages/groupDetail/groupDetail.html", { groupKey: group.key });
            } else {
                // If the page is not snapped, the user invoked an item.
                var item = Data.items.getAt(args.detail.itemIndex);
                nav.navigate("/pages/itemDetail/itemDetail.html", { item: Data.getItemReference(item) });
            }
        },

        itemSelected: function (eventObject) {
            var listView = document.querySelector(".groupeditemslist").winControl;

            // Check for selection.
            if (listView.selection.count() === 0) {
                hideAppBar();
            } else {
                listView.selection.getItems().then(function (items) {
                    showAppBar(items[0]);
                });
            }
        },


        // This function is called whenever a user navigates to this page. It
        // populates the page elements with the app's data.
        ready: function (element, options) {
            var listView = element.querySelector(".groupeditemslist").winControl;

            // Add the following two lines of code.
            var listViewZoomOut = element.querySelector(".groupeditemslistZoomOut").winControl;
            var semanticZoom = element.querySelector(".sezoDiv").winControl;

            listView.groupHeaderTemplate = element.querySelector(".headerTemplate");
            listView.itemTemplate = element.querySelector(".itemtemplate");
            
            //002
            listView.onselectionchanged = this.itemSelected.bind(this);

            //003
            listViewZoomOut.itemTemplate = element.querySelector(".itemtemplate");

            listView.oniteminvoked = this.itemInvoked.bind(this);

            //003
            listViewZoomOut.oniteminvoked = this.groupInvoked.bind(this)

            this.initializeLayout(listView, listViewZoomOut, semanticZoom, appView.value);
            //001 //listView.element.focus();

            appbar.winControl.disabled = false;
            appbar.winControl.hideCommands([textSizeZoomIn, textSizeZoomOut, markItem]);
            appbar.winControl.showCommands([labelSizeZoomIn, labelSizeZoomOut]);

        },

        //003
        groupInvoked: function (args) {
            var group = Data.groups.getAt(args.detail.itemIndex);
            nav.navigate("/pages/groupDetail/groupDetail.html", { groupKey: group.key });
        },

        // This function updates the page layout in response to viewState changes.
        updateLayout: function (element, viewState, lastViewState) {
            /// <param name="element" domElement="true" />
            /// <param name="viewState" value="Windows.UI.ViewManagement.ApplicationViewState" />
            /// <param name="lastViewState" value="Windows.UI.ViewManagement.ApplicationViewState" />

            var listView = element.querySelector(".groupeditemslist").winControl;

            //003
            var listViewZoomOut = element.querySelector(".groupeditemslistZoomOut").winControl;
            var semanticZoom = element.querySelector(".sezoDiv").winControl;

            if (lastViewState !== viewState) {
                if (lastViewState === appViewState.snapped || viewState === appViewState.snapped) {
                    var handler = function (e) {
                        listView.removeEventListener("contentanimating", handler, false);
                        e.preventDefault();
                    }
                    listView.addEventListener("contentanimating", handler, false);

                    //003
                    this.initializeLayout(listView, listViewZoomOut, semanticZoom, viewState);

                    if (lastViewState === appViewState.snapped) {
                        semanticZoom.zoomedOut = true;
                        semanticZoom.forceLayout();
                    }

                    //002.5
                    hideItemCommands();
                    if (viewState === appViewState.snapped)
                        listView.selectionMode = "none";
                    else
                        listView.selectionMode = "multi";
                }
            }
        },


        //Animation
        //001
        getAnimationElements: function () {
            return [[this.element.querySelector("header")], [this.element.querySelector("section")]];
        },

        setPageFocus: function () {
            this.element.querySelector(".groupeditemslist").winControl.element.focus();
        }


    });
})();
