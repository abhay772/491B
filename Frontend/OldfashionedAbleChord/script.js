const content = document.getElementById('content');
const api = "https://localhost:7135/api/"

function loadLoginPage() {
  // fetch login page
  fetch('./Views/login.html')
    .then(response => response.text())
    .then(data => {
      // update content div with login page
      content.innerHTML = data;

      // select register link
      const registerLink = document.getElementById('register-link');
      // select forgot password button
      const forgotPasswordButton = document.querySelector('.forget');

      // add event listeners to register link, forgot password button
      registerLink.addEventListener('click', loadRegisterPage);
      //forgotPasswordButton.addEventListener('click', loadForgotPasswordPage);

      // select login form
      const loginForm = document.getElementById('login-form');

      // add event listener to login form submit
      loginForm.addEventListener('submit', (event) => {
        event.preventDefault();

        // perform login
        const username = document.querySelector('#logemail').value;
        const password = document.querySelector('#logpassword').value;

        url = api + "Authentication/Login";
        data = {username: username, password: password}

        send(url, data)
            .then(response => console.log(response.text()))
            .catch(error => console.log(error));
      });
    })
    .catch(error => console.log(error));
}


function loadRegisterPage() {
  // fetch register page html
  fetch("./Views/register.html")
    .then(response => response.text())
    .then(data => {
      // update content div with register page
      content.innerHTML = data;

      const cancelBtn = document.getElementById('cancel');

      // add event listeners
      cancelBtn.addEventListener('click', loadLoginPage)

      // select register form
      const registerForm = document.getElementById('register-form');

      // add event listener to register form submit
      registerForm.addEventListener('submit', (event) => {
        event.preventDefault();

        // perform registration action
        console.log('Performing registration action...');
        // You can perform your registration API call here

        // after successful registration, load propertyEval.html
        loadLoginPage();
      });
    })
    .catch(error => console.log(error))
}


// function to load property evaluation page
function loadPropertyEvalPage() {
  // fetch property evaluation page html
  fetch('./Views/propEval.html')
    .then(response => response.text())
    .then(data => {
      // update content div with property evaluation page html
      content.innerHTML = data;
    })
    .catch(error => console.log(error));
}


// From https://github.com/v-vong3/csulb/tree/master/cecs_491
function get(url) {

  const options = {
    method: 'GET',
    mode: 'no-cors',
    cache: 'default',
    credentials: 'include',
    headers: {
      'Content-Type': 'application/json'
    },
    redirect: 'follow',
    referrerPolicy: 'no-referrer-when-downgrade',
  };

  return fetch(url, options);
}

// Exposing send() to the global object ("Public" functions)
function send(url, data) {
  const options = {
    method: 'POST',
    mode: 'no-cors',
    cache: 'default',
    credentials: 'include',
    headers: {
        'Content-Type': 'application/json',
        'Origin':'www.example.com'
    },
    redirect: 'follow',
    referrerPolicy: 'no-referrer-when-downgrade',
    body: JSON.stringify(data),
  };

  return fetch(url, options);
}

// load login page initially
loadLoginPage();