const content = document.getElementById('content');
const api = "https://localhost:7135/api"

window.addEventListener("load", function() {
  url = api + '/Authentication/IsLoggedIn';

  get(url)
  .then(response => response.json())
  .then(data => {
    if (data === true){
      loadHomePage();
    }
    else{
      loadLoginPage();
    }
  })
});


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
        .then((response) => {
          if (response.ok) {
            loadHomePage();
          }
        })
          .catch(error => console.log(error.text()));

      });
      
    })
    .catch(error => console.log(error));
}

var role="";
function UserRole() {
  if (document.getElementById("SP").checked) {
    role = "Service Provider";
  }
  else if (document.getElementById("PM").checked) {
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

    const homepageContent = document.getElementsByClassName("homepage-content")[0];

   //select propertyEvaluation
    const propertyEvalFeature = document.getElementById('propertyEvaluation');
     //select request management
    const requestFeature = document.getElementById('requestManagement');

    //add event listeners
    logoutUser.addEventListener('click', () => {
      url = api + '/Authentication/Logout';

      get(url)
        .then(response => response.json())
        .then(data => {
          if (data === false){

        alert('Logout was unsuccesful. Try again or contact an administratior.');
        loadHomePage();
        }
        else{
          loadLoginPage();
        }
      })
    });

    //add event listener to nav to request management
    requestFeature.addEventListener('click', () => {
      loadRequestManagementPage(homepageContent);
    });  

    // add event listeners to nav to property evaluation
    propertyEvalFeature.addEventListener('click', () => {
      loadPropertyEvalPage(homepageContent);
    });
    })
    .catch(error => console.log(error));
  
}

function loadAccountDeletionPage(homepageContent){
  //fetch account deletion page
  fetch('./Views/deleteAcc.html')
    .then(response => response.text())
    .then(data => {
      // update content div with property evaluation page html
      homepageContent.innerHTML = data;

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
function loadPropertyEvalPage(homepageContent) {

  // fetch property evaluation page html
  fetch('./Views/propEval.html')
    .then(response => response.text())
    .then(data => {
      // update content div with property evaluation page html
      homepageContent.innerHTML = data;

      LoadProfile();
      updateEvaluation();

      const evaluateForm = document.getElementById("PropertyProfile");

      evaluateForm.addEventListener('submit', function(event) {
        event.preventDefault();

        SaveProfile();
        updateEvaluation();
    });
    
      
    })
    .catch(error => console.log(error));

}

function updateEvaluation(){
  // Update the evaluation
  url = api + '/PropEval/evaluate';

  get(url)
  .then((response) => {
    if (response.ok) {
      return response.text();
    }
  })
  .then(data => {
    document.getElementById("property-evaluation").innerHTML = `Property Evaluation: ${data}`;
  })
  .catch(error => console.error(error));
}

async function LoadProfile(){
  // Auto loading the Property Profile if available
  url = api + '/PropEval/loadProfile';

  get(url)
  .then(response => {

    if(!response.ok){

      fetch("./Views/NotAuthorized.html")
      .then(response => response.text())
      .then(text => {

        const homepageContent = document.getElementsByClassName("homepage-content")[0];
    
        homepageContent.innerHTML = text;
      })

    } else {

      response.json().then(data => {
        const fieldset = document.querySelector('#PropertyProfile fieldset');
        assignData(fieldset,data);
      }).catch(error => console.error(error));

    }
  })
  .catch(error => console.error(error.text));
}

async function SaveProfile(){

  // Saving the Property Profile
  url = api + '/PropEval/saveProfile';

  const fieldset = document.querySelector('#PropertyProfile fieldset');

  let data = extractData(fieldset);
  console.log(data);
  await put(url,data)
  .catch(error => console.error(error));
}

function assignData(fieldset, data){
  const inputs = fieldset.querySelectorAll('input, textarea');
  inputs.forEach(input => {

    // Lower casing the first letter of the inout field
    // to match the property name in data
    const key = input.name.charAt(0).toLowerCase() + input.name.slice(1);

    if(data.hasOwnProperty(key)){
      if(data[key] !== 'string' && data[key] !== 0)
      {
        //console.log(data[key]);
        input.setAttribute('value',data[key]);
      }
    }
  });
}

function extractData(fieldset){
  const inputs = fieldset.querySelectorAll('input, textarea');
  let data = {};

  inputs.forEach(input => {
    const key = input.name.charAt(0).toLowerCase() + input.name.slice(1);
    data[key] = input.value;
  })
  console.log(data)
  return data;
}

const createRequestsTable = () =>{
  const requests = document.querySelector("div.requests");
  let tableHeaders = ["Request ID", "Service Name", "Service Type",  "Service Description", 
  "Service Frequency", "Comments", "Property Manager Name", "Property Manager Email", "Action"];
    while (requests.firstChild) requests.removeChild(requests.firstChild)
    let requestsTable = document.createElement('table');
    requestsTable.className="requestsTable";
    requestsTable.id="requestsTable";

    let requestTableHead = document.createElement("thead");
    requestTableHead.className="requestsTableHead";

    let requestTableHeaderRow= document.createElement('tr')
    requestTableHeaderRow.className= "requestHeaderRow";

    tableHeaders.forEach(header =>{
      let requestsHeader = document.createElement('th');
      requestsHeader.innerText = header;
      requestTableHeaderRow.append(requestsHeader);
    })

    requestTableHead.append(requestTableHeaderRow);
    requestsTable.append(requestTableHead);

    let requestsTableBody = document.createElement('tbody');
    requestsTableBody.className="requestsTableBody"
    requestsTable.append(requestsTableBody);

    requests.append(requestsTable);
}
const appendRequest =(request, id) => {
  const requestsTable = document.querySelector(".requestsTable");
  const userInfo = document.querySelector(".userInfo");
  userInfo.innerText= `${request[0].serviceProviderName}`;
  //let allrequest = ""
  console.log(Object.entries(request));
  let requestTableBodyRow = document.createElement('tr');
  requestTableBodyRow.className = "requestTableBodyRow";
  requestTableBodyRow.id= String(id);

  //add the data
  let requestId = document.createElement('td');
  requestId.innerText = `${request[0].requestId}`;

  let serviceName = document.createElement('td');
  serviceName.innerText = `${request[0].serviceName}`;

  let serviceType = document.createElement('td');
  serviceType.innerText = `${request[0].serviceType}`;

  let serviceDescription = document.createElement('td');
  serviceDescription.innerText = `${request[0].serviceDescription}`;

  let serviceFrequeny = document.createElement('td');
  serviceFrequeny.innerText = `${request[0].serviceFrequency}`;

  let comments = document.createElement('td');
  comments.innerText = `${request[0].comments}`;

  let propertyManager = document.createElement('td');
  propertyManager.innerText = `${request[0].propertyManagerName}`;

  let propertyManagerEmail = document.createElement('td');
  propertyManagerEmail.innerText = `${request[0].propertyManagerEmail}`;

  let acceptAction = document.createElement('td');
  let accept = document.createElement('button');
  accept.className ="accept";
  accept.innerText = "Accept";
  acceptAction.append(accept);

  let decline = document.createElement('button');
  decline.className="decline";
  decline.innerText = "Decline";

  requestTableBodyRow.append(requestId,serviceName,serviceType,serviceDescription,serviceFrequeny,
    comments,propertyManager,propertyManagerEmail,accept,decline);
    //allrequest += requestTableBodyRow;
  requestsTable.append(requestTableBodyRow);
}

function getrequest(){
  url = api + '/Request/getrequest';
  get(url)
    .then(response => response.json())
    //.then(response => console.log(response))
    .then(response => {
      createRequestsTable();
      response.forEach(() => {
        let id = 1;
        appendRequest(response, id);
        id += 1;})
    })
    .catch(error => console.log(error));
}
function acceptRequest(){
  console.log("accept this request");
}
function declineRequest(){
  console.log("decline this request");
}
//fucntion to load request Management page
function loadRequestManagementPage(homepageContent) {
  // fetch request evaluation page html
  fetch('./Views/requestMan.html')
    .then(response => response.text())
    .then(data => {
      // Handle the response data
      homepageContent.innerHTML = data;
      const hamburger = document.getElementById("back");
      hamburger.addEventListener("click", loadHomePage);  
      
      
      getrequest();
        
      console.log("here");
      const accept = document.getElementsByClassName("accept");
      accept.addEventListener('click', acceptRequest());

    })   
    .catch(error => console.log(error));
    
}

function logout(){
  url = api + '/Authentication/Logout';

  get(url)
    .then(response => response.json())
    .then(data => {
      if (data === false){

        alert('Logout was unsuccesful. Try again or contact an administratior.');
        loadHomePage();
      }
      else{
        loadLoginPage();
      }
    })
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

function put(url, data) {
  const options = {
    method: 'PUT',
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

