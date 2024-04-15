//loginBtn.addEventListener('click', function () {
//    Login();
//    saveUser("employee:be93045f0f01e63a");
//});
//function Login() {
//    alert("Hallo")
//    let p = document.getElementById('EmaiL');
//    var value = p.value;
//    let g = document.getElementById('Psw');
//    var valueg = g.value;
//    //var valueg = "Hr2022$!S";

//    var data = {
//        Email: value,
//        psw: valueg
//    };

//    $.ajax({
//        type: 'POST',
//        url: '/Home/LoginFunc',
//        data: data,

//        success: function (result) {
//            alert(result);
//            saveUser(result);
//        },
//        error: function (result, success, error) {
//            alert("Error : " + error);
//        }
//    });

//    return false;
//}
//function onSuccess(data) {
//    alert(data);
//    saveUser(data);
//}

function saveUser(emp) {
    if (typeof (Storage) !== "undefined") {
        sessionStorage.EmployeeId = emp;
    } else {

    }
}