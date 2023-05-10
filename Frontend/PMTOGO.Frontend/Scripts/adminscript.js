function loadAdminPage(homepageContent) 
{ 
  // fetch request evaluation page html
  fetch('./Views/admin.html')
    .then(response => response.text())
    .then(data => {
      // Handle the response data
      homepageContent.innerHTML = data;

      const admincontent = document.querySelector("div.admin-content");
      const usersmng = document.getElementById('adminuserman');
      usersmng.addEventListener('click', () => {
        getUsersManagement(admincontent);
      });

      const usage = document.getElementById('adminusage');
      usage.addEventListener('click', () => {
        console.log("nothing yet");
        //getUsersManagement();
      });

    })   
    .catch(error => console.error(error));
    
}
//user management

/*function loadRateUserService(id, homepageContent){
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

      });
    })
    .catch(error => console.log(error))  
}*/
const createUsersTable = () =>{
  const users = document.querySelector("div.users");
  let tableHeaders = ["","Username","First Name", "Last Name", "Role", "IsActive?"];
  while (users.firstChild) users.removeChild(users.firstChild)
    let usersTable = document.createElement('table');
    usersTable.className="usersTable";
    usersTable.id="usersTable";

    let usersTableHead = document.createElement("thead");
    usersTableHead.className="usersTableHead";

    let usersTableHeaderRow= document.createElement('tr')
    usersTableHeaderRow.className= "usersHeaderRow";

    tableHeaders.forEach(header =>{
      let usersHeader = document.createElement('th');
      usersHeader.innerText = header;
      usersTableHeaderRow.append(usersHeader);
    })

    usersTableHead.append(usersTableHeaderRow);
    usersTable.append(usersTableHead);

    let usersTableBody = document.createElement('tbody');
    usersTableBody.className="usersTableBody"
    usersTable.append(usersTableBody);

    users.append(usersTable);
}

const appendUsers =(user, id) => {
  const UsersTable = document.querySelector(".usersTable");

  let usersTableBodyRow = document.createElement('tr');
  usersTableBodyRow.className = "usersTableBodyRow";
  usersTableBodyRow.id= String(id);

  let Action = document.createElement('td');
  var actionbox = document.createElement("INPUT");
  actionbox.setAttribute("type", "checkbox");
  actionbox.className="useraction-link";
  actionbox.value=`${user.username}`; 
  
  Action.append(actionbox);

  //add the data
  let userName = document.createElement('td');
  userName.innerText = `${user.username}`;

  let firstname  = document.createElement('td');
  firstname.innerText = `${user.firstName}`;

  let lastname  = document.createElement('td');
  lastname.innerText = `${user.lastName}`;

  let role = document.createElement('td');
  role.innerText = `${user.role}`;

  let active = document.createElement('td');
  active.innerText = `${user.isActive}`;


  usersTableBodyRow.append(Action,userName, firstname, lastname, role, active);
    //allrequest += requestTableBodyRow;
  UsersTable.append(usersTableBodyRow);
}
function getUsersManagement(admincontent){
  url = api + '/UserManagement/getusers';
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
      fetch('./Views/usersMng.html')
      .then(response => response.text())
      .then(data => {
      // Handle the response data
        admincontent.innerHTML = data;

        response.json().then(data => {
          createUsersTable();
          let id = 0;
          console.log(data);
          data.forEach((user) =>{
            appendUsers(user, id);
            id = id + 1;
          })
          const action = Array.from(document.getElementsByClassName("useraction-link")); 
          action.forEach((key)=>{
            key.addEventListener('onclick', function(){loadRateUserService(key.id, admincontent)});
          })

            /*const emailAdmin = document.getElementById("notifyAdmin");
            emailAdmin.addEventListener('click', function() {loadEmailPage(homepageContent)});*/
          })
          .catch(error => console.error(error))
      })   
      .catch(error => console.error(error));
    }
  })
  .catch(error => console.error(error));
  
}
