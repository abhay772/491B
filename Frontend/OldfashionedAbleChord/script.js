const content = document.getElementById('content');
const api = "https://localhost:7135/api"

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
      //const forgotPasswordButton = document.querySelector('.forget');

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

        url = api + "/Authentication/Login";
        data = { username: username, password: password }

        send(url, data)
          .then(data => data.json())
          .then(response => console.log(response))
          .then(loadHomePage());
          //.then((response) =>{if(response.ok){loadHomePage()}})

      });
      
    })
    .catch(error => console.log(error));
}

var role="";
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
        const email = document.querySelector('#email').value;
        const password = document.querySelector('#pass').value;
        const firstName = document.querySelector('#firstName').value;
        const lastName = document.querySelector('#lastName').value; 
        const role = UserRole();
  
        url = api + '/UserManagement/register';
        data = {email: email, firstName: firstName, lastName: lastName, password: password, role: role }
    
          send(url, data)
            .then(data => data.json())
            .then(response => console.log(response))
        // You can perform your registration API call here

        // after successful registration, load propertyEval.html homePage.html
        loadLoginPage();
      });
    })
    .catch(error => console.log(error))
}

//function to load homepage
function loadHomePage() {
  // fetch property evaluation page html
  fetch('./Views/homepage.html')
    .then(response => response.text())
    .then(data => {
      // update content div with property evaluation page html
      content.innerHTML = data;

      const hamburger = document.getElementById("back");
      hamburger.addEventListener("click", loadHomePage);
    //select log out
      const logoutUser = document.getElementById("logout");
      const setting = document.getElementById("settings");
      
    //select propertyEvaluation
      const propertyEvalFeature = document.getElementById('propertyEvaluation');
      //select request management
      const requestFeature = document.getElementById('requestManagement');
      //add event listeners
      logoutUser.addEventListener('click', loadLoginPage);
        //add event listener to nav to request management
      requestFeature.addEventListener('click', loadRequestManagementPage);  

        // add event listeners to nav to property evaluation
      propertyEvalFeature.addEventListener('click', loadPropertyEvalPage);
      setting.addEventListener('click', loadAccountDeletionPage);
      
    })
    .catch(error => console.log(error));
  
}

function loadAccountDeletionPage(){
  //fetch account deletion page
  fetch('./Views/deleteAcc.html')
    .then(response => response.text())
    .then(data => {
      // update content div with property evaluation page html
      content.innerHTML = data;
      const hamburger = document.getElementById("back");
      hamburger.addEventListener("click", loadHomePage);
      const cancel = document.getElementById("cancel");
      const confirm = document.getElementById("confirm");

      cancel.addEventListener('click', loadHomePage);
      confirm.addEventListener('click', ()=>{
        url = api + '/UserManagement/delete';
        del(url)
          .then(data => data.json())
          .then(response => console.log(response))
      })
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
      const hamburger = document.getElementById("back");
      hamburger.addEventListener("click", loadHomePage);
    })
    .catch(error => console.log(error));
}

function displayRequest(requests){
  const requestContainer = document.getElementById("request_container");
  let allrequest="";
  requests?.forEach((request) => {
    const requestElement = `
                            <tr>
                              <th scope="row">${request.requestId}</th>
                              <td>${request.propertyManagerName}</td>
                              <td>${request.propertyManagerEmail}</td>
                              <td>${request.serviceName}</td>
                              <td>${request.serviceType}</td>
                              <td>${request.serviceDescription}</td>
                              <td>${request.serviceFrequency}</td>
                              <td>${request.comments}</td>
                            </tr>
                          `;
    allrequest += requestElement;    
  });
  requestContainer.innerHTML = allrequest;

}

//fucntion to load request Management page
function loadRequestManagementPage() {
  // fetch request evaluation page html
  fetch('./Views/requestMan.html')
    .then(response => response.text())
    .then(data => {
      // Handle the response data
      content.innerHTML = data;
      const hamburger = document.getElementById("back");
      hamburger.addEventListener("click", loadHomePage);
      
      url = api + '/Request/getrequest';
      get(url)
        .then(data => data.json())
        .then(response => console.log(response))
        .then(response => displayRequest(response))
        .catch(error => console.log(error));
    })   
    .catch(error => console.log(error));
}


// From https://github.com/v-vong3/csulb/tree/master/cecs_491
function get(url) {

  const options = {
    method: 'GET',
    mode: 'cors',
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
    mode: 'cors',
    cache: 'default',
    credentials: 'include',
    headers: {

      'Content-Type': 'application/json'
    },
    redirect: 'follow',
    referrerPolicy: 'no-referrer-when-downgrade',
    body: JSON.stringify(data),
  };

  return fetch(url, options);
}

function del(url) {

  const options = {
    method: 'DELETE',
    mode: 'cors',
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

// load login page initially
loadLoginPage();