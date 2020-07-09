//TODO, set for authorization
var username;
var password;

async function sendRequest(object, requestPath, requestType, result) {
    var xmlHttp = new XMLHttpRequest();
    if (typeof username === 'string' && typeof password === 'string') {
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
    constructor(sidesCount, rerollAtBelow, mimimunNumber) {
        this.sidesCount = sidesCount;
        this.rereollAtBelow = rerollAtBelow;
        this.mimimunNumber = mimimunNumber;
    }
}
class AttackRoll {
    constructor(numericalAddition, advantage, disadvantage, luckyDie, elvenAccuracy, critRangeCount, halflingLuck, diceAddition, rerollMiss) {
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
    constructor(dice, numericalAddition, resisted, additionalCritDice, rerollCountOfDice, rollUseHighest) {
        this.dice = dice;
        this.numericalAddition = numericalAddition;
        this.resisted = resisted;
        this.additionalCritDice = additionalCritDice;
        this.rerollCountOfDice = rerollCountOfDice;
        this.rollUseHighest = rollUseHighest;
    }
}
class Attack {
    constructor(name, attackRoll, damageRoll) {
        this.id = 0;
        this.name = name;
        this.attackRoll = attackRoll;
        this.damageRoll = damageRoll;
    }
}
function viewAttackResultByIdRequest(id) {
    var url = "/api/user/attack/calculation/" + id;
    sendRequest(null, url, "POST", viewAttackResultResponse);
}
function viewAttackResultRequest(attack) {
    var url = "/api/user/attack/calculation/";
    sendRequest(attack, url, "POST", viewAttackResultResponse);
}
function viewAttackResultResponse(result) {
    //Display result in UI
}
