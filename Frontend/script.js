

const createUser = document.getElementById("register");
const loginUser = document.getElementById("login");
const firstName = document.querySelector('#firstName');
const lastName = document.querySelector('#lastName');
const email = document.querySelector('#email');
const logemail = document.querySelector('#logemail');
const password = document.querySelector('#password');
const logpassword = document.querySelector('#logpassword');

var role = "";
function UserRole() {

  if (document.getElementById("SP").checked) {
    let role = "Service Provider";
  }
  if (document.getElementById("PM").checked) {
    let role = "Property Manager";
  }
  return role;
}

function getUserByEmail(email) {
  fetch('https://localhost:7135/api/Users/${email}')
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
        .then(response => alert(response));
    
}

function logInUser(email, password) {
  const body = {
    email: email,
    password: password,

  };

  fetch('https://localhost:7135/api/Users', {
    method: 'POST',
    body: JSON.stringify(body),
    headers: {
      "content-type": "application/json"
    }
  })
    .then(data => data.json())
    .then(response => console.log(response));
    if (response == true) {
      window.location.assign("homepage_view");
    } else {
      alert("Please enter valid information");
      return;
    }

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
  if (thisYear - year < 13) {
    alert("You must be 13 years old or older");
    return false;
  }
}

createUser.addEventListener('click', function() {
  role = UserRole();
  addUser(firstName.value, lastName.value, email.value, password.value, role);
})

loginUser.addEventListener('click', function() {
  logInUser(email.value, password.value);
})

//side bar activation
var hamburger = document.querySelector(".hamburger");
hamburger.addEventListener("click", function() {
  document.querySelector("body").classList.toggle("active");
})
