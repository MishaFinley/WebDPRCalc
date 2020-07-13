function parseCalculation(calc) {
    json = JSON.parse(calc.replace(/&quot;/g, '"'));
    console.log(json);
    var slider = document.getElementById("acSlider");
    var output = document.getElementById("acLabel");
    var hitChance = document.getElementById("hitChance");
    var critChance = document.getElementById("critChance");
    var dmg = document.getElementById("dmg");
    slider.oninput = function () {
        output.innerHTML = this.value;
        fragment = json.dPRFragments[this.value - 5];
        hitChance.innerHTML = fragment.hitChance;
        critChance.innerHTML = fragment.critChance;
        dmg.innerHTML = fragment.averageDamage;
    }
}
