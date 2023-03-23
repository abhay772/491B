const content = document.getElementById('content');
const api = "https://localhost:7135/swagger/api"

function loadLoginPage() {
  // fetch login page
  fetch('./Views/deleteAcc.html')
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

        url = api + "/Auth/Login";
        data = { username: username, password: password }

        get(url)
          .then(response => console.log(response.text()));
      });
    })
    .catch(error => console.log(error));
}

function UserRole() {
  if (document.getElementById("SP").checked) {
    role = "Service Provider";
  }
  if (document.getElementById("PM").checked) {
    role = "Property Manager";
  }
  return role;
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
        const email = document.querySelector('#email').value;
        const password = document.querySelector('#password').value;
        const firstName = document.querySelector('#firstName');
        const lastName = document.querySelector('#lastName'); 
        const role = UserRole();
  
        url = 'https://localhost:7135/api/UserManagement/register';
        data = {Email: email, Password: password, FirstName: firstName, LastName: lastName, Role: role }
    
            send(url)
              .then(response => console.log(response.text()));
        // You can perform your registration API call here

        // after successful registration, load propertyEval.html homePage.html
        loadLoginPage();
      });
    })
    .catch(error => console.log(error))
}

function loadAccountDeletionPage(){
  //fetch account deletion page
  fetch('./Views/deleteAcc.html')
    .then(response => response.text())
    .then(data => {
      // update content div with property evaluation page html
      content.innerHTML = data;
    })
    .catch(error => console.log(error));
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

//fucntion to load request Management page
function loadRequestManagementPage() {
  // fetch request evaluation page html
  fetch('./Views/requestMan.html')
    .then(response => response.text())
    .then(data => {
      // update content div with property evaluation page html
      content.innerHTML = data;
    })
    .catch(error => console.log(error));
}


//function to load homepage
function loadHomePage() {
  // fetch property evaluation page html
  fetch('./Views/homepage.html')
    .then(response => response.text())
    .then(data => {
      // update content div with property evaluation page html
      content.innerHTML = data;
    })
    .catch(error => console.log(error));

    //select log out
    const logoutUser = document.getElementById('settings');
    logoutUser.addEventListener('click', loadLoginPage);
   //select propertyEvaluation
    const propertyEvalFeature = document.getElementById('serviceManagement');
     //select request management
    const requestFeature = document.getElementById('requestManagement');
      //add event listener to nav to request management
    requestFeature.addEventListener('click', loadRequestManagementPage);  

      // add event listeners to nav to property evaluation
    propertyEvalFeature.addEventListener('click', loadPropertyEvalPage);
  
}


// From https://github.com/v-vong3/csulb/tree/master/cecs_491
function get(url) {

  const options = {
    method: 'GET',
    mode: 'no-cors',
    cache: 'default',
    credentials: 'same-origin',
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
    credentials: 'same-origin',
    headers: {
      'Content-Type': 'application/json'
    },
    redirect: 'follow',
    referrerPolicy: 'no-referrer-when-downgrade',
    body: JSON.stringify(data),
  };

  return fetch(url, options);
}
// load login page initially
loadLoginPage();