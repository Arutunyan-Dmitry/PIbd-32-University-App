var navLine = document.getElementById("navLine");
var fL1 = document.getElementsByClassName("mid-solid-line");
var fL2 = document.getElementsByClassName("mid-normal-line");

navLine.style.backgroundColor = "#464FEC";
for (let i = 0; i < fL1.length; i++) {
    fL1[i].style.backgroundColor = "#464FEC";
}
for (let i = 0; i < fL2.length; i++) {
    fL2[i].style.backgroundColor = "#DDE8FE";
}