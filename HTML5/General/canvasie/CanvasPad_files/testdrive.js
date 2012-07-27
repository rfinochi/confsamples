// Copyright © Microsoft Corporation.  All Rights Reserved.
// This code released under the terms of the 
// Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html).

//  We do some work here to make sure the Test Drive site is visitable by IE8 users,
//  users of older platform preview versions, and folks internally running full
//  builds of IE9.

//  for this code to work, the page must be loaded with document mode [trying to be] IE9
var _isAnyIE = false;
var _isPP = false;
var _isPP1 = false;
var _isPP2 = false;
var _isPP3 = false;
var _isPP4 = false;
var _isIE9 = false;
var _isIE8 = false;
var _isWin7 = false;
var _isVista = false;
var _isXP = false;

function appendRule(ss, selector, ruleBody) {
    if (ss.insertRule)
        ss.insertRule(selector + "{" + ruleBody + "}", ss.cssRules.length)
    else if (ss.addRule)
        ss.addRule(selector, ruleBody);
}

function InitFlags() {
    _isAnyIE = (typeof window.ActiveXObject != 'undefined');
    _isWin7 = window.navigator.userAgent.indexOf("Windows NT 6.1") != -1;
    _isVista = window.navigator.userAgent.indexOf("Windows NT 6.0") != -1;
    _isXP = !_isWin7 && !_isVista && window.navigator.userAgent.indexOf("Windows") != -1;

    //  these document mode checks only work because all of our test drive pages
    //  are specifically asking for 9 mode via the meta tag
    if (_isAnyIE && typeof document.documentMode != 'undefined') {
        if (window.external == null) {
            _isPP = true;

            if (document.hasOwnProperty)
                _isPP4 = true;
            else if (typeof Array.prototype.indexOf != 'undefined')
                _isPP3 = true;
            else if (typeof document.getElementsByClassName != 'undefined')
                _isPP2 = true;
            else
                _isPP1 = true;
        }
        else if (document.documentMode == 9)
            _isIE9 = true;
        else
            _isIE8 = true;
    }

    if (document.styleSheets.length > 0) {
        var ss0 = document.styleSheets[0];

        appendRule(ss0, ".ifIsIE8", "display: " + (_isIE8 ? "block" : "none"));
        appendRule(ss0, ".ifNotIE8", "display: " + (!_isIE8 ? "block" : "none"));

        appendRule(ss0, ".ifIsIE9", "display: " + (_isIE9 ? "block" : "none"));
        appendRule(ss0, ".ifNotIE9", "display: " + (!_isIE9 ? "block" : "none"));

        appendRule(ss0, ".ifIsIE8Or9", "display: " + ((_isIE8 || _isIE9) ? "block" : "none"));
        appendRule(ss0, ".ifNotIE8Or9", "display: " + (!(_isIE8 || _isIE9) ? "block" : "none"));

        if (!_isPP) appendRule(ss0, ".ifIsPP", "display: none");
        if (_isPP) appendRule(ss0, ".ifNotPP", "display: none");

        if (!_isPP1) appendRule(ss0, ".ifIsPP1", "display: none");
        if (_isPP1) appendRule(ss0, ".ifNotPP1", "display: none");

        if (!_isPP2) appendRule(ss0, ".ifIsPP2", "display: none");
        if (_isPP2) appendRule(ss0, ".ifNotPP2", "display: none");

        if (!_isPP3) appendRule(ss0, ".ifIsPP3", "display: none");
        if (_isPP3) appendRule(ss0, ".ifNotPP3", "display: none");
        
        if (!_isPP4) appendRule(ss0, ".ifIsPP4", "display: none");
        if (_isPP4) appendRule(ss0, ".ifNotPP4", "display: none");

        if (!(_isPP && !_isPP4)) appendRule(ss0, ".ifIsOldPP", "display: none");

        if (!(_isIE9 || _isPP)) appendRule(ss0, ".ifIsIE9OrPP", "display: none");
        if (_isIE9 || _isPP) appendRule(ss0, ".ifNotIE9OrPP", "display: none");

        if (!(_isVista || _isWin7)) appendRule(ss0, ".ifIsVistaOrWin7", "display: none");
        if (_isVista || _isWin7) appendRule(ss0, ".ifNotVistaOrWin7", "display: none");
    }
}

//  run this now so the CSS is right when we first layout the page
InitFlags();

var zoom = 0;
var zoomWatermark = null;

function UpdateZoomWatermark() {
    var newZoom = screen.deviceXDPI / screen.logicalXDPI;
    if (newZoom != zoom) {
        var firstCall = (zoom == 0);
        zoom = newZoom;
        zoomWatermark.innerHTML = (zoom * 100).toString() + '%';

        //        if (!firstCall && window.AdjustMap)
        //			window.AdjustMap();
    }
}

function InitOnLoad() {

    //	display this page's URL as seen here on the client if we have such an element
    var tu = document.getElementById("thisUrl");
    if (tu != null) {
        tu.value = window.location.href;
        tu.setAttribute("title", "The Web address of this page is " + window.location.href);
    }

    //  if this is the platform preview, remove from the DOM any hyperlink that shouldn't be there
    //  this fixes bugs an accessility bug with tabbing to hidden hyperlinks and then following them
    if (_isPP) {
        var noPPlinks = document.querySelectorAll("a.ifNotPP");

        for (var i = 0; i < noPPlinks.length; ++i) {
            noPPlinks[i].parentNode.removeChild(noPPlinks[i]);
        }
    }

    //  remove the install link if not running on an operating system where it can possibly run
    if (!(_isVista || _isWin7)) {
        var install = document.getElementById("installnow");
        if (install != null)
            install.parentNode.removeChild(install);
    }

    //	initialize the zoom watermark if we have one (we usually don't)
    zoomWatermark = document.getElementById("zoomwatermark");
    if (zoomWatermark != null) {
        zoomWatermark.style.display = (_isIE8 || _isIE9 || _isPP) ? "block" : "none";

        if (_isIE8 || _isIE9 || _isPP) {
            UpdateZoomWatermark();
            window.setInterval(UpdateZoomWatermark, 1000.0 / 15.0); // update it 15 times a second
        }
    }

    gdid2dWatermark = document.getElementById("gdid2dwatermark");
    if (gdid2dWatermark != null) {
        gdid2dWatermark.style.display = (_isIE9 || _isPP1 || _isPP2) ? "block" : "none";
    }

    //  should probably do something similar for IE7 but it's more work
    if (_isIE8) {
        var noIE8links = document.querySelectorAll("a.noIE8");

        for (var i = 0; i < noIE8links.length; ++i) {
            noIE8links[i].removeAttribute("href");
            noIE8links[i].style.color = "#ccc";
            noIE8links[i].setAttribute("title", "This page does not work in Internet Explorer 8");
        }
    }
    
    if (_isPP1) {
        var noPP1links = document.querySelectorAll("a.noPP1");

        for (var i = 0; i < noPP1links.length; ++i) {
            noPP1links[i].removeAttribute("href");
            noPP1links[i].style.color = "#ccc";
            noPP1links[i].setAttribute("title", "This page does not work in Internet Explorer 9 Platform Preview 1");
        }
    }
}

if (window.addEventListener) {
    window.addEventListener("load", InitOnLoad, false);
}
else if (window.attachEvent) {
    window.attachEvent("onload", InitOnLoad);
}

function RemoveMoreLessRules(what, ss0) {
    var rxMoreLessSelectors = new RegExp("^ul#" + what + "\\s+li\\.(isOld|moreDemos|fewerDemos)$", "i");
    var rules = ss0.cssRules ? ss0.cssRules : ss0.rules;

    for (var i = rules.length - 1; 0 <= i; --i) {
        var selector = rules[i].selectorText;
        if (selector != null && selector.match(rxMoreLessSelectors)) {
            if (typeof(ss0.deleteRule) != 'undefined')
                ss0.deleteRule(i);
            else
                ss0.removeRule(i);
        }
    }
}

function ShowMore(what) {
    var ss0 = document.styleSheets[0];
    RemoveMoreLessRules(what, ss0);
    appendRule(ss0, "ul#" + what + " li.moreDemos", "display: none");
    appendRule(ss0, "ul#" + what + " li.isOld", "display: block");
    appendRule(ss0, "ul#" + what + " li.fewerDemos", "display: block");
}

function ShowFewer(what) {
    var ss0 = document.styleSheets[0];
    RemoveMoreLessRules(what, ss0);
    appendRule(ss0, "ul#" + what + " li.fewerDemos", "display: none");
    appendRule(ss0, "ul#" + what + " li.isOld", "display: none");
    appendRule(ss0, "ul#" + what + " li.moreDemos", "display: block");
}
