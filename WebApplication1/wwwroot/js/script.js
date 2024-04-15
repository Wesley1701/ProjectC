let startBtn = document.getElementById('start');
let stopBtn = document.getElementById('stop');
let resetBtn = document.getElementById('reset');

let hour = 00;
let minute = 00;
let second = 00;
let timer = false;
let start = new Date();
let end = new Date();

function onLoadFunctions(jsonItems, message) {
    if (typeof (Storage) !== "undefined") {
        if (message != '' && message != null) {
            sessionStorage.EmployeeId = message;
        }
    }
    var myRequest = {
        UserID: sessionStorage.EmployeeId,
    }

    console.log(jsonItems);
    var projectSel = document.getElementById('projects');
    var serviceSel = document.getElementById('services');
    var hourtypeSel = document.getElementById('hourtypes');

    for (var x in jsonItems) {
        projectSel.options[projectSel.options.length] = new Option(jsonItems[x].Name, x);
    }
    projectSel.onchange = function () {
        serviceSel.length = 1;
        hourtypeSel.length = 1;
        var val = projectSel.value;
        for (var y in jsonItems[val].ServiceItems) {
            serviceSel.options[serviceSel.options.length] = new Option(jsonItems[val].ServiceItems[y].Name, y);
        }
    }
    serviceSel.onchange = function () {
        hourtypeSel.length = 1;
        var val = projectSel.value;
        var val1 = serviceSel.value;
        var z = jsonItems[val].ServiceItems[val1].HourTypeItems;
        for (var i = 0; i < z.length; i++) {
            hourtypeSel.options[hourtypeSel.options.length] = new Option(z[i].Name, z[i].Id);
        }
    }

    var interval = 60 * 60 * 1000; // 1 hour in milliseconds


    function showBreakReminder() {
        alert("You have worked an hour. Take a break.");
    }


    setInterval(showBreakReminder, interval);

    window.onbeforeunload = function () {
        return "Are you sure you want to leave this page?";
    }

    if (sessionStorage.HourTypeID != null) {
        setProject(jsonItems, sessionStorage.HourTypeID);
    }

    $.ajax({
        type: 'POST',
        url: '/Home/GetStart',
        data: myRequest, 

        success: function (response) {
            if (response !== "") {
                var res = response.split('/')
                start = new Date(res[0]);
                current = new Date();
                var timeDiff = current - start;
                timeDiff /= 1000;

                var seconds = Math.round(timeDiff)
                hour = Math.floor(seconds / 3600)
                minute = Math.floor(seconds / 60) % 60
                second = seconds % 60

                let hrString = hour;
                let minString = minute;
                let secString = second;

                if (hour < 10) {
                    hrString = "0" + hrString;
                }

                if (minute < 10) {
                    minString = "0" + minString;
                }

                if (second < 10) {
                    secString = "0" + secString;
                }


                document.getElementById('hr').innerHTML = hrString;
                document.getElementById('min').innerHTML = minString;
                document.getElementById('sec').innerHTML = secString;

                timer = true;
                stopWatch();
                var text = res[1];
                sessionStorage.HourTypeID = text;
                setProject(jsonItems, text);
                document.getElementById('startTime').innerHTML = start.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
            }
        },
    });
}

startBtn.addEventListener('click', function () {
    if (!timer) {
        if (document.getElementById('hourtypes').value != "") {
            timer = true;
            stopWatch();
            start = new Date();
            sendStart(start);
            document.getElementById('startTime').innerHTML = start.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
        } else {
            alert("Please select a Project, Service and Hour Type before starting the timer");
        }
    }
}
);

stopBtn.addEventListener('click', function () {
    if (timer) {
        timer = false;
        end = new Date();
        sendEnd(end);
        window.sessionStorage.removeItem('HourTypeID');
        var p = document.getElementById('projects');
        var s = document.getElementById('services');
        var ht = document.getElementById('hourtypes');
        p.removeAttribute("disabled");
        s.removeAttribute("disabled");
        ht.removeAttribute("disabled");
    }  
});

resetBtn.addEventListener('click', function () {
    timer = false;
    hour = 0;
    minute = 0;
    second = 0;
    document.getElementById('hr').innerHTML = "00";
    document.getElementById('min').innerHTML = "00";
    document.getElementById('sec').innerHTML = "00";
    document.getElementById('startTime').innerHTML = "not set";
    document.getElementById('projects').value = "";
    document.getElementById('services').value = "";
    document.getElementById('hourtypes').value = "";
});

function stopWatch() {
    if (timer) {
        second++;

        if (second == 60) {
            minute++;
            second = 0;
        }

        if (minute == 60) {
            hour++;
            minute = 0;
            second = 0;
        }

        let hrString = hour;
        let minString = minute;
        let secString = second;

        if (hour < 10) {
            hrString = "0" + hrString;
        }

        if (minute < 10) {
            minString = "0" + minString;
        }

        if (second < 10) {
            secString = "0" + secString;
        }


        document.getElementById('hr').innerHTML = hrString;
        document.getElementById('min').innerHTML = minString;
        document.getElementById('sec').innerHTML = secString;
        setTimeout(stopWatch, 1000);
    }
}
function sendStart(s) {
    var projectSel = document.getElementById('projects');
    var serviceSel = document.getElementById('services');
    var hourtypeSel = document.getElementById('hourtypes');
    sessionStorage.HourTypeID = hourtypeSel.value;

    var data = {
        startTime: s.toLocaleTimeString(),
        hourtypeID: sessionStorage.HourTypeID,
        userID: sessionStorage.EmployeeId
    };
    projectSel.setAttribute("disabled", "disabled");
    serviceSel.setAttribute("disabled", "disabled");
    hourtypeSel.setAttribute("disabled", "disabled");

    $.ajax(
        {
            type: 'POST',
            url: '/Home/SetStart',
            data: data,
            succes: function (result) {
                console.log("Succes");
            }
        }
    )
}
function sendEnd(e) {
    var p = document.getElementById("projects");
    var value = p.value;
    var data = {
        endTime: e.toLocaleTimeString(),
        projectID: value,
        userID: sessionStorage.EmployeeId
    };

    $.ajax(
        {
            type: 'POST',
            url: '/Home/SetEnd',
            data: data,
            succes: function (result) {
                console.log("Succes");
            }
        }
    )
}
function setProject(jsonItems, hID) {
    var p = document.getElementById('projects');
    var sv = document.getElementById('services');
    var ht = document.getElementById('hourtypes');
    valueH = hID;
    for (var x in jsonItems) {
        for (var y in jsonItems[x].ServiceItems) {
            var z = jsonItems[x].ServiceItems[y].HourTypeItems;
            for (var i = 0; i < z.length; i++) {
                if (z[i].Id == valueH) {
                    p.value = x;
                    for (var y in jsonItems[x].ServiceItems) {
                        sv.options[sv.options.length] = new Option(jsonItems[x].ServiceItems[y].Name, y);
                    }
                    sv.value = y;
                    for (var j = 0; j < z.length; j++) {
                        ht.options[ht.options.length] = new Option(z[j].Name, z[j].Id);
                    }
                    ht.value = z[i].Id;
                }
            }
        }
    }
    p.setAttribute("disabled", "disabled");
    sv.setAttribute("disabled", "disabled");
    ht.setAttribute("disabled", "disabled");
}

    //var jsonData = JSON.stringify(data);
    //try {
    //    $.ajax({
    //        url: 'Home/SetData',
    //        data: { jsonData },
    //        dataType: "json",
    //        type: 'POST',
    //        succes: getSucces,
    //        error: getFail
    //    }).done(function () {
    //        alert("Posted!");
    //    });
    //} catch (e) {
    //    alert(e);
    //}
    //function getSuccess(data, textStatus, jqXHR) {
    //    alert(data.Response);
    //};
    //function getFail(jqXHR, textStatus, errorThrown) {
    //    alert(jqXHR.status);
    //};



    //Instantiating easyHTTP
    //let http = new easyHTTP;

    // //Put prototype method(url, data,
    // //response text)
   
    //http.post(
    //    'Home/SetData',
    //    data, function (err, post) {
    //        if (err) {
    //            console.log(err);
    //        } else {
    //            console.log(post);
    //        }
    //});

    //let json = JSON.stringify(data)

    //let xhr = new XMLHttpRequest();
    //xhr.open("POST", "Home/SetData");
    //xhr.setRequestHeader('Content-type', 'application/json; charset=utf-8');

    //xhr.send(json);

    //xhr.onload = () => alert(xhr.response);


function easyHTTP() {

    // Initializing new XMLHttpRequest method.
    this.http = new XMLHttpRequest();
}

easyHTTP.prototype.post = function (url, data, callback) {
    this.onreadystatechange = function () {
        console.log(this.readyState) // should be 4
        console.log(this.status) // should be 200 OK
        console.log(this.responseText) // response return from request
    };

    // Open an object (POST, PATH, ASYNC-TRUE/FALSE)
    this.http.open('POST', url, true);

    // Set content-type
    this.http.setRequestHeader(
        'Content-type', 'application/json');

    // Assigning this to self to have 
    // scope of this into the function onload
    let self = this;

    // When response is ready
    this.http.onload = function () {

        // Callback function (Error, response text)
        callback(null, self.http.responseText);
    }

    // Since the data is an object so
    // we need to stringify it
    let dataSend = JSON.stringify(data);
    if (dataSend != null) {
        this.http.send(dataSend);
    }
}

