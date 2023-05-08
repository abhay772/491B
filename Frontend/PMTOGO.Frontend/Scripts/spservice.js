const createSPServiceTable = () =>{
    const spservices = document.querySelector("div.spservices");
    let tableHeaders = ["Service Name", "Service Type",  "Service Description", 
   "Service Provider Name", "Service Provider Email", "Price ($)", "Delete Service?"];
      while (spservices.firstChild) spservices.removeChild(spservices.firstChild)
      let SPServiceTable = document.createElement('table');
      SPServiceTable.className="SPServiceTable";
      SPServiceTable.id="SPServiceTable";
  
      let SPServiceTableHead = document.createElement("thead");
      SPServiceTableHead.className="SPServiceTableHead";
  
      let SPServiceTableHeaderRow= document.createElement('tr')
      SPServiceTableHeaderRow.className= "SPServiceHeaderRow";
  
      tableHeaders.forEach(header =>{
        let SPServiceHeader = document.createElement('th');
        SPServiceHeader.innerText = header;
        SPServiceTableHeaderRow.append(SPServiceHeader);
      })
  
      SPServiceTableHead.append(SPServiceTableHeaderRow);
      SPServiceTable.append(SPServiceTableHead);
  
      let SPServiceTableBody = document.createElement('tbody');
      SPServiceTableBody.className="SPServiceTableBody"
      SPServiceTable.append(SPServiceTableBody);
  
      spservices.append(SPServiceTable);
  }
  
  const appendSPServices =(service, id) => {
    const SPServiceTable = document.querySelector(".SPServiceTable");
    
    let SPServiceTableBodyRow = document.createElement('tr');
    SPServiceTableBodyRow.className = "SPServiceTableBodyRow";
    SPServiceTableBodyRow.id= String(id);
  
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
  
    let delAction = document.createElement('td');
    let delbtn = document.createElement('button');
    delbtn.innerText = "Delete Service";
    delbtn.className="del-link";
    delbtn.id=`${service.id}`;
  
    delAction.append(delbtn);
  
    SPServiceTableBodyRow.append(serviceName,serviceType,serviceDescription,
      serviceProvider,serviceProviderEmail,price, delAction);
  
    SPServiceTable.append(SPServiceTableBodyRow);
  }
  function loadAddService(homepageContent){
      fetch("./Views/newspservice.html")
      .then(response => response.text())
      .then(data => {
        // update content div with rate user page
        homepageContent.innerHTML = data;
  
        const backBtn = document.getElementById('BacktoServices');
        backBtn.addEventListener('click', loadHomePage);
  
        const spserviceForm = document.getElementById('newspservice-form');
        spserviceForm.addEventListener('submit', (event) => {
          event.preventDefault();
  
          const name = document.querySelector('#name').value;
          const type = document.querySelector('#type').value;
          const description  = document.querySelector('#description').value;
          const price  = document.querySelector('#price').value;
  
          url = api + "/UserService/addspservice";
          data = {serviceName: name, serviceType:type, serviceDescription:description, servicePrice:price };
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
              loadServicePage(homepageContent)
            }
          })
          .catch(error => console.log(error));
      
        });
      })
      .catch(error => console.log(error)) 
  }
  
  function loadDeleteService(id, homepageContent){
    fetch("./Views/delSPService.html")
      .then(response => response.text())
      .then(data => {
        // update content div with rate user page
        homepageContent.innerHTML = data;
  
        const backBtn = document.getElementById('BacktoServices');
        backBtn.addEventListener('click', loadHomePage);
  
        const delForm = document.getElementById('del-form');
        delForm.addEventListener('submit', (event) => {
          event.preventDefault();
  
          url = api + '/UserService/delspservice';
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
              loadServicePage(homepageContent);
            }
          })
          .catch(error => console.log(error));
        })
      })
      
  }
  function getSPService(){
    url = api + '/UserService/getspservice';
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
          createSPServiceTable();
          let id = 0;
          data.forEach((userservice) =>{
            appendSPServices(userservice, id);
            id = id + 1;
          })
          const homepageContent = document.getElementsByClassName("homepage-content")[0];
          const delService = Array.from(document.getElementsByClassName("del-link")); 
          delService.forEach((key)=>{
            key.addEventListener('click', function(){loadDeleteService(key.id, homepageContent)});
          })
          const AddService = document.getElementById("addservice");
            AddService.addEventListener('click', function() {loadAddService(homepageContent)});
  
            /*const emailAdmin = document.getElementById("notifyAdmin");
            emailAdmin.addEventListener('click', function() {loadEmailPage(homepageContent)});*/
          })
          .catch(error => console.error(error))
        }
    })
    .catch(error => console.error(error));
    
  }
  
  //fucntion to load service Management page
  function loadServicePage(homepageContent) { 
    // fetch request evaluation page html
    fetch('./Views/spservices.html')
      .then(response => response.text())
      .then(data => {
        // Handle the response data
        homepageContent.innerHTML = data;

        getSPService();

  
      })   
      .catch(error => console.error(error));
      
  } 