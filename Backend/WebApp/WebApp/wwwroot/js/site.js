// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
//connect to data base
//use xhrto send browser data



const cancel = document.querySelector("#cancel");
const register = document.querySelector("#register");
const login = document.querySelector("#login");
const forgotPass = document.querySelector("#forget")

let userEmail = document.forms["login"]["email"].value;

cancel.onclick = cancelUser;
register.onclick = registerUser;
login.onclick = loginUser;
forgotPass.onclick = accountRecovery;

function validate() {
    let email = document.forms["register"]["email"].value;
    if (email == "") {//valid email statment  ``
        alert("Must enter a valid email");
        return false;
    }
    let repass = document.forms["register"]["repeatpass"].value;
    let pass = document.forms["register"]["pass"].value;
    if (pass !== repass) {
        alert("Passwords must be the same");
        return false;
    }

}
function loginUser() {
    console.log("hello");
    //authenticate user and login
}

function registerUser() {
    console.log("hello");
    //add user top database
}

function cancelUser() {
    console.log("hello");
    //delete user from database
}

function accountRecovery(userEmail) {
    //send user a email to recover password
}

//fetch
var url = 'https://jsonplaceholder.typicode.com/todos/1';

// Making our request 
fetch(url, { method: 'GET' })
    .then(Result => Result.json())
    .then(string => {

        // Printing our response 
        console.log(string);

        // Printing our field of our response
        console.log(`Title of our response :  ${string.title}`);
    })
    .catch(errorMsg => { console.log(errorMsg); });

//xhr
function run() {

    // Creating Our XMLHttpRequest object 
    var xhr = new XMLHttpRequest();

    // Making our connection  
    var url = 'https://jsonplaceholder.typicode.com/todos/1';
    xhr.open("GET", url, true);

    // function execute after request is successful 
    xhr.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            console.log(this.responseText);
        }
    }
    // Sending our request 
    xhr.send();
}
run();