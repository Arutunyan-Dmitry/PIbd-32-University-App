var navItems = [document.getElementById("index"),
document.getElementById("plan"),
document.getElementById("lessons"),
document.getElementById("reports")];

for (let i = 0; i < navItems.length; i++) {
    navItems[i].classList.remove('active');
}
navItems[0].classList.add('active');