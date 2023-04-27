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
        const forgotPasswordButton = document.getElementById('forgotPass');

      // add event listeners to register link, forgot password button
        registerLink.addEventListener('click', loadRegisterPage);
        forgotPasswordButton.addEventListener('click', /*loadCrimeMapPage*/loadForgotPasswordPage);

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
              loadHomePage(`${data.claims[1].value}`, username);
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

function loadCrimeMapPage(homepageContent, username) {
    // fetch crime map page html
    fetch('./Views/crimeMap.html')
        .then(response => response.text())
        .then(async (data) => {
            // update homepage content div with crime map page html
            homepageContent.innerHTML = data;

            // add image to crime map page
            const imageContainer = document.createElement('div');
            imageContainer.classList.add('image-container');

            const image = document.createElement('img');
            image.src = './images/crimemap.png';
            image.alt = 'Crime map';
            imageContainer.appendChild(image);

            const crimeMapContent = document.getElementById('crime-map-content');
            crimeMapContent.appendChild(imageContainer);

            // add styles to image container
            imageContainer.style.overflow = 'auto';
            imageContainer.style.width = '1000px';
            imageContainer.style.height = '1000px';
            image.style.transform = 'scale(1)';
            image.style.transition = 'transform 0.5s';

            // set initial scroll position
            imageContainer.scrollLeft = 5400;
            imageContainer.scrollTop = 7300;

            // add event listener to image for panning
            let isDragging = false;
            let lastX, lastY;
            image.addEventListener('mousedown', function (event) {
                event.preventDefault();
                isDragging = true;
                lastX = event.clientX;
                lastY = event.clientY;
            });
            image.addEventListener('mousemove', function (event) {
                event.preventDefault();
                if (isDragging) {
                    const deltaX = event.clientX - lastX;
                    const deltaY = event.clientY - lastY;
                    lastX = event.clientX;
                    lastY = event.clientY;
                    imageContainer.scrollLeft -= deltaX;
                    imageContainer.scrollTop -= deltaY;
                }
            });
            image.addEventListener('mouseup', function (event) {
                event.preventDefault();
                isDragging = false;
            });

            // add "Add Crime Alert" button
            const addCrimeAlertButton = document.createElement('addbtn');
            addCrimeAlertButton.textContent = 'Add Crime Alert';
            addCrimeAlertButton.addEventListener('click', function () {
                loadAddAlertPage(homepageContent, username);
            });
            crimeMapContent.appendChild(addCrimeAlertButton);

            // get alerts and mark on map
            const alerts = await getAlerts();
            alerts.forEach(alert => {
                const button = document.createElement('button');
                button.classList.add('dot');
                button.style.position = 'absolute';
                button.style.width = '10px';
                button.style.height = '10px';
                button.style.backgroundColor = 'red';
                button.style.borderRadius = '50%';
                button.style.left = `${alert.x}px`;
                button.style.top = `${alert.y}px`;
                imageContainer.appendChild(button);
                console.log(`Marked alert: ${alert.ID}`);

                // add click event listener to button
                button.addEventListener('click', () => {
                    // create popup
                    const popup = document.createElement('div');
                    popup.style.position = 'absolute';
                    popup.style.top = '50%';
                    popup.style.left = '50%';
                    popup.style.transform = 'translate(-50%, -50%)';
                    popup.style.backgroundColor = 'white';
                    popup.style.padding = '20px';
                    popup.style.borderRadius = '10px';
                    popup.style.boxShadow = '0 0 10px rgba(0, 0, 0, 0.5)';
                    popup.style.zIndex = '1';

                    // add close button to popup
                    const closeButton = document.createElement('button');
                    closeButton.innerHTML = 'X';
                    closeButton.style.position = 'absolute';
                    closeButton.style.top = '5px';
                    closeButton.style.right = '5px';
                    closeButton.addEventListener('click', () => {
                        popup.remove();
                    });
                    popup.appendChild(closeButton);

                    // add form to popup
                    const form = document.createElement('form');
                    form.style.display = 'flex';
                    form.style.flexDirection = 'column';
                    form.style.gap = '10px';

                    // add input fields to form
                    const nameLabel = document.createElement('label');
                    nameLabel.innerHTML = 'Name:';
                    const nameInput = document.createElement('input');
                    nameInput.type = 'text';
                    form.appendChild(nameLabel);
                    form.appendChild(nameInput);

                    const descriptionLabel = document.createElement('label');
                    descriptionLabel.innerHTML = 'Description:';
                    const descriptionInput = document.createElement('textarea');
                    descriptionInput.rows = '5';
                    form.appendChild(descriptionLabel);
                    form.appendChild(descriptionInput);

                    // add save button to form
                    const saveButton = document.createElement('button');
                    saveButton.innerHTML = 'Save';
                    saveButton.type = 'submit';
                    form.appendChild(saveButton);

                    popup.appendChild(form);
                    document.body.appendChild(popup);
                });
            });
        });
}


async function getAlerts() {
    try {
        const response = await fetch(api+'/CrimeAlert/getAlerts');
        if (response.ok) {
            const data = await response.json();
            return data;
        } else {
            throw new Error('Error');
        }
    } catch {
        throw new Error('Error');
    }
}

function loadAddAlertPage(homepageContent, username) {
    // fetch add alert page html
    fetch('./Views/addAlert.html')
        .then(response => response.text())
        .then(data => {
            // update homepage content div with add alert page html
            homepageContent.innerHTML = data;

            const crimeMapImage = document.createElement('img');
            crimeMapImage.src = './images/crimemap.png';
            crimeMapImage.alt = 'Crime map';

            const crimeMapContainer = document.createElement('div');
            crimeMapContainer.classList.add('image-container');
            crimeMapContainer.style.overflow = 'auto';
            crimeMapContainer.style.width = '750px';
            crimeMapContainer.style.height = '750px';
            crimeMapContainer.appendChild(crimeMapImage);

            const addAlertContainer = document.getElementById('add-alert-container');
            addAlertContainer.appendChild(crimeMapContainer);

            const marker = document.createElement('div');
            marker.classList.add('marker');
            crimeMapContainer.appendChild(marker);

            let isPlacingMarker = true;
            let xCoordinate = '';
            let yCoordinate = '';

            // add event listener to image for placing marker
            crimeMapImage.addEventListener('click', function addMarker(event) {
                if (isPlacingMarker) {
                    marker.style.display = 'block';
                    marker.style.left = (event.clientX - crimeMapContainer.getBoundingClientRect().left + crimeMapContainer.scrollLeft - 10) + 'px';
                    marker.style.top = (event.clientY - crimeMapContainer.getBoundingClientRect().top + crimeMapContainer.scrollTop - 10) + 'px';

                    // log location in pixels
                    xCoordinate = (event.clientX - crimeMapContainer.getBoundingClientRect().left + crimeMapContainer.scrollLeft).toString();
                    yCoordinate = (event.clientY - crimeMapContainer.getBoundingClientRect().top + crimeMapContainer.scrollTop).toString();

                    isPlacingMarker = false;
                }
            });

            const submitButton = document.getElementById('submit-button');
            submitButton.addEventListener('click', function () {
                const nameInput = document.getElementById('name-input').value;
                const locationInput = document.getElementById('location-input').value;
                const descriptionInput = document.getElementById('description-input').value;
                const timeInput = document.getElementById('time-input').value;
                const dateInput = document.getElementById('date-input').value;

                const url = api + '/CrimeAlert/addAlert';
                const data = {
                    Email: username,
                    Name: nameInput,
                    Location: locationInput,
                    Description: descriptionInput,
                    Time: timeInput,
                    Date: dateInput,
                    X: xCoordinate,
                    Y: yCoordinate
                };

                send(url, data)
                    .then(response => {
                        console.log(data);
                        console.log(response);

                        loadCrimeMapPage(homepageContent);
                    })
                    .catch(error => console.log(error));
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

//function to load homepage
function loadHomePage(userrole, username) {
    // fetch property evaluation page html
    fetch('./Views/homepage.html')
        .then(response => response.text())
        .then(data => {
            // update content div with property evaluation page html
            content.innerHTML = data;

            const hamburger = document.getElementById("back");
            hamburger.addEventListener("click", loadHomePage);

            const userinfo = document.getElementById("userrole");
            userinfo.innerHTML = userrole;
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

            //add event listener to nav to crime map
            crimeMapFeature.addEventListener('click', () => {
                loadCrimeMapPage(homepageContent, username);
            });

            // add event listeners to nav to property evaluation
            propertyEvalFeature.addEventListener('click', () => {
                loadPropertyEvalPage(homepageContent);
            });
        })
        .catch(error => console.log(error));
}

/*function loadCrimeMapPage() {
    // fetch crime map page html
    fetch('./Views/crimeMap.html')
        .then(response => response.text())
        .then(data => {

            // add image to crime map page
            const imageContainer = document.createElement('div');
            imageContainer.classList.add('image-container');

            const image = document.createElement('img');
            image.src = './images/crimemap.png';
            image.alt = 'Crime map';
            imageContainer.appendChild(image);

            const crimeMapContent = document.getElementById('crime-map-content');
            crimeMapContent.appendChild(imageContainer);

            // attach event listener to back button
            const backButton = document.getElementById('back');
            if (backButton) {
                backButton.addEventListener('click', () => {
                    loadHomePage();
                });
            }
        })
        .catch(error => console.log(error));
}*/


function loadAccountDeletionPage(homepageContent){
  //fetch account deletion page
  fetch('./Views/deleteAcc.html')
    .then(response => response.text())
    .then(data => {
      // update content div with property evaluation page html
      homepageContent.innerHTML = data;

      const cancel = document.getElementById("cancel");
      const confirm = document.getElementById("confirm");

      cancel.addEventListener('click', loadHomePage);
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
  "Service Frequency", "Comments", "Property Manager Name", "Property Manager Email", "Accept?", "Decline?"];
    while (requests.firstChild) requests.removeChild(requests.firstChild)
    let requestsTable = document.createElement('table');
    requestsTable.className="requestsTable";
    requestsTable.id="requestsTable";
    requestsTable.pageLength = 6;

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
  userInfo.innerText= `${request.serviceProviderName}`;

  let requestTableBodyRow = document.createElement('tr');
  requestTableBodyRow.className = "requestTableBodyRow";
  requestTableBodyRow.id= String(id);

  //add the data
  let requestId = document.createElement('td');
  requestId.innerText = `${request.id}`;

  let serviceName = document.createElement('td');
  serviceName.innerText = `${request.serviceName}`;

  let serviceType = document.createElement('td');
  serviceType.innerText = `${request.serviceType}`;

  let serviceDescription = document.createElement('td');
  serviceDescription.innerText = `${request.serviceDescription}`;

  let serviceFrequeny = document.createElement('td');
  serviceFrequeny.innerText = `${request.serviceFrequency}`;

  let comments = document.createElement('td');
  comments.innerText = `${request.comments}`;

  let propertyManager = document.createElement('td');
  propertyManager.innerText = `${request.propertyManagerName}`;

  let propertyManagerEmail = document.createElement('td');
  propertyManagerEmail.innerText = `${request.propertyManagerEmail}`;

  let acceptAction = document.createElement('td');
  let acceptbtn = document.createElement('button');
  acceptbtn.className ="accept"; 
  acceptbtn.id=`${request.id}`;
  acceptbtn.innerText = "Accept";

  let declineAction = document.createElement('td');
  let declinebtn = document.createElement('button');
  declinebtn.innerText = "Decline";
  declinebtn.className="decline";
  declinebtn.id=`${request.id}`; 
  
  acceptAction.append(acceptbtn);
  declineAction.append(declinebtn);

  requestTableBodyRow.append(requestId,serviceName,serviceType,serviceDescription,serviceFrequeny,
    comments,propertyManager,propertyManagerEmail,acceptAction,declineAction);
    //allrequest += requestTableBodyRow;
  requestsTable.append(requestTableBodyRow);
}
function acceptRequest(requestid){
  const homepageContent = document.getElementsByClassName("homepage-content")[0];
  url = api + "/ServiceRequest/accept";
  data = {id: requestid}
  send(url, data)
  .then(data => data.json())
  .then(response => console.log(response))
  .then(loadRequestManagementPage(homepageContent));
  
}
function declineRequest(requestid){
  const homepageContent = document.getElementsByClassName("homepage-content")[0];
  url = api + "/ServiceRequest/decline";
  data = {id: requestid}
  send(url, data)
    .then(data => data.json())
    .then(response => console.log(response))
    .then(loadRequestManagementPage(homepageContent));
}
function getrequest(){
  url = api + '/ServiceRequest/getrequest';
  get(url)
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
      else {
        response.json().then(data => {
          createRequestsTable();
          let id = 0;
          data.forEach((request) => {         
            appendRequest(request, id)
            id = id + 1;
          })
          const acceptlist = Array.from(document.getElementsByClassName("accept")); 
          acceptlist.forEach((key)=>{
            key.addEventListener('click', function() { acceptRequest(key.id)});
          })
          const declinelist = Array.from(document.getElementsByClassName("decline")); 
          declinelist.forEach((key)=>{
            key.addEventListener('click', function() {declineRequest(key.id)});
          })
        })
        .catch(error => console.error(error));
      }
    })
    .catch(error => console.error(error));
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

//fucntion to load request Management page
function loadRequestManagementPage(homepageContent) {
  // fetch request evaluation page html
  fetch('./Views/requestMan.html')
    .then(response => response.text())
    .then(data => {
      // Handle the response data
      homepageContent.innerHTML = data;
      
      getrequest();

    })   
    .catch(error => console.log(error));
    
}
function rateUserService(id, rate){
  const homepageContent = document.getElementsByClassName("homepage-content")[0];
  url = api + "/Service/rate";
  data = {id: id, rate: rate}
  put(url, data)
  .then(response => console.log(response))
  .then(loadServiceManagementPage(homepageContent));
  
}
const createUserServiceTable = () =>{
  const userservices = document.querySelector("div.userservices");
  let tableHeaders = ["Service Name", "Service Type",  "Service Description", 
  "Service Frequency", "Service Provider Name", "Service Provider Email", "Status", "Rating", "Rate?", "Frequency Change?"];
    while (userservices.firstChild) userservices.removeChild(userservices.firstChild)
    let userServiceTable = document.createElement('table');
    userServiceTable.className="userServiceTable";
    userServiceTable.id="userServiceTable";

    let userServiceTableHead = document.createElement("thead");
    userServiceTableHead.className="userServiceTableHead";

    let userServiceTableHeaderRow= document.createElement('tr')
    userServiceTableHeaderRow.className= "userServiceHeaderRow";

    tableHeaders.forEach(header =>{
      let userServiceHeader = document.createElement('th');
      userServiceHeader.innerText = header;
      userServiceTableHeaderRow.append(userServiceHeader);
    })

    userServiceTableHead.append(userServiceTableHeaderRow);
    userServiceTable.append(userServiceTableHead);

    let userServiceTableBody = document.createElement('tbody');
    userServiceTableBody.className="userServiceTableBody"
    userServiceTable.append(userServiceTableBody);

    userservices.append(userServiceTable);
}

const appendUserService =(userservice, id) => {
  const UserServiceTable = document.querySelector(".userServiceTable");
  const userInfo = document.querySelector(".userInfo");
  userInfo.innerText= `${userservice.propertyManagerName}`;

  let userServiceTableBodyRow = document.createElement('tr');
  userServiceTableBodyRow.className = "userServiceTableBodyRow";
  userServiceTableBodyRow.id= String(id);

  //add the data
  let serviceName = document.createElement('td');
  serviceName.innerText = `${userservice.serviceName}`;

  let serviceType = document.createElement('td');
  serviceType.innerText = `${userservice.serviceType}`;

  let serviceDescription = document.createElement('td');
  serviceDescription.innerText = `${userservice.serviceDescription}`;

  let serviceFrequeny = document.createElement('td');
  serviceFrequeny.innerText = `${userservice.serviceFrequency}`;

  let serviceProvider = document.createElement('td');
  serviceProvider.innerText = `${userservice.serviceProviderName}`;

  let serviceProviderEmail = document.createElement('td');
  serviceProviderEmail.innerText = `${userservice.serviceProviderEmail}`;

  let status = document.createElement('td');
  status.innerText = `${userservice.status}`;

  let rating = document.createElement('td');
  rating.innerText = `${userservice.rating}`;

  let rateAction = document.createElement('td');
  let rateForm = document.createElement('form');
  rateForm.id = `${userservice.id}`;
  rateForm.className="rateform"
  let rateInput = document.createElement('input');
  rateInput.type ="number";
  rateInput.placeholder="Enter rating 1-5";
  rateInput.id ="rating"

  let ratebtn = document.createElement('button');
  ratebtn.className="ratebtn";
  ratebtn.type="submit"
  ratebtn.innerText = "Submit";

  rateAction.append(rateForm);
  rateForm.append(rateInput);
  rateForm.append(ratebtn);

  userServiceTableBodyRow.append(serviceName,serviceType,serviceDescription,serviceFrequeny,
    serviceProvider,serviceProviderEmail,status,rating, rateAction);
    //allrequest += requestTableBodyRow;
  UserServiceTable.append(userServiceTableBodyRow);
}
//fucntion to load service Management page
function loadServiceManagementPage(homepageContent) { 
  // fetch request evaluation page html
  fetch('./Views/serviceMan.html')
    .then(response => response.text())
    .then(data => {
      // Handle the response data
      homepageContent.innerHTML = data;
      
      //getrequest();
      url = api + '/Service/getuserservice';
      get(url)
        .then(response => response.json())
        .then(response => {
            createUserServiceTable();
            let id = 0;
            response.forEach((userservice) =>{
              appendUserService(userservice, id)
              id = id + 1;
            })
            const rateService = Array.from(document.getElementsByClassName("rateform")); 
            rateService.forEach((key)=>{
              key.addEventListener('submit', (event) => {
                event.preventDefault();   

                const rate = document.querySelector('#rating').value;
                rateUserService(key.id, rate)});
            })

            /*const emailAdmin = document.getElementById("notifyAdmin");
            emailAdmin.addEventListener('click', function() {loadEmailPage(homepageContent)});*/
        })
        .catch(error => console.log(error));

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

