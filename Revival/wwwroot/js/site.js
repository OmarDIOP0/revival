// Vanilla JS only. The site must stay fully usable without this file.

// Close the mobile nav disclosure after a link is activated.
document.querySelectorAll(".nav-toggle").forEach(function (toggle) {
  toggle.querySelectorAll("a").forEach(function (link) {
    link.addEventListener("click", function () {
      toggle.removeAttribute("open");
    });
  });
});
