var canvas1;
var ctx;
var WIDTH = 400, HEIGHT = 500;
var g_cExecutions = 1;
var controls_lists = ["controls_bullet0", "controls_bullet1", "controls_bullet2", "controls_bullet3", "controls_bullet4"];
var timer1, timer2;
var video;
var currentX=0, currentY=0;
var idTextArea = document.getElementById('idTextArea');
var description = document.getElementById('description');
var idRectTest = document.getElementById('idRectTest');

var queryList     = ["rectangles", "arcs", "quadratic", "bezier", "clipping", "fillStyle", "strokeStyle", "gradients", "patterns", "lineWidth", "lineCap", "lineJoin", 
                     "shadows", "text", "image", "video", "rotate", "scale", "setTransform", "stroke", "animation", "mouse"];
var commandList   = ["runTest('idRectDescription', 'idRectTest');", "runTest('idArcDescription', 'idArcTest');", "runTest('idQuadraticDescription', 'idQuadraticTest');", "runTest('idBezierDescription', 'idBezierTest');", 
                    "runTest('idClipDescription', 'idClippingTest');", "runTest('idFillStyleDescription', 'idFillStyleTest');", "runTest('idStrokeStyleDescription', 'idStrokeStyleTest');", "runTest('idGradientsDescription', 'idGradientTest');", 
                    "runTest('idPatternsDescription', 'idWallpaperTest');", "runTest('idLineWidthDescription', 'idLineWidthTest');", "runTest('idLineCapDescription', 'idLineCapTest');", "runTest('idLineJoinDescription', 'idLineJoinTest');", 
                    "showList(-1); runTest('idShadowsDescription', 'idShadowTest');", "showList(-1); runTest('idTextDescription', 'idFontTest');", "runTest('idImagesDescription', 'idImageTest');", "runTest('idImagesDescription', 'idVideoTest');",
                    "runTest('idTransformationDescription', 'idRotateTest');", "runTest('idTransformationDescription', 'idScaleTest');", "runTest('idTransformationDescription', 'idSetTransformTest');" ,
                    "runTest('idTransformationDescription', 'idStrokeTransformTest');", "showList(-1); runTest('idAnimationDescription', 'idBezierScribbleTest');", "showList(-1); runTest('idMouseDescription', 'idMouseTest');"];
                     

function setInnerText(e, s) {
    if (e.innerText != undefined) {
        e.innerText = s;
    }
    else {
        e.textContent = s;
    }
}

function getInnerText(e) {
    var t = e.innerText;
    if (t == undefined) {
        t = e.textContent;
    }
    return t;
}

function getInnerHTML(e) {
    var t = e.innerHTML;
    if (t == undefined) {
        t = e.textContent;
    }
    return t;
}

function runTest(testDescription, testCase) {
    var d = document.getElementById(testDescription);
    var t = document.getElementById(testCase);

    description.innerHTML = getInnerHTML(d);
    idTextArea.value = getInnerText(t); 
    doExec();
}

function checkAndExec(e) {
    if (e.keyCode == 13 && e.ctrlKey == 1) doExec();
}

function doExec() {
    //reset Canvas
    canvas1.width = WIDTH;
    canvas1.height = HEIGHT;

    //clear rect
    ctx.save();
    ctx.fillStyle = 'rgb(255,255,255)';
    ctx.fillRect(0, 0, WIDTH, HEIGHT);
    ctx.restore();
    
    //clear paths
    ctx.beginPath();

    //stop the video
    if (!isNaN(video.duration)) video.pause();
        
    //clear timers
    if (timer1 != null) clearInterval(timer1);
    if (timer2 != null) clearInterval(timer2);
    
    ctx.save();
    for (iExecCounter = 0; iExecCounter < g_cExecutions; iExecCounter++) {
        window.eval(idTextArea.value);
    }
    ctx.restore();
}


function initGlobals() {
    canvas1 = document.getElementById('canvas1');
    ctx = canvas1.getContext('2d');

    canvas1.width = WIDTH;
    canvas1.height = HEIGHT;

    video = document.getElementById("vid");

    checkParameters();

    if (window.addEventListener) window.addEventListener("resize", OnWindowResize, false);
    else if (window.attachEvent) window.attachEvent("onresize", OnWindowResize);
    
    if (canvas1.addEventListener) canvas1.addEventListener("mousemove", OnMouseMove, false);
    else if (canvas1.attachEvent) canvas1.attachEvent("onmousemove", OnMouseMove);
    
}

function checkParameters() {
    //look for something like ?c=arcs
    var urlquery = parent.document.URL;
    var command = urlquery.substring(urlquery.indexOf('?')+3, urlquery.length);

    var testExists = false;

    if (urlquery.indexOf('?') != -1 && urlquery.indexOf('c=') != -1) {
        //alert(command);
        for (i = 0; i < queryList.length; i++) {
            if (command == queryList[i]) {
                window.eval(commandList[i]);
                testExists = true;
                break;
            }
        }
        if (!testExists) runTest('idRectDescription', 'idRectTest');
    }
    else runTest('idRectDescription', 'idRectTest');
    
    
}

function OnMouseMove(e) {

    if (typeof e == 'undefined') e = canvas1.event;
    if (typeof e.offsetX != 'undefined' && typeof e.offsetY != 'undefined') {
        currentX = e.offsetX;
        currentY = e.offsetY;
    }
    else {
        var relPos = getRelativePos(e.clientX, e.clientY);
        currentX = relPos[0];
        currentY = relPos[1];
    }
    
    document.getElementById('mousePosition').innerHTML = "Mouse position: (" + currentX + ", " + currentY + ")";
}

// My thanks to QuirksMode.org for the insight here
function getRelativePos(x, y) {
    var curleft = curtop = 0;

    var obj = document.getElementById('canvas1');
    if (obj.offsetParent) {
        do {
            curleft += obj.offsetLeft;
            curtop += obj.offsetTop;
        } while (obj = obj.offsetParent);
    }

    // Webkit isn't compliant with CSS OM View here; thus, we need to grab scrollTop from body instead of documentElement

    if (document.body.scrollLeft > 0) {
        var scrollLeft = document.body.scrollLeft;
    }
    else {
        scrollLeft = document.documentElement.scrollLeft;
    }

    if (document.body.scrollTop > 0) {
        var scrollTop = document.body.scrollTop;
    }
    else {
        scrollTop = document.documentElement.scrollTop;
    }

    return [(x - curleft + scrollLeft), (y - curtop + scrollTop)];
}

function resetCanvas() {
    //reset Canvas
    canvas1.width = WIDTH;
    canvas1.height = HEIGHT;

    //clear rect
    ctx.save();
    ctx.fillStyle = 'rgb(255,255,255)';
    ctx.fillRect(0, 0, WIDTH, HEIGHT);
    ctx.restore();
    
    //clear paths
    ctx.beginPath();

    //stop the video
    if (!isNaN(video.duration)) video.pause();

    if (timer1 != null) clearInterval(timer1);
    if (timer2 != null) clearInterval(timer2);
}

function OnWindowResize() {
    canvas1.width = WIDTH;
    canvas1.height = HEIGHT;

    doExec();
}

//x is the exception
function hideAllLists(x) {
    var nodes;
    
    for (var j = 0; j < controls_lists.length; j++) {
        if (x == j) continue;
        nodes = document.getElementsByClassName(controls_lists[j]);
        for (var i = 0; i < nodes.length; ++i) {
            nodes[i].style.display = "none";
        }
    }
}

function showList(x) {
    hideAllLists(x);

    var nodes = document.getElementsByClassName(controls_lists[x]);

    for (var i = 0; i < nodes.length; i++) {
       if (nodes[i].style.display == "list-item") nodes[i].style.display = "none";
       else nodes[i].style.display = "list-item";
    }
}

