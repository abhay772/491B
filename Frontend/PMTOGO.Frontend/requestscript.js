const createRequestsTable = () =>{
  const requests = document.querySelector("div.requests");
  let tableHeaders = ["Request ID","Request Type", "Service Name", "Service Type",  "Service Description", 
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

  let requestType = document.createElement('td');
  requestType.innerText = `${request.requestType}`;

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
  acceptbtn.name = `${request.requestType}`;
  acceptbtn.value = `${request.serviceFrequency}`;
  acceptbtn.innerText = "Accept";

  let declineAction = document.createElement('td');
  let declinebtn = document.createElement('button');
  declinebtn.innerText = "Decline";
  declinebtn.className="decline";
  declinebtn.id=`${request.id}`; 
  
  acceptAction.append(acceptbtn);
  declineAction.append(declinebtn);

  requestTableBodyRow.append(requestId,requestType,serviceName,serviceType,serviceDescription,serviceFrequeny,
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
function acceptFrequencyRequest(requestid, frequency){
  const homepageContent = document.getElementsByClassName("homepage-content")[0];
  url = api + "/ServiceRequest/frequencychange";
  data = {id: requestid, frequency: frequency}
  send(url, data)
  .then(data => data.json())
  .then(response => console.log(response))
  .then(loadRequestManagementPage(homepageContent));
  
}
function acceptCancelRequest(requestid){
  const homepageContent = document.getElementsByClassName("homepage-content")[0];
  url = api + "/ServiceRequest/cancel";
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
            key.addEventListener('click', function() { 
              if (key.name == "New Request")
                {acceptRequest(key.id)}
              if (key.name == "Cancellation")
              {acceptCancelRequest(key.id)}
              else{acceptFrequencyRequest(key.id, key.value)}
            })
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