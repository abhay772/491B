

const createUser = document.querySelector('#submit');
const firstName = document.querySelector('#firstName');
const lastName = document.querySelector('#lastName');
const email = document.querySelector('#email');
const password = document.querySelector('#password');

var role ="";
function UserRole(){
    
    if(document.getElementById("SP").checked) {
    let role = "Service Provider";
    }
    if(document.getElementById("PM").checked) {
    let role = "Property Manager";
    }
    return role;
}

function getUserByEmail(email){
    fetch('https://localhost:7281/api/Users/${email}')
    .then(data => data.json())
    .then(response => console.log(response))
}

function addUser(firstName, lastName, email, password, role){
    const body = {
        Email: email,
        Password: password,
        FirstName: firstName,
        LastName: lastName,
        Role: role,
        
    };  
    
    fetch('https://localhost:7135/api/UserManagement/register', {
        method: 'POST',
        body: JSON.stringify(body),
        headers: {
            "content-type": "application/json"
        }
    })    
        .then(data => data.json())
        .then(response => console.log(response));
    
}

function validate() {
    let email = document.forms["register"]["email"].value;
    if (email == "") {//valid email statment  ``
      alert("Must enter a valid email");
      return false;
    }
    let repass = document.forms["register"]["repeatpass"].value;
    let pass = document.forms["register"]["password"].value;
    if (pass !== repass) {
      alert("Passwords must be the same");
      return false;
    }
    let year = document.forms["register"]["age"].value;
    var today = new Date();
    var thisYear = today.getFullYear();
    if(thisYear - year < 13){
        alert("You must be 13 years old or older");
        return false;
    }
}

createUser.addEventListener('click', function(){
    role =  UserRole();
    addUser(firstName.value, lastName.value, email.value, password.value, role);
})