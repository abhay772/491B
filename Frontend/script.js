const url  = "https://localhost:7135/api";

const firstName = document.getElementById("firstName").value;
const lastName = document.getElementById("lastName").value;
const email = document.getElementById("email").value;
const password = document.getElementById("pass").value;
const logEmail = document.getElementById("logemail").value;
const logPass = document.getElementById("logpass").value;

function validate() {
  let repass = document.forms["register"]["repeatpass"].value;
  let pass = document.forms["register"]["pass"].value;
  if (pass !== repass) {
    alert("Passwords do not match");
    return false;
  }
  let year = document.forms["register"]["age"].value;
  var today = new Date();
  var thisYear = today.getFullYear();
  if (thisYear - year < 13) {
    alert("You must be 13 years old or older");
    return false;
  }
  return true;
}

var role = "";
function UserRole() {

  if (document.getElementById("SP").checked) {
    role = "Service Provider";
  }
  if (document.getElementById("PM").checked) {
    role = "Property Manager";
  }
  return role;
}
role = UserRole();

const register = {firstName: firstName, lastName: lastName, email:email, password:password, role:role};
const logIn ={Email:logEmail, PassDigest:logPass};

const regform = document.getElementById('signupForm');

regform.addEventListener('submit', event => {
  event.preventDefault();

  const api = url + "/UserManagement/register";

  if(validate()){

    const options = {
      method:'POST',
      body: JSON.stringify(register),
      
      mode: 'no-cors',
      credentials: 'include',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/problems+json'
      },
      redirect: 'follow',
      referrerPolicy: 'no-referrer-when-downgrade'
    };

    fetch(api, options)
      .then(response => response.text())
      .then(text => console.log(text))
      .catch(error => console.error(error));
  }
  else{
    alert("try again");
  }
});

const logform = document.getElementById('loginForm');

logform.addEventListener('submit', event => {
  event.preventDefault();
  const api = url + "/Authentication";

  const options = {
    method:'POST',
    body: JSON.stringify(logIn),
    
    mode: 'cors',
    credentials: 'include',
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json'
    },
    //redirect: 'follow',
    //referrerPolicy: 'no-referrer-when-downgrade'
  };

  fetch(api, options)
    .then(response => response.text())
    .then(text => console.log(text))
    .catch(error => console.error(error));

});


/*side bar activation
var hamburger = document.querySelector(".hamburger");
hamburger.addEventListener("click", function() {
  document.querySelector("body").classList.toggle("active");
})*/