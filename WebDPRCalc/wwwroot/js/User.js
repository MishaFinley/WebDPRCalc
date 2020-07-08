//TODO, set for authorization
var username;
var password;

async function sendRequest(object, requestPath, requestType, result) {
	var xmlHttp = new XMLHttpRequest();
    if (typeof username === 'string' && typeof password === 'string' ) {
        xmlHttp.open(requestType, requestPath, true, username, password);
        xmlHttp.withCredentials = true;
    } else {
        xmlHttp.open(requestType, requestPath, true);
        
    }
	xmlHttp.setRequestHeader('Content-type', 'application/json');
	xmlHttp.onreadystatechange = function () {
		if (this.readyState === XMLHttpRequest.DONE && typeof result === "function") {
			result(JSON.parse(this.responseText));
		}
	};
	if (object) {
		xmlHttp.send(JSON.stringify(object));
	} else {
		xmlHttp.send();
	}
}
class User {
	constructor(username, password) {
        this.username = username;
        this.password = password;
        this.attacks = null;
	}
}
function createUserRequest(user) {
	var url = "/api/user";
	sendRequest(user, url, "POST", null);
}
function readUserRequest() {
	var url = "/api/user";
	sendRequest(null, url, "GET", readUserResponse);
}
function readUserResponse(user) {
    //Display user in UI
}
function updateUserRequest(user) {
	var url = "/api/user";
	sendRequest(user, url, "PATCH", null);
}
function deleteUserRequest() {
	var url = "/api/user";
	sendRequest(null, url, "DELETE", null);
}