const api = "https://localhost:7135/api";
const content = document.getElementById('content');
var userrole = "";

function loadUnAuthPage(){
    // fetch login page
    fetch('./Views/unauthUser.html')
    .then(response => response.text())
    .then(data => {
      // update content div with login page
      content.innerHTML = data;

      // select register link
      const loginLink = document.getElementById('login');
      loginLink.addEventListener('click', loadLoginPage);

      loadServices("Unauthorized User","unauth-content");
      // select forgot password button
    })

}
window.addEventListener("load", function() {
  url = api + '/Authentication/IsLoggedIn';

  get(url)
  .then(response => response.json())
  .then(data => {
    if (data === true){
      loadHomePage(userrole);
    }
    else{
      loadUnAuthPage();
      //loadLoginPage();
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
        const forgotPasswordButton = document.getElementById('forgotPass');

      // add event listeners to register link, forgot password button
        registerLink.addEventListener('click', loadRegisterPage);
        forgotPasswordButton.addEventListener('click', loadForgotPasswordPage);

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
            response.json().then(data => {
                userrole = `${data.claims[1].value}`;
              loadHomePage(userrole, username);
            })
          }
        })
        .catch(error => {
          console.log(error.text())
        });

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

function loadForgotPasswordPage() {
    // fetch recovery page html
  fetch("./Views/recovery.html")
    .then(response => response.text())
    .then(data => {
        // update content div with recovery page
      content.innerHTML = data;

      const emailInput = document.getElementById('email-input');
      const submitBtn = document.getElementById('forgot-password-submit-btn');

      // add event listener to submit button
      submitBtn.addEventListener('click', (event) => {
        event.preventDefault();

          // get user email
        const email = emailInput.value;

          // perform recovery action
        const url = api + '/UserManagement/recovery';
        const data = { email: email }

        send(url, data)
          .then(response => {
            console.log(response);

            // after successful recovery, load OTP page
            loadEnterOTPPage(email);
          })
          .catch(error => console.log(error));
      });
    })
    .catch(error => console.log(error))
}

function loadEnterOTPPage(email) {
  // fetch otpForm page html
  fetch("./Views/otpForm.html")
      .then(response => response.text())
      .then(data => {
          // update content div with recovery page
          content.innerHTML = data;

          const otpInput = document.getElementById('otp-input');
          const submitBtn = document.getElementById('otp-submit-btn');

          // add event listener to submit button
          submitBtn.addEventListener('click', (event) => {
              event.preventDefault();

              // get user otp
              const otp = otpInput.value;

              // perform otp action
              const url = api + '/UserManagement/otp';
              const data = { email: email, otp: otp };

              send(url, data)
                  .then(response => {
                      if (response.ok) {
                          // after successful otp, load password update page
                          loadUpdatePasswordPage(email);
                      } else {
                          throw new Error('OTP validation failed');
                      }
                  })
                  .catch(error => console.log(error));
          });
      })
      .catch(error => console.log(error))
}

function loadUpdatePasswordPage(email) {
    // fetch update password page html
  fetch("./Views/updatePassword.html")
    .then(response => response.text())
    .then(data => {
      // update content div with recovery page
      content.innerHTML = data;

      const passwordInput = document.getElementById('password-input');
      const submitBtn = document.getElementById('update-password-submit-btn');

      // add event listener to submit button
      submitBtn.addEventListener('click', (event) => {
      event.preventDefault();

      // get user otp
      const newPassword = passwordInput.value;

      // perform otp action
      const url = api + '/UserManagement/updatePassword';
      const data = { email: email, password: newPassword };

      send(url, data)
        .then(response => {
          if (response.ok) {
            // after successful otp, load password update page
            loadLoginPage();
          } else {
            throw new Error('failed');
          }
        })
        .catch(error => console.log(error));
      });
  })
  .catch(error => console.log(error))
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

const createServicesTable = (page) =>{
  const contentpage = "div." + page;
  const services = document.querySelector(contentpage);
  let tableHeaders = ["Service Name", "Service Type",  "Service Description",
 "Service Provider Name", "Service Provider Email", "Price ($)", "Request Service?", "Add to Project"];
    while (services.firstChild) services.removeChild(services.firstChild)
    let ServiceTable = document.createElement('table');
    ServiceTable.className="ServiceTable";
    ServiceTable.id="ServiceTable";

    let ServiceTableHead = document.createElement("thead");
    ServiceTableHead.className="ServiceTableHead";

    let ServiceTableHeaderRow= document.createElement('tr')
    ServiceTableHeaderRow.className= "ServiceHeaderRow";

    tableHeaders.forEach(header =>{
      let ServiceHeader = document.createElement('th');
      ServiceHeader.innerText = header;
      ServiceTableHeaderRow.append(ServiceHeader);
    })

    ServiceTableHead.append(ServiceTableHeaderRow);
    ServiceTable.append(ServiceTableHead);

    let ServiceTableBody = document.createElement('tbody');
    ServiceTableBody.className="ServiceTableBody"
    ServiceTable.append(ServiceTableBody);

    services.append(ServiceTable);
}

const appendServices =(service, id) => {
  const ServiceTable = document.querySelector(".ServiceTable");

  let ServiceTableBodyRow = document.createElement('tr');
  ServiceTableBodyRow.className = "ServiceTableBodyRow";
  ServiceTableBodyRow.id= String(id);

  //add the data
  let serviceName = document.createElement('td');
  serviceName.innerText = `${service.serviceName}`;

  let serviceType = document.createElement('td');
  serviceType.innerText = `${service.serviceType}`;

  let serviceDescription = document.createElement('td');
  serviceDescription.innerText = `${service.serviceDescription}`;

  let serviceProvider = document.createElement('td');
  serviceProvider.innerText = `${service.serviceProvider}`;

  let serviceProviderEmail = document.createElement('td');
  serviceProviderEmail.innerText = `${service.serviceProviderEmail}`;

  let price = document.createElement('td');
  price.innerText ="$" + `${service.servicePrice}`;

  let requestAction = document.createElement('td');
  let requestbtn = document.createElement('button');
  requestbtn.innerText = "Book Service";
  requestbtn.className="servicerequest-link";
  requestbtn.id=`${service.id}`;

  requestAction.append(requestbtn);

  let projectAction = document.createElement('td');
  let projectbtn = document.createElement('button');
  projectbtn.innerText = "Add to Project";
  projectbtn.className="addproject-link";
  //may change id
  projectbtn.id=`${service.id}`;

  projectAction.append(projectbtn);

  ServiceTableBodyRow.append(serviceName,serviceType,serviceDescription,
    serviceProvider,serviceProviderEmail,price, requestAction, projectAction);

  ServiceTable.append(ServiceTableBodyRow);
}
function loadNewRequest(id, userrole, homepageContent){
  if(userrole === "Unauthorized User"){
    loadLoginPage;
  }
  else{
    fetch("./Views/newRequest.html")
    .then(response => response.text())
    .then(data => {
      // update content div with rate user page
      homepageContent.innerHTML = data;

      const backBtn = document.getElementById('BacktoServices');
      backBtn.addEventListener('click', loadHomePage);

      const requestForm = document.getElementById('newrequest-form');
      requestForm.addEventListener('submit', (event) => {
        event.preventDefault();

        const time = document.querySelector('#times').value;
        const frequencies = document.querySelector('#frequencies').value;
        const comments = document.querySelector('#comments').value;

        const frequency = time + "x/" + frequencies;
        frequency.toString;

        url = api + "/UserService/addrequests";
        data = {id:id, frequency: frequency, comments:comments};
        send(url, data)
        .then(response => {
          if(!response.ok){
            fetch("./Views/NotAuthorized.html")
              .then(response => response.text())
              .then(text => {

                const homepageContent = document.getElementsByClassName("homepage-content")[0];

                homepageContent.innerHTML = text;
              })
              .catch(error => console.error(error))
          }
          else{
            loadHomePage(userrole)
          }
        })
        .catch(error => console.log(error));

      });
    })
    .catch(error => console.log(error))
  }
}
function loadServices(userrole, page){
  url = api + '/UserService/getservice';
  get(url)
    .then(response => response.json())
    .then(data => {
      const Content = document.getElementsByClassName(page)[0];
      Content.innerHTML = data;

      createServicesTable(page);
      let id = 0;
      data.forEach((service) => {
        appendServices(service, id)
        id = id + 1;
      })
      const requestlist = Array.from(document.getElementsByClassName("servicerequest-link"));
      requestlist.forEach((key)=>{
        key.addEventListener('click', function()
        {
          if(userrole === "Unauthorized User"){
            loadLoginPage();
          }
          loadNewRequest(key.id, userrole, Content)
        })
      })
      const projectlist = Array.from(document.getElementsByClassName("addproject-link"));
      projectlist.forEach((key)=>{
        key.addEventListener('click', function()
        {
          if(userrole === "Unauthorized User"){
            loadLoginPage();
          }
          //to be changed
          loadNewRequest(key.id, userrole, Content)
        })
      })
  })
  .catch(error => console.error(error));
}
//function to load homepage
function loadHomePage(userrole, username) {
    // fetch property evaluation page html
  fetch('./Views/homepage.html')
    .then(response => response.text())
    .then(data => {
        // update content div with property evaluation page html
      content.innerHTML = data;

      const userinfo = document.getElementById("userrole");
      userinfo.innerHTML = userrole;

      const hamburger = document.getElementById("back");
      hamburger.addEventListener("click", loadHomePage);

      loadServices(userrole, "homepage-content");
      //select log out
      const logoutUser = document.getElementById("logout");

            const homepageContent = document.getElementsByClassName("homepage-content")[0];
            //select settings
            const deleteAccount = document.getElementById("settings");
            //select propertyEvaluation
            const propertyEvalFeature = document.getElementById('propertyEvaluation');
            //select request management
            const requestFeature = document.getElementById('requestManagement');
            //select service Management
            const serviceFeature = document.getElementById('serviceManagement');
            //select crime alert
            const crimeMapFeature = document.getElementById('crimemap');
            //select crime alert
            const diyFeature = document.getElementById('diy');
            //select sp services
            const spserviceFeature = document.getElementById('Services');
            //select Maintenance And Renovation
            const maintenanceFeature = document.getElementById('maintenance');


      const adminFeature = document.getElementById('admin');

        //add event listeners
      logoutUser.addEventListener('click', () => {
        url = api + '/Authentication/Logout';
        get(url)
          .then(response => response.json())
          .then(data => {
            if (data === false) {
              alert('Logout was unsuccesful. Try again or contact an administratior.');
              loadHomePage();
            }
            else {
              loadLoginPage();
            }
        })
      });
        //add event listener to nav to account delection

        deleteAccount.addEventListener('click', () => {
            loadAccountDeletionPage(homepageContent);
        });
        //add event listener to nav to request management
        requestFeature.addEventListener('click', () => {
            loadRequestManagementPage(homepageContent);
        });

        //add event listener to nav to service management
        serviceFeature.addEventListener('click', () => {
            loadServiceManagementPage(homepageContent);
        });

        //add event listener to nav to service management
        spServiceFeature.addEventListener('click', () => {
          loadServicePage(homepageContent);
      });

        //add event listener to nav to crime map
        crimeMapFeature.addEventListener('click', () => {
            loadCrimeMapPage(homepageContent, username, userrole);
        });


        //add event listener to nav to diy
        diyFeature.addEventListener('click', () => {
            loadDIYPage(homepageContent, username, userrole);
        });

        // add event listeners to nav to property evaluation
        propertyEvalFeature.addEventListener('click', () => {
            loadPropertyEvalPage(homepageContent);
        });

         // add event listeners to nav to admin
        adminFeature.addEventListener('click', () => {
          loadAdminPage(homepageContent);
        });

        // add event listeners to nav to MnR
        maintenanceFeature.addEventListener('click', () => {
          LoadMnRPage(homepageContent);
          });

        spserviceFeature.addEventListener('click', () => {
          loadServicePage(homepageContent);
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

      const cancel = document.getElementById("cancel");
      const confirm = document.getElementById("confirm");

      cancel.addEventListener('click', loadHomePage(userrole));
      confirm.addEventListener('click', ()=>{
        url = api + '/UserManagement/delete';
        del(url)
          .then(data => data.json())
          .then(response => console.log(response))
          .then(loadLoginPage);
      })

    })
    .catch(error => console.log(error));
}

function loadEmailPage(homepageContent){
  fetch('./Views/Email.html')
    .then(response => response.text())
    .then(data => {
      // Handle the response data
      homepageContent.innerHTML = data;

      const emailForm = document.getElementById('email-form');

      // add event listener to register form submit
      emailForm.addEventListener('submit', (event) => {
        event.preventDefault();

        // perform registration action
        const firstName = document.querySelector('#firstName').value;
        const lastName = document.querySelector('#lastName').value;
        const description = document.querySelector('#description').value;
        var subject = "Request Management issue";

        url = api + '/Request/email';
        data = {FirstName: firstName, LastName: lastName, Subject: subject, Description: description}

        send(url, data)
          .then(data => data.json())
          .then(response => console.log(response))
          .then(response => alert(response))
          .then(loadRequestManagementPage());
      });

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
