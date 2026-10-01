const header = document.querySelector('[data-header]');
const navToggle = document.querySelector('[data-nav-toggle]');
const nav = document.querySelector('[data-nav]');

function updateHeader() {
  header?.classList.toggle('scrolled', window.scrollY > 18);
}

updateHeader();
window.addEventListener('scroll', updateHeader, { passive: true });

navToggle?.addEventListener('click', () => {
  const open = nav.classList.toggle('open');
  navToggle.setAttribute('aria-expanded', String(open));
});

nav?.querySelectorAll('a').forEach((link) => {
  link.addEventListener('click', () => {
    nav.classList.remove('open');
    navToggle?.setAttribute('aria-expanded', 'false');
  });
});

const tabs = [...document.querySelectorAll('[data-tab]')];
const panels = [...document.querySelectorAll('[data-panel]')];

function selectTab(name) {
  tabs.forEach((tab) => tab.setAttribute('aria-selected', String(tab.dataset.tab === name)));
  panels.forEach((panel) => { panel.hidden = panel.dataset.panel !== name; });
}

tabs.forEach((tab, index) => {
  tab.addEventListener('click', () => selectTab(tab.dataset.tab));
  tab.addEventListener('keydown', (event) => {
    if (!['ArrowLeft', 'ArrowRight'].includes(event.key)) return;
    event.preventDefault();
    const direction = event.key === 'ArrowRight' ? 1 : -1;
    const next = tabs[(index + direction + tabs.length) % tabs.length];
    next.focus();
    selectTab(next.dataset.tab);
  });
});

const observer = new IntersectionObserver((entries) => {
  entries.forEach((entry) => {
    if (entry.isIntersecting) {
      entry.target.classList.add('visible');
      observer.unobserve(entry.target);
    }
  });
}, { threshold: .12 });

document.querySelectorAll('.reveal').forEach((element) => observer.observe(element));
document.querySelectorAll('[data-year]').forEach((element) => { element.textContent = new Date().getFullYear(); });
