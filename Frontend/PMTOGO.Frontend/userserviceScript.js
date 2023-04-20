function loadFrequencyChangeRequest(id, homepageContent){
  fetch("./Views/frequencyChange.html")
    .then(response => response.text())
    .then(data => {
      // update content div with rate user page
      homepageContent.innerHTML = data;

      const backBtn = document.getElementById('BacktoServices');
      backBtn.addEventListener('click', loadHomePage);

      const fchangeForm = document.getElementById('fchange-form');
      fchangeForm.addEventListener('submit', (event) => {
        event.preventDefault();

        const time = document.querySelector('#times').value;
        const frequencies = document.querySelector('#frequencies').value;

        const frequency = time + "x/" + frequencies;
        frequency.toString;

        url = api + '/UserService/frequencyrequest';
        data = {id: id, frequency:frequency}   
        put(url, data)
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
            loadServiceManagementPage(homepageContent);
          }
        })
        .catch(error => console.log(error));
      })
    })
    
}
function loadCancelRequest(id, homepageContent){
  fetch("./Views/cancelService.html")
    .then(response => response.text())
    .then(data => {
      // update content div with rate user page
      homepageContent.innerHTML = data;

      const backBtn = document.getElementById('BacktoServices');
      backBtn.addEventListener('click', loadHomePage);

      const cancelForm = document.getElementById('cancel-form');
      cancelForm.addEventListener('submit', (event) => {
        event.preventDefault();

        url = api + '/UserService/cancelrequest';
        data = {id: id}   
        put(url, data)
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
            loadServiceManagementPage(homepageContent);
          }
        })
        .catch(error => console.log(error));
      })
    })
    
}

function loadRateUserService(id, homepageContent){
  fetch("./Views/rateservice.html")
    .then(response => response.text())
    .then(data => {
      // update content div with rate user page
      homepageContent.innerHTML = data;

      const backBtn = document.getElementById('BacktoServices');
      backBtn.addEventListener('click', loadHomePage);

      const rateForm = document.getElementById('rate-form');
      rateForm.addEventListener('submit', (event) => {
        event.preventDefault();

        const rate = document.querySelector('#rating').value;

        url = api + '/UserService/rate';
        data = {id: id, rate:rate}   
        put(url, data)
        .then(response => console.log(response))
        .then(loadServiceManagementPage(homepageContent))
        .catch(error => console.log(error))  
        // You can perform your registration API call here
      });
    })
    .catch(error => console.log(error))  
}
const createUserServiceTable = () =>{
  const userservices = document.querySelector("div.userservices");
  let tableHeaders = ["Service Name", "Service Type",  "Service Description", 
  "Service Frequency", "Service Provider Name", "Service Provider Email", "Status", "Rating", "Rate?", "Frequency Change?", "Cancel?"];
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
  if(userservice.propertyManagerName){
    userInfo.innerText= `${userservice.propertyManagerName}`;
  }
  else{
    userInfo.innerText= `${userservice.ServiceProviderName}`;
  }
  

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

  let changeAction = document.createElement('td');
  let changebtn = document.createElement('button');
  changebtn.innerText = "Change Frequency";
  changebtn.className="fchange-link";
  changebtn.id=`${userservice.id}`; 
  
  changeAction.append(changebtn);

  let cancelAction = document.createElement('td');
  let cancellbtn = document.createElement('button');
  cancellbtn.innerText = "Cancel";
  cancellbtn.className="cancel-link";
  cancellbtn.id=`${userservice.id}`; 
  
  cancelAction.append(cancellbtn);

  let rateAction = document.createElement('td');
  let ratebtn = document.createElement('button');
  ratebtn.innerText = "Rate";
  ratebtn.className="rate-link";
  ratebtn.id=`${userservice.id}`; 
  
  rateAction.append(ratebtn);

  userServiceTableBodyRow.append(serviceName,serviceType,serviceDescription,serviceFrequeny,
    serviceProvider,serviceProviderEmail,status,rating, rateAction, changeAction, cancelAction);
    //allrequest += requestTableBodyRow;
  UserServiceTable.append(userServiceTableBodyRow);
}
function getUserService(){
  url = api + '/UserService/getuserservice';
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
    else{
      response.json().then(data => {
        createUserServiceTable();
        let id = 0;
        data.forEach((userservice) =>{
          appendUserService(userservice, id);
          id = id + 1;
        })
        const homepageContent = document.getElementsByClassName("homepage-content")[0];
        const rateService = Array.from(document.getElementsByClassName("rate-link")); 
        rateService.forEach((key)=>{
          key.addEventListener('click', function(){loadRateUserService(key.id, homepageContent)});
        })
        const fchangeService = Array.from(document.getElementsByClassName("fchange-link")); 
        fchangeService.forEach((key)=>{
          key.addEventListener('click', function(){loadFrequencyChangeRequest(key.id, homepageContent)});
        })

        const cancellation = Array.from(document.getElementsByClassName("cancel-link")); 
        cancellation.forEach((key)=>{
          key.addEventListener('click', function(){loadCancelRequest(key.id, homepageContent)});
        })

          /*const emailAdmin = document.getElementById("notifyAdmin");
          emailAdmin.addEventListener('click', function() {loadEmailPage(homepageContent)});*/
        })
        .catch(error => console.error(error))
      }
  })
  .catch(error => console.error(error));
  
}