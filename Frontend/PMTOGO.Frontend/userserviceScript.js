function rateUserService(id, homepageContent){
  fetch("./Views/rateservice.html")
  .then(response => response.text())
  .then(data => {
    // update content div with register page
    content.innerHTML = data;

    const backBtn = document.getElementById('backServices');

    // add event listeners
    backBtn.addEventListener('click', loadServiceManagementPage(homepageContent))

    // select register form
    const rateForm = document.getElementById('rate-form');

    // add event listener to register form submit
    rateForm.addEventListener('submit', (event) => {
      event.preventDefault();

      const rate = document.querySelector('#rating').value;


      // perform registration action
      url = api + '/UserService/rate';
      data = {id: id, rate:rate, frequency:frequency}   
        send(url, data)
          .then(data => data.json())
          .then(response => console.log(response))
      // You can perform your registration API call here

      // after successful registration, load propertyEval.html homePage.html
      loadServiceManagementPage(homepageContent);
    });
  })
  .catch(error => console.log(error))
    
  }
  function frequencyService(id, frequency){
    const homepageContent = document.getElementsByClassName("homepage-content")[0];
    url = api + "/Service/frequencychange";
    data = {id: id, rate: 0, frequency:frequency}
    put(url, data)
    .then(response => console.log(response))
    .then(loadServiceManagementPage(homepageContent));
    
  }
  function cancelService(id){
    const homepageContent = document.getElementsByClassName("homepage-content")[0];
    url = api + "/Service/cancel";
    data = {id: id}
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
  
    //only need the button to go to rating page
    let rating = document.createElement('td');
    rating.innerText = `${userservice.rating}`;
  
    /*let rateAction = document.createElement('td');
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
    rateForm.append(ratebtn);*/

    //frequncy change button for window
    let frequencyAction = document.createElement('td');
    let frequencybtn = document.createElement('button');
    frequencybtn.innerText = "Frequncy Change";
    frequencybtn.className="frequency";
    frequencybtn.id=`${request.id}`; 
    
    frequencyAction.append(frequencybtn);

    let rateAction = document.createElement('td');
    let ratebtn = document.createElement('button');
    ratebtn.innerText = "Rate";
    ratebtn.className="rate";
    ratebtn.id=`${request.id}`; 
    
    rateAction.append(ratebtn);

     //cancellation button for window
     let cancelAction = document.createElement('td');
     let cancelbtn = document.createElement('button');
     cancelbtn.innerText = "Cancel";
     cancelbtn.className="cancel";
     cancelbtn.id=`${request.id}`; 
     
     cancelAction.append(cancelbtn);
  
    userServiceTableBodyRow.append(serviceName,serviceType,serviceDescription,serviceFrequeny,
      serviceProvider,serviceProviderEmail,status,rating, rateAction, frequencyAction, cancelAction);
      //allrequest += requestTableBodyRow;
    UserServiceTable.append(userServiceTableBodyRow);
  }

  function loadCancelServicePage(id, homepageContent) {
    // fetch register page html
    fetch("./Views/cancelService.html")
      .then(response => response.text())
      .then(data => {
        // update content div with register page
        content.innerHTML = data;
  
        const backBtn = document.getElementById('backServices');
  
        // add event listeners
        backBtn.addEventListener('click', loadServiceManagementPage(homepageContent))
  
        // select register form
        const cancelForm = document.getElementById('cancel-form');
  
        // add event listener to register form submit
        cancelForm.addEventListener('submit', (event) => {
          event.preventDefault();
  
          // perform registration action
          url = api + '/UserService/cancelrequest';
          data = {id: id }   
            send(url, data)
              .then(data => data.json())
              .then(response => console.log(response))
          // You can perform your registration API call here
  
          // after successful registration, load propertyEval.html homePage.html
          loadServiceManagementPage(homepageContent);
        });
      })
      .catch(error => console.log(error))
  }
  function loadChangeServicePage(id, homepageContent) {
    // fetch register page html
    fetch("./Views/frequencyChange.html")
      .then(response => response.text())
      .then(data => {
        // update content div with register page
        content.innerHTML = data;
  
        const backBtn = document.getElementById('backServices');
  
        // add event listeners
        backBtn.addEventListener('click', loadServiceManagementPage(homepageContent))
  
        // select register form
        const changeForm = document.getElementById('fchange-form');
  
        // add event listener to register form submit
        changeForm.addEventListener('submit', (event) => {
          event.preventDefault();

          const rate = document.querySelector('#times').value;
          const frequencies = document.querySelector('#frequencies').value;
          
          let frequency = rate + frequencies.toString();

  
          // perform registration action
          url = api + '/UserService/cancelrequest';
          data = {id: id, rate:rate, frequency:frequency }   
            send(url, data)
              .then(data => data.json())
              .then(response => console.log(response))
          // You can perform your registration API call here
  
          // after successful registration, load propertyEval.html homePage.html
          loadServiceManagementPage(homepageContent);
        });
      })
      .catch(error => console.log(error))
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
              const rateService = Array.from(document.getElementsByClassName("rate")); 
              rateService.forEach((key)=>{
                key.addEventListener('submit', (event) => {
                  event.preventDefault();   
  
                  rateUserService(key.id, homepageContent)});
              })
              const cancelUserService = Array.from(document.getElementsByClassName("frequency")); 
              cancelUserService.forEach((key)=>{
                key.addEventListener('submit', (event) => {
                  event.preventDefault();   

                  loadCancelServicePage(key.id, homepageContent)});
              })
              const changeService = Array.from(document.getElementsByClassName("cancel")); 
              changeService.forEach((key)=>{
                key.addEventListener('submit', (event) => {
                  event.preventDefault();   
                  loadChangeServicePage(key.id, homepageContent)});
              })
  
              /*const emailAdmin = document.getElementById("notifyAdmin");
              emailAdmin.addEventListener('click', function() {loadEmailPage(homepageContent)});*/
          })
          .catch(error => console.log(error));
  
      })   
      .catch(error => console.log(error));
      
  }