function saveId(edit) {
    if (typeof (Storage) !== "undefined") {
        sessionStorage.EditId = edit;
    } else {
    }
}

function showSelect(jsonItems) {
    var myRequest = {
        EditID: sessionStorage.EditId + '',
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
    $.ajax({
        type: 'POST',
        url: '/Home/SendEdit',
        data: myRequest,

        success: function (response) {
            if (response !== "") {
                var res = response.split('/')
                var p = document.getElementById('projects');
                var sv = document.getElementById('services');
                var ht = document.getElementById('hourtypes');
                valueH = res[0];
                for (var x in jsonItems) {
                    for (var y in jsonItems[x].ServiceItems) {
                        var z = jsonItems[x].ServiceItems[y].HourTypeItems;
                        for (var i = 0; i < z.length; i++) {
                            if (z[i].Id == valueH) {
                                p.value = x;
                                for (var y in jsonItems[x].ServiceItems) {
                                    serviceSel.options[serviceSel.options.length] = new Option(jsonItems[x].ServiceItems[y].Name, y);
                                }
                                sv.value = y;
                                for (var j = 0; j < z.length; j++) {
                                    hourtypeSel.options[hourtypeSel.options.length] = new Option(z[j].Name, z[j].Id);
                                }
                                ht.value = z[i].Id;
                            }
                        }
                    }
                }
                var s = document.getElementById('sTime');
                var e = document.getElementById('eTime');
                s.value = res[1];
                e.value = res[2];
                document.getElementById('storedS').innerHTML = res[1];
                document.getElementById('storedE').innerHTML = res[2];
                var h = document.getElementById('hiddenId');
                h.value = sessionStorage.EditId;
            }
        }

    })
 }