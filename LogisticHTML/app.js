const LS_USERS = 'lf_users_v1';
const LS_ORDERS = 'lf_orders_v1';
const LS_SESSION = 'lf_session_v1';

let loginModal, registerModal, orderModal, profileModal;

document.addEventListener('DOMContentLoaded', () => {
  loginModal = new bootstrap.Modal(document.getElementById('loginModal'));
  registerModal = new bootstrap.Modal(document.getElementById('registerModal'));
  orderModal = new bootstrap.Modal(document.getElementById('orderModal'));
  profileModal = new bootstrap.Modal(document.getElementById('profileModal'));

  seedIfEmpty();
  renderNavUser();
});

function seedIfEmpty(){
  if(!localStorage.getItem(LS_USERS)){
    const demoUsers = [
      { username: 'staff1', password: '123', name: 'Nhân viên A', role: 'staff' },
      { username: 'user1', password: '123', name: 'Khách hàng 1', role: 'customer' }
    ];
    localStorage.setItem(LS_USERS, JSON.stringify(demoUsers));
  }
  if(!localStorage.getItem(LS_ORDERS)){
    localStorage.setItem(LS_ORDERS, JSON.stringify([]));
  }
}

function getUsers(){ return JSON.parse(localStorage.getItem(LS_USERS) || '[]'); }
function saveUsers(list){ localStorage.setItem(LS_USERS, JSON.stringify(list)); }

function getOrders(){ return JSON.parse(localStorage.getItem(LS_ORDERS) || '[]'); }
function saveOrders(list){ localStorage.setItem(LS_ORDERS, JSON.stringify(list)); }

function getSession(){ return JSON.parse(sessionStorage.getItem(LS_SESSION) || 'null'); }
function setSession(obj){ sessionStorage.setItem(LS_SESSION, JSON.stringify(obj)); renderNavUser(); }
function clearSession(){ sessionStorage.removeItem(LS_SESSION); renderNavUser(); }

function openLoginModal(){ document.getElementById('loginForm').reset(); loginModal.show(); }
function openRegisterModal(){ document.getElementById('registerForm').reset(); registerModal.show(); }
function openOrderModal(){ document.getElementById('orderForm').reset(); orderModal.show(); }
function openProfileModal(){
  const s = getSession();
  if(!s) return;
  document.getElementById('profileContent').innerHTML = `<p>Tài khoản: ${s.username}</p><p>Tên: ${s.name}</p><p>Vai trò: ${s.role}</p>`;
  profileModal.show();
}

function renderNavUser(){
  const s = getSession();
  const dashArea = document.getElementById('dashboard-area');

  if(s){
    dashArea.style.display = 'block';
    document.getElementById('dash-welcome').innerText = 'Xin chào, ' + s.name;
    document.getElementById('dash-role').innerText = 'Vai trò: ' + s.role;

    // ẩn/hiện menu theo vai trò
    document.getElementById('li-staff-orders').style.display = s.role==='staff' ? 'block' : 'none';
    document.getElementById('li-users').style.display = s.role==='staff' ? 'block' : 'none';
    document.getElementById('li-myorders').style.display = s.role==='customer' ? 'block' : 'none';

    renderMyOrders(); // load mặc định
  } else {
    dashArea.style.display = 'none';
  }
}


function handleLogin(e){
  e.preventDefault();
  const f = e.target;
  const u = f.username.value.trim(), p = f.password.value.trim();
  const users = getUsers();
  const found = users.find(x => x.username===u && x.password===p);
  if(found){ setSession(found); loginModal.hide(); showToast('Đăng nhập thành công','success'); }
  else showToast('Sai thông tin','danger');
}

function handleRegister(e){
  e.preventDefault();
  const f = e.target;
  const u = f.username.value.trim(), p = f.password.value.trim(), n = f.name.value.trim();
  const users = getUsers();
  if(users.find(x => x.username===u)) return showToast('Tên đã tồn tại','warning');
  users.push({ username:u, password:p, name:n, role:'customer' });
  saveUsers(users);
  registerModal.hide();
  showToast('Đăng ký thành công','success');
}

function handleCreateOrder(e){
  e.preventDefault();
  const f = e.target;
  const s = getSession();
  if(!s) return;
  const orders = getOrders();
  orders.push({ id: Date.now(), receiver:f.receiver.value, from:f.from.value, to:f.to.value, weight:f.weight.value, note:f.note.value, user:s.username });
  saveOrders(orders);
  orderModal.hide();
  renderMyOrders();
  showToast('Đơn đã tạo','success');
}

function renderMyOrders(e){
  if(e) e.preventDefault();
  const s = getSession(); 
  if(!s) return;
  const orders = getOrders().filter(o => o.user===s.username || s.role==='staff');

  if(orders.length === 0){
    document.getElementById('dash-content').innerHTML = "<p class='text-muted'>Chưa có đơn hàng nào.</p>";
    return;
  }

  let html = `
  <table class="table table-striped table-bordered align-middle shadow-sm">
    <thead class="table-dark">
      <tr>
        <th>STT</th>
        <th>Người nhận</th>
        <th>Từ</th>
        <th>Đến</th>
        <th>Khối lượng (kg)</th>
        <th>Ghi chú</th>
      </tr>
    </thead>
    <tbody>
  `;

  orders.forEach((o, i) => {
    html += `
      <tr>
        <td>${i+1}</td>
        <td><b>${o.receiver}</b></td>
        <td>${o.from}</td>
        <td>${o.to}</td>
        <td>${o.weight}</td>
        <td>${o.note || ''}</td>
      </tr>
    `;
  });

  html += `</tbody></table>`;
  document.getElementById('dash-content').innerHTML = html;
}


function renderAllOrders(){ renderMyOrders(); }
function renderUsers(e){
  if(e) e.preventDefault();
  const users = getUsers();

  if(users.length === 0){
    document.getElementById('dash-content').innerHTML = "<p class='text-muted'>Chưa có người dùng nào.</p>";
    return;
  }

  let html = `
  <table class="table table-hover table-bordered shadow-sm">
    <thead class="table-dark">
      <tr>
        <th>STT</th>
        <th>Tài khoản</th>
        <th>Tên hiển thị</th>
        <th>Vai trò</th>
      </tr>
    </thead>
    <tbody>
  `;

  users.forEach((u, i) => {
    html += `
      <tr>
        <td>${i+1}</td>
        <td>${u.username}</td>
        <td>${u.name || ''}</td>
        <td>${u.role}</td>
      </tr>
    `;
  });

  html += `</tbody></table>`;
  document.getElementById('dash-content').innerHTML = html;
}


function logout(){ clearSession(); showToast('Đã đăng xuất','info'); }

function showToast(msg,bg='primary'){
  const t = document.getElementById('appToast');
  t.className = 'toast align-items-center text-bg-'+bg+' border-0';
  document.getElementById('appToastBody').innerText = msg;
  t.style.display = 'block';
  const bs = new bootstrap.Toast(t, { delay: 2000 });
  bs.show();
}
function hideToast(){ document.getElementById('appToast').style.display='none'; }
