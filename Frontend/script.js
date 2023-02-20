
var role = "";
function UserRole(){
   
    if(document.getElementById("SP").checked) {
    role = "Service Provider";
    }
    if(document.getElementById("PM").checked) {
    role = "Property Manager";
    }
     return role;
}
role = UserRole(role);
function addUser(firstName, lastName, email, username, password, role){
    fetch('')
}