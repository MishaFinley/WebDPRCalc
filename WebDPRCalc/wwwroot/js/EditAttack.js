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
class Die {
	constructor(sidesCount,rerollAtBelow,mimimunNumber) {
        this.sidesCount = sidesCount;
        this.rereollAtBelow = rerollAtBelow;
        this.mimimunNumber = mimimunNumber;
	}
}
class AttackRoll {
	constructor(numericalAddition,advantage,disadvantage,luckyDie,elvenAccuracy,critRangeCount,halflingLuck,diceAddition,rerollMiss) {
        this.numericalAddition = numericalAddition;
        this.advantage = advantage;
        this.disadvantage = disadvantage;
        this.luckyDie = luckyDie;
        this.elvenAccuracy = elvenAccuracy;
        this.critRangeCount = critRangeCount;
        this.halflingLuck = halflingLuck;
        this.diceAddition = diceAddition;
        this.rerollMiss = rerollMiss;
	}
}
class DamageRoll {
	constructor(dice, numericalAddition, resisted, additionalCritDice,rerollCountOfDice,rollUseHighest) {
        this.dice = dice;
        this.numericalAddition = numericalAddition;
        this.resisted = resisted;
        this.additionalCritDice = additionalCritDice;
        this.rerollCountOfDice = rerollCountOfDice;
        this.rollUseHighest = rollUseHighest;
	}
}
class Attack {
	constructor(name,attackRoll,damageRoll) {
        this.id = 0;
        this.name = name;
        this.attackRoll = attackRoll;
        this.damageRoll = damageRoll;
	}
}
function createAttackRequest(attack) {
	var url = "/api/user/attack/";
	sendRequest(attack, url, "POST", null);
}
function readAttackRequest(id) {
	var url = "/api/user/attack/"+id;
	sendRequest(null, url, "GET", readAttackResponse);
}
function readAttackResponse(attack) {
    //Display attack in UI
}
function readAttacksRequest() {
	var url = "/api/user/attack/";
	sendRequest(null, url, "GET", readAttacksResponse);
}
function readAttacksResponse(attacks) {
    //Display attacks in UI
}
function updateAttackRequest(attack) {
	var url = "/api/user/attack/";
	sendRequest(attack, url, "PATCH", null);
}
function deleteAttackRequest(id) {
	var url = "/api/user/attack/"+id;
	sendRequest(null, url, "DELETE", null);
}