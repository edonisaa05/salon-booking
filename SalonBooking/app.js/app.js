/* ═══════════════════════════════════════════════════════
   GLAMOUR STUDIO — app.js
   Frontend logic për sistemin e rezervimeve
   Simulon API calls me localStorage + mock data
═══════════════════════════════════════════════════════ */
const API_URL = 'http://localhost:5131/api';
'use strict';

/* ─── MOCK DATABASE ─────────────────────────────────── */
const USERS = [
  { id: 1, email: 'user@salon.com',  password: 'user123',  name: 'Ana Krasniqi', role: 'user'  },
  { id: 2, email: 'admin@salon.com', password: 'admin123', name: 'Pronarja',      role: 'admin' },
];

const DEFAULT_SERVICES = [
  { id: 1, name: 'Prerje flokësh',    duration: 45, price: 15 },
  { id: 2, name: 'Ngjyrosje flokësh', duration: 90, price: 40 },
  { id: 3, name: 'Vetulla',           duration: 20, price: 8  },
  { id: 4, name: 'Qerpikë',           duration: 60, price: 25 },
  { id: 5, name: 'Manikyri',          duration: 40, price: 12 },
  { id: 6, name: 'Maskë fytyre',      duration: 30, price: 18 },
];

/* ─── STATE ─────────────────────────────────────────── */
let currentUser = null;
let nextBookingId = 100;

/* ─── HELPERS: LocalStorage ─────────────────────────── */
function getBookings() {
  try { return JSON.parse(localStorage.getItem('sb_bookings') || '[]'); }
  catch { return []; }
}

function saveBookings(arr) {
  localStorage.setItem('sb_bookings', JSON.stringify(arr));
}

function getServices() {
  try {
    const s = JSON.parse(localStorage.getItem('sb_services') || 'null');
    return s || DEFAULT_SERVICES;
  } catch { return DEFAULT_SERVICES; }
}

function saveServices(arr) {
  localStorage.setItem('sb_services', JSON.stringify(arr));
}

function getNextId() {
  const id = parseInt(localStorage.getItem('sb_next_id') || '100');
  localStorage.setItem('sb_next_id', id + 1);
  return id;
}

/* ─── INIT ───────────────────────────────────────────── */
document.addEventListener('DOMContentLoaded', () => {
  // Set minimum date for booking (today)
  const dateInput = document.getElementById('b-date');
  if (dateInput) {
    dateInput.min = new Date().toISOString().split('T')[0];
  }

  // Initialize services if first load
  if (!localStorage.getItem('sb_services')) {
    saveServices(DEFAULT_SERVICES);
  }

  // Check for persisted session
  const saved = sessionStorage.getItem('sb_user');
  if (saved) {
    currentUser = JSON.parse(saved);
    renderDashboard();
  }
});

/* ─── ROUTING / PAGE SWITCH ─────────────────────────── */
function showPage(pageId) {
  document.querySelectorAll('.page').forEach(p => {
    p.classList.remove('active');
    p.classList.add('hidden');
  });
  const page = document.getElementById(pageId);
  page.classList.remove('hidden');
  page.classList.add('active');
}

/* ─── AUTH ───────────────────────────────────────────── */
/* ─── AUTH TAB SWITCH ────────────────────────────────── */
function switchAuthTab(tab) {
  const isLogin = tab === 'login';
  document.getElementById('form-login').classList.toggle('hidden', !isLogin);
  document.getElementById('form-register').classList.toggle('hidden', isLogin);
  document.getElementById('tab-login').classList.toggle('active', isLogin);
  document.getElementById('tab-register').classList.toggle('active', !isLogin);
  // clear errors
  document.getElementById('login-error').classList.add('hidden');
  document.getElementById('register-error').classList.add('hidden');
  document.getElementById('register-success').classList.add('hidden');
}

/* ─── REGISTER ───────────────────────────────────────── */
function getRegisteredUsers() {
  try { return JSON.parse(localStorage.getItem('sb_users') || '[]'); }
  catch { return []; }
}

function saveRegisteredUsers(arr) {
  localStorage.setItem('sb_users', JSON.stringify(arr));
}


async function doRegister() {
  const name  = document.getElementById('reg-name').value.trim();
  const email = document.getElementById('reg-email').value.trim().toLowerCase();
  const pass1 = document.getElementById('reg-password').value;
  const pass2 = document.getElementById('reg-password2').value;
  const sucEl = document.getElementById('register-success');

  document.getElementById('register-error').classList.add('hidden');
  sucEl.classList.add('hidden');

  if (!name || !email || !pass1 || !pass2) {
    showRegisterError('Ju lutemi plotësoni të gjitha fushat.');
    return;
  }
  if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) {
    showRegisterError('Adresa email nuk është e vlefshme.');
    return;
  }
  if (pass1.length < 6) {
    showRegisterError('Fjalëkalimi duhet të ketë të paktën 6 karaktere.');
    return;
  }
  if (pass1 !== pass2) {
    showRegisterError('Fjalëkalimet nuk përputhen.');
    return;
  }

  try {
    const response = await fetch(`${API_URL}/users/register`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ name, email, password: pass1, role: 'user' })
    });

    if (response.status === 409) {
      showRegisterError('Ky email është i regjistruar tashmë.');
      return;
    }

    if (!response.ok) {
      const msg = await response.text();
      showRegisterError(msg || 'Ndodhi një gabim.');
      return;
    }

    // Pastro formen
    document.getElementById('reg-name').value      = '';
    document.getElementById('reg-email').value     = '';
    document.getElementById('reg-password').value  = '';
    document.getElementById('reg-password2').value = '';

    sucEl.classList.remove('hidden');
    setTimeout(() => {
      sucEl.classList.add('hidden');
      switchAuthTab('login');
      document.getElementById('login-email').value = email;
    }, 2000);

  } catch (err) {
    showRegisterError('Nuk u lidh me serverin. A është API aktive?');
  }
}

function showRegisterError(msg) {
  document.getElementById('register-error-msg').textContent = msg;
  document.getElementById('register-error').classList.remove('hidden');
}

function togglePassword(inputId, btn) {
  const input = document.getElementById(inputId);
  if (input.type === 'password') {
    input.type = 'text';
    btn.style.color = 'var(--rose)';
  } else {
    input.type = 'password';
    btn.style.color = '';
  }
}

async function doLogin() {
  const email    = document.getElementById('login-email').value.trim();
  const password = document.getElementById('login-password').value;

  if (!email || !password) {
    showLoginError('Ju lutemi plotësoni të gjitha fushat.');
    return;
  }

  try {
    const response = await fetch(`${API_URL}/users/login`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ email, password })
    });

    if (response.status === 401) {
      showLoginError('Email ose fjalëkalimi i gabuar. Provoni përsëri.');
      return;
    }

    if (!response.ok) {
      showLoginError('Ndodhi një gabim. Provoni përsëri.');
      return;
    }

    const user = await response.json();
    currentUser = user;
    sessionStorage.setItem('sb_user', JSON.stringify(user));
    renderDashboard();

  } catch (err) {
    showLoginError('Nuk u lidh me serverin. A është API aktive?');
  }
}

function showLoginError(msg) {
  const el = document.getElementById('login-error');
  document.getElementById('login-error-msg').textContent = msg;
  el.classList.remove('hidden');

  // Shake animation
  const card = document.querySelector('.login-card');
  card.style.animation = 'none';
  setTimeout(() => { card.style.animation = ''; }, 10);
}

function doLogout() {
  currentUser = null;
  sessionStorage.removeItem('sb_user');
  document.getElementById('login-email').value = '';
  document.getElementById('login-password').value = '';
  document.getElementById('login-error').classList.add('hidden');
  showPage('page-login');
  showToast('U çkyçët me sukses.');
}

function renderDashboard() {
  if (currentUser.role === 'admin') {
    showPage('page-admin');
    renderAdminBookings();
    renderServicesAdmin();
  } else {
    showPage('page-user');
    document.getElementById('user-display-name').textContent = currentUser.name;
    document.getElementById('user-avatar-initials').textContent = currentUser.name.charAt(0).toUpperCase();
    populateServicesDropdown();
    renderUserBookings();
    showUserSection('sec-rezervimet', document.querySelector('[data-target="sec-rezervimet"]'));
  }
}

/* ─── USER NAVIGATION ─────────────────────────────────── */
function showUserSection(sectionId, btn) {
  document.querySelectorAll('#page-user .dashboard-section').forEach(s => {
    s.classList.remove('active');
    s.classList.add('hidden');   // ensure it's hidden
    s.style.display = '';
  });
  const sec = document.getElementById(sectionId);
  sec.classList.remove('hidden');
  sec.classList.add('active');

  // Update nav tabs
  document.querySelectorAll('#page-user .nav-tab').forEach(t => t.classList.remove('active'));
  if (btn) btn.classList.add('active');

  // Re-render if switching to bookings
  if (sectionId === 'sec-rezervimet') renderUserBookings();
  if (sectionId === 'sec-rezervo') {
    document.getElementById('booking-error').classList.add('hidden');
    document.getElementById('booking-success').classList.add('hidden');
  }
}

/* ─── ADMIN NAVIGATION ─────────────────────────────────── */
function showAdminSection(sectionId, btn) {
  document.querySelectorAll('#page-admin .dashboard-section').forEach(s => {
    s.classList.remove('active');
    s.classList.add('hidden');
    s.style.display = '';
  });
  const sec = document.getElementById(sectionId);
  sec.classList.remove('hidden');
  sec.classList.add('active');

  document.querySelectorAll('#page-admin .nav-tab').forEach(t => t.classList.remove('active'));
  if (btn) btn.classList.add('active');

  if (sectionId === 'sec-terminet') renderAdminBookings();
  if (sectionId === 'sec-sherbimet') renderServicesAdmin();
}

/* ─── USER: RENDER BOOKINGS ─────────────────────────────── */
function renderUserBookings() {
  const bookings = getBookings().filter(b => b.userId === currentUser.id);
  const container = document.getElementById('user-bookings-list');
  const emptyEl   = document.getElementById('user-no-bookings');

  if (bookings.length === 0) {
    container.innerHTML = '';
    emptyEl.classList.remove('hidden');
    return;
  }

  emptyEl.classList.add('hidden');

  // Sort newest first
  bookings.sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt));

  container.innerHTML = bookings.map(b => {
    const statusClass = b.status === 'Confirmed' ? 'status-confirmed'
                      : b.status === 'Cancelled' ? 'status-cancelled'
                      : 'status-pending';
    const statusLabel = b.status === 'Confirmed' ? '✓ Konfirmuar'
                      : b.status === 'Cancelled' ? '✕ Anuluar'
                      : '○ Në pritje';
    const canCancel = b.status === 'Confirmed';

    return `
      <div class="booking-card" id="bcard-${b.id}">
        <div class="booking-card-header">
          <span class="booking-service">${escHtml(b.service)}</span>
          <span class="status-badge ${statusClass}">${statusLabel}</span>
        </div>
        <div class="booking-card-body">
          <span>📅 ${formatDate(b.date)}</span>
          <span>🕐 ${escHtml(b.time)}</span>
          <span>🔖 ID: #${b.id}</span>
        </div>
        <div class="booking-card-footer">
          ${canCancel
            ? `<button class="btn-danger" onclick="cancelUserBooking(${b.id})">Anulo terminit</button>`
            : `<span style="font-size:.8rem;color:var(--ink-muted)">Statusi: ${escHtml(b.status)}</span>`
          }
        </div>
      </div>`;
  }).join('');
}

/* ─── USER: CANCEL BOOKING ─────────────────────────────── */
function cancelUserBooking(id) {
  if (!confirm('Jeni të sigurt që doni ta anuloni këtë termin?')) return;

  const bookings = getBookings();
  const idx = bookings.findIndex(b => b.id === id && b.userId === currentUser.id);

  if (idx === -1) { showToast('Termini nuk u gjet.'); return; }

  bookings[idx].status = 'Cancelled';
  saveBookings(bookings);
  renderUserBookings();
  showToast('Termini u anulua me sukses.');
}

/* ─── USER: SERVICES DROPDOWN ─────────────────────────── */
function populateServicesDropdown() {
  const select = document.getElementById('b-service');
  const services = getServices();
  select.innerHTML = '<option value="">— Zgjidhni shërbimin —</option>'
    + services.map(s =>
        `<option value="${escHtml(s.name)}">${escHtml(s.name)} (${s.duration} min · €${s.price})</option>`
      ).join('');
}

/* ─── USER: SUBMIT BOOKING ─────────────────────────────── */
function submitBooking() {
  const service = document.getElementById('b-service').value;
  const date    = document.getElementById('b-date').value;
  const time    = document.getElementById('b-time').value;
  const errorEl = document.getElementById('booking-error');
  const successEl = document.getElementById('booking-success');

  errorEl.classList.add('hidden');
  successEl.classList.add('hidden');

  // Validim
  if (!service || !date || !time) {
    showBookingError('Ju lutemi plotësoni të gjitha fushat.');
    return;
  }

  // Kontroll konflikti orarit
  const bookings = getBookings();
  const conflict = bookings.find(b =>
    b.date === date && b.time === time && b.status !== 'Cancelled'
  );

  if (conflict) {
    showBookingError('Orari është i zënë! Ju lutemi zgjidhni një orë tjetër.');
    return;
  }

  // Krijo rezervimin
  const newBooking = {
    id:        getNextId(),
    userId:    currentUser.id,
    userName:  currentUser.name,
    service,
    date,
    time,
    status:    'Confirmed',
    createdAt: new Date().toISOString(),
  };

  bookings.push(newBooking);
  saveBookings(bookings);

  // Reset form
  document.getElementById('b-service').value = '';
  document.getElementById('b-date').value    = '';
  document.getElementById('b-time').value    = '';

  successEl.classList.remove('hidden');
  setTimeout(() => successEl.classList.add('hidden'), 4000);
  showToast('✓ Rezervimi u konfirmua!');
}

function showBookingError(msg) {
  document.getElementById('booking-error-msg').textContent = msg;
  document.getElementById('booking-error').classList.remove('hidden');
}

/* ─── ADMIN: RENDER BOOKINGS ─────────────────────────────── */
function renderAdminBookings(filter = '') {
  let bookings = getBookings();

  if (filter) {
    bookings = bookings.filter(b =>
      b.userName.toLowerCase().includes(filter.toLowerCase())
    );
  }

  const tbody  = document.getElementById('admin-bookings-tbody');
  const emptyEl = document.getElementById('admin-no-bookings');

  if (bookings.length === 0) {
    tbody.innerHTML = '';
    emptyEl.classList.remove('hidden');
    return;
  }

  emptyEl.classList.add('hidden');

  // Sort newest first
  bookings.sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt));

  tbody.innerHTML = bookings.map(b => {
    const statusClass = b.status === 'Confirmed' ? 'status-confirmed'
                      : b.status === 'Cancelled' ? 'status-cancelled'
                      : 'status-pending';
    const statusLabel = b.status === 'Confirmed' ? 'Konfirmuar'
                      : b.status === 'Cancelled' ? 'Anuluar'
                      : 'Në pritje';
    return `
      <tr id="arow-${b.id}">
        <td>#${b.id}</td>
        <td>${escHtml(b.userName)}</td>
        <td>${escHtml(b.service)}</td>
        <td>${formatDate(b.date)}</td>
        <td>${escHtml(b.time)}</td>
        <td><span class="status-badge ${statusClass}">${statusLabel}</span></td>
        <td>
          <div class="table-actions">
            <button class="btn-icon view" title="Shiko detajet" onclick="openDetailModal(${b.id})">👁</button>
            <button class="btn-icon" title="Redakto" onclick="openEditModal(${b.id})">✏️</button>
            <button class="btn-icon delete" title="Fshi" onclick="adminDeleteBooking(${b.id})">🗑</button>
          </div>
        </td>
      </tr>`;
  }).join('');
}

function filterAdminBookings(val) {
  renderAdminBookings(val);
}

/* ─── ADMIN: DELETE BOOKING ─────────────────────────────── */
function adminDeleteBooking(id) {
  if (!confirm(`A jeni të sigurt që doni të fshini termin #${id}?`)) return;

  const bookings = getBookings().filter(b => b.id !== id);
  saveBookings(bookings);
  renderAdminBookings();
  showToast(`Termini #${id} u fshi.`);
}

/* ─── ADMIN: DETAIL MODAL ─────────────────────────────────── */
function openDetailModal(id) {
  const b = getBookings().find(b => b.id === id);
  if (!b) return;

  const statusLabel = b.status === 'Confirmed' ? 'Konfirmuar'
                    : b.status === 'Cancelled' ? 'Anuluar'
                    : 'Në pritje';

  document.getElementById('modal-title').textContent = `Termini #${b.id}`;
  document.getElementById('modal-body').innerHTML = `
    <div class="modal-row"><span class="label">Klienti</span><span class="value">${escHtml(b.userName)}</span></div>
    <div class="modal-row"><span class="label">Shërbimi</span><span class="value">${escHtml(b.service)}</span></div>
    <div class="modal-row"><span class="label">Data</span><span class="value">${formatDate(b.date)}</span></div>
    <div class="modal-row"><span class="label">Ora</span><span class="value">${escHtml(b.time)}</span></div>
    <div class="modal-row"><span class="label">Statusi</span><span class="value">${statusLabel}</span></div>
    <div class="modal-row"><span class="label">Krijuar</span><span class="value">${formatDateTime(b.createdAt)}</span></div>
  `;
  document.getElementById('modal-actions').innerHTML = `
    <button class="btn-secondary" onclick="closeModal()">Mbyll</button>
    <button class="btn-primary" onclick="openEditModal(${b.id})">Redakto</button>
  `;
  openModal();
}

/* ─── ADMIN: EDIT MODAL ─────────────────────────────────── */
function openEditModal(id) {
  const b = getBookings().find(b => b.id === id);
  if (!b) return;

  const services = getServices();
  const serviceOptions = services.map(s =>
    `<option value="${escHtml(s.name)}" ${b.service === s.name ? 'selected' : ''}>${escHtml(s.name)}</option>`
  ).join('');

  const timeOptions = ['09:00','09:30','10:00','10:30','11:00','11:30','12:00','12:30',
    '13:00','13:30','14:00','14:30','15:00','15:30','16:00','16:30','17:00','17:30']
    .map(t => `<option value="${t}" ${b.time === t ? 'selected' : ''}>${t}</option>`).join('');

  document.getElementById('modal-title').textContent = `Redakto termin #${b.id}`;
  document.getElementById('modal-body').innerHTML = `
    <div class="modal-edit-group">
      <label>Shërbimi</label>
      <select id="edit-service">${serviceOptions}</select>
    </div>
    <div class="modal-edit-group">
      <label>Data</label>
      <input type="date" id="edit-date" value="${b.date}" />
    </div>
    <div class="modal-edit-group">
      <label>Ora</label>
      <select id="edit-time">${timeOptions}</select>
    </div>
    <div class="modal-edit-group">
      <label>Statusi</label>
      <select id="edit-status">
        <option value="Confirmed" ${b.status === 'Confirmed' ? 'selected' : ''}>Konfirmuar</option>
        <option value="Cancelled" ${b.status === 'Cancelled' ? 'selected' : ''}>Anuluar</option>
        <option value="Pending"   ${b.status === 'Pending'   ? 'selected' : ''}>Në pritje</option>
      </select>
    </div>
  `;
  document.getElementById('modal-actions').innerHTML = `
    <button class="btn-secondary" onclick="closeModal()">Anulo</button>
    <button class="btn-primary"   onclick="saveEditBooking(${b.id})">Ruaj ndryshimet</button>
  `;
  openModal();
}

function saveEditBooking(id) {
  const service = document.getElementById('edit-service').value;
  const date    = document.getElementById('edit-date').value;
  const time    = document.getElementById('edit-time').value;
  const status  = document.getElementById('edit-status').value;

  if (!date) { showToast('Ju lutemi plotësoni datën.'); return; }

  // Conflict check (exclude current booking)
  const bookings = getBookings();
  const conflict = bookings.find(b =>
    b.id !== id && b.date === date && b.time === time && b.status !== 'Cancelled'
  );
  if (conflict) { showToast('Orari është i zënë! Zgjidhni orë tjetër.'); return; }

  const idx = bookings.findIndex(b => b.id === id);
  if (idx === -1) return;

  bookings[idx] = { ...bookings[idx], service, date, time, status };
  saveBookings(bookings);
  closeModal();
  renderAdminBookings();
  showToast(`Termini #${id} u përditësua.`);
}

/* ─── ADMIN: SERVICES ─────────────────────────────────── */
function renderServicesAdmin() {
  const services = getServices();
  const container = document.getElementById('services-list');

  if (services.length === 0) {
    container.innerHTML = '<p style="color:var(--ink-muted);font-size:.9rem">Nuk ka shërbime të regjistruara.</p>';
    return;
  }

  container.innerHTML = services.map(s => `
    <div class="service-chip" id="schip-${s.id}">
      <div class="service-chip-info">
        <span class="service-chip-name">${escHtml(s.name)}</span>
        <span class="service-chip-meta">${s.duration} min · €${s.price}</span>
      </div>
      <button class="chip-delete" onclick="deleteService(${s.id})" title="Fshi shërbimin">✕</button>
    </div>
  `).join('');

  // Sync user dropdown if on user page
  if (currentUser && currentUser.role === 'user') populateServicesDropdown();
}

function addService() {
  const name     = document.getElementById('s-name').value.trim();
  const duration = parseInt(document.getElementById('s-duration').value);
  const price    = parseFloat(document.getElementById('s-price').value);
  const successEl = document.getElementById('service-success');
  const errorEl   = document.getElementById('service-error');

  successEl.classList.add('hidden');
  errorEl.classList.add('hidden');

  if (!name) {
    document.getElementById('service-error-msg').textContent = 'Emri i shërbimit është i detyrueshëm.';
    errorEl.classList.remove('hidden');
    return;
  }

  if (isNaN(duration) || duration < 15) {
    document.getElementById('service-error-msg').textContent = 'Kohëzgjatja duhet të jetë të paktën 15 minuta.';
    errorEl.classList.remove('hidden');
    return;
  }

  if (isNaN(price) || price < 0) {
    document.getElementById('service-error-msg').textContent = 'Çmimi duhet të jetë një vlerë pozitive.';
    errorEl.classList.remove('hidden');
    return;
  }

  const services = getServices();
  const duplicate = services.find(s => s.name.toLowerCase() === name.toLowerCase());
  if (duplicate) {
    document.getElementById('service-error-msg').textContent = 'Ky shërbim ekziston tashmë.';
    errorEl.classList.remove('hidden');
    return;
  }

  const newService = {
    id:       Date.now(),
    name,
    duration,
    price:    parseFloat(price.toFixed(2)),
  };

  services.push(newService);
  saveServices(services);

  // Clear form
  document.getElementById('s-name').value     = '';
  document.getElementById('s-duration').value = '';
  document.getElementById('s-price').value    = '';

  renderServicesAdmin();
  successEl.classList.remove('hidden');
  setTimeout(() => successEl.classList.add('hidden'), 3000);
  showToast(`Shërbimi "${name}" u shtua.`);
}

function deleteService(id) {
  if (!confirm('Jeni të sigurt që doni ta fshini këtë shërbim?')) return;

  const services = getServices().filter(s => s.id !== id);
  saveServices(services);
  renderServicesAdmin();
  showToast('Shërbimi u fshi.');
}

/* ─── MODAL HELPERS ─────────────────────────────────── */
function openModal() {
  const overlay = document.getElementById('modal-overlay');
  overlay.classList.remove('hidden');
  document.body.style.overflow = 'hidden';
}

function closeModal() {
  document.getElementById('modal-overlay').classList.add('hidden');
  document.body.style.overflow = '';
}

// Close on Escape
document.addEventListener('keydown', e => {
  if (e.key === 'Escape') closeModal();
});

/* ─── TOAST ─────────────────────────────────────────── */
let toastTimer = null;

function showToast(msg, duration = 3000) {
  const el = document.getElementById('toast');
  el.textContent = msg;
  el.classList.remove('hidden');

  if (toastTimer) clearTimeout(toastTimer);
  toastTimer = setTimeout(() => el.classList.add('hidden'), duration);
}

/* ─── UTILITIES ─────────────────────────────────────── */
function escHtml(str) {
  if (typeof str !== 'string') return '';
  return str
    .replace(/&/g, '&amp;')
    .replace(/</g, '&lt;')
    .replace(/>/g, '&gt;')
    .replace(/"/g, '&quot;')
    .replace(/'/g, '&#39;');
}

function formatDate(dateStr) {
  if (!dateStr) return '—';
  try {
    const [y, m, d] = dateStr.split('-');
    return `${d}.${m}.${y}`;
  } catch { return dateStr; }
}

function formatDateTime(isoStr) {
  if (!isoStr) return '—';
  try {
    const d = new Date(isoStr);
    return d.toLocaleDateString('sq-AL', {
      day: '2-digit', month: '2-digit', year: 'numeric',
      hour: '2-digit', minute: '2-digit',
    });
  } catch { return isoStr; }
}