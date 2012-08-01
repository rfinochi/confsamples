// For an introduction to the Blank template, see the following documentation:
// http://go.microsoft.com/fwlink/?LinkId=232509
(function () {
    "use strict";

    var app = WinJS.Application;
    var activation = Windows.ApplicationModel.Activation;
    WinJS.strictProcessing();

    app.onactivated = function (args) {
        if (args.detail.kind === activation.ActivationKind.launch) {
            if (args.detail.previousExecutionState !== activation.ApplicationExecutionState.terminated) {
                // TODO: This application has been newly launched. Initialize
                // your application here.
            } else {
                // TODO: This application has been reactivated from suspension.
                // Restore application state here.
            }

            //001
            addTitle();

            //002 - XmlHttpRequest
            //loadJson("json/people.json", { id: 1 }, function (response) {
            //    var json = JSON.parse(response.responseText);
            //    setJson(json);
            //});

            ////003 - Promesa
            //WinJS.xhr({ url: "json/people.json" })
            //     .then(function (response) {
            //         var json = JSON.parse(response.responseText);
            //         setJson(json);

            //     });

            ////004 - Promesa
            //WinJS.xhr({ url: "json2/people.json" })
            //     .then(function (response) { xhrComplete(response) },
            //           function (ex) { xhrError(ex) },
            //           function () { xhrProgress() });

            args.setPromise(WinJS.UI.processAll());
        }
    };

    app.oncheckpoint = function (args) {
        // TODO: This application is about to be suspended. Save any state
        // that needs to persist across suspensions here. You might use the
        // WinJS.Application.sessionState object, which is automatically
        // saved and restored across suspension. If you need to complete an
        // asynchronous operation before your application is suspended, call
        // args.setPromise().
    };

    app.start();
})();


//001
function addTitle() {
    var title = document.createElement("a");
    title.innerHTML = "&nbsp;&nbsp;&nbsp;&nbsp";
    title.innerHTML += "Windows 8 - Metro Style App";
    title.setAttribute("style", "font-size:80px");
    document.getElementById("divTitle").appendChild(title);
}

function addFooter() {
    var footer = document.createElement("label");
    footer.innerHTML = "<br /> <br /> <br /> <br /> <br /> <br />";
    footer.innerHTML += "Lagash Systems";
    footer.setAttribute("style", "font-size:30px; color: white");
    document.body.appendChild(footer);
}


//002
function loadJson(url, data, callback) {
    var xhr = new XMLHttpRequest();
    xhr.open("GET", url, true);
    xhr.onload = function () {
        callback(xhr);
    }
    xhr.send(JSON.stringify(data));
}






//002 y 003
function setJson(response) {
    var firstName = response.people.first;
    var lastName = response.people.last;
    var sex = response.people.sex;
    var profesion = response.people.profesion;
    var avatar = response.people.avatar;

    var img = document.createElement("img");
    img.src = avatar;
    img.setAttribute("style", "float:left; padding: 0 50px 0 300px;");
    document.body.appendChild(img);

    var p = document.createElement("p");
    p.innerHTML = "Nombre: " + firstName + "<br />";
    p.innerHTML += "Apellido: " + lastName + "<br />";
    p.innerHTML += "Sexo: " + sex;
    p.setAttribute("style", "font-size:40px");
    document.body.appendChild(p);

    var p2 = document.createElement("p");
    p2.innerHTML = "Profesión: " + profesion;
    p2.setAttribute("style", "top:0px; font-size:40px; color:red;");
    document.body.appendChild(p2);
}

//004
function xhrComplete(response) {
    var json = JSON.parse(response.responseText);
    setJson(json);
}

function xhrError(ex) {
    var msg = new Windows.UI.Popups.MessageDialog("Ejemplo de error: se escribió mal la dirección del archivo Json. \n" + ex.message);
    msg.showAsync();
}

function xhrProgress() {
    //loading image
}