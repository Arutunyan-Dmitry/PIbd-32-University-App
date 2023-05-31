var navItems = [document.getElementById("index"),
document.getElementById("plan"),
document.getElementById("lessons"),
document.getElementById("reports")];

var navLine = document.getElementById("navLine");
var fL1 = document.getElementsByClassName("mid-solid-line");
var fL2 = document.getElementsByClassName("mid-normal-line");

for (let i = 0; i < navItems.length; i++) {
    navItems[i].classList.remove('active');
}
navItems[1].classList.add('active');

navItems[1].style.backgroundColor = "#464FEC";
navLine.style.backgroundColor = "#464FEC";
for (let i = 0; i < fL1.length; i++) {
    fL1[i].style.backgroundColor = "#464FEC";
}
for (let i = 0; i < fL2.length; i++) {
    fL2[i].style.backgroundColor = "#DDE8FE";
}