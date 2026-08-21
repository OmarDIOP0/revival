// Vanilla JS only. The site must stay fully usable without this file.

// Close the mobile nav disclosure after a link is activated.
document.querySelectorAll(".nav-toggle").forEach(function (toggle) {
  toggle.querySelectorAll("a").forEach(function (link) {
    link.addEventListener("click", function () {
      toggle.removeAttribute("open");
    });
  });
});

// Reveal-on-scroll — purely decorative, content is already visible without it
// (see .js .reveal in base.css). One-time reveal, no re-triggering.
// A safety timeout force-reveals everything if IntersectionObserver never
// fires for some reason, so content can never get stuck invisible.
(function () {
  var revealTargets = document.querySelectorAll(".reveal");
  if (!revealTargets.length) {
    return;
  }

  var revealAll = function () {
    revealTargets.forEach(function (el) {
      el.classList.add("is-visible");
    });
  };

  if (!("IntersectionObserver" in window)) {
    revealAll();
    return;
  }

  var revealObserver = new IntersectionObserver(
    function (entries) {
      entries.forEach(function (entry) {
        if (entry.isIntersecting) {
          entry.target.classList.add("is-visible");
          revealObserver.unobserve(entry.target);
        }
      });
    },
    { threshold: 0.2, rootMargin: "0px 0px -60px 0px" }
  );
  revealTargets.forEach(function (el) {
    revealObserver.observe(el);
  });

  window.setTimeout(revealAll, 2500);
})();

// Contact page: the map only loads once the visitor asks for it.
// Without JS, the button just opens Google Maps in a new tab instead.
var mapTrigger = document.getElementById("map-trigger");
if (mapTrigger) {
  mapTrigger.addEventListener("click", function (event) {
    event.preventDefault();
    var container = document.getElementById("map-container");
    if (!container) {
      return;
    }
    var iframe = document.createElement("iframe");
    iframe.src = "https://www.google.com/maps?q=Point+E+Rue+A+x+9+Dakar+Senegal&output=embed";
    iframe.loading = "lazy";
    iframe.title = mapTrigger.textContent.trim();
    iframe.height = "360";
    container.replaceChildren(iframe);
    container.hidden = false;
    mapTrigger.hidden = true;
  });
}
