/* ========================
   app.js - Frontend logic
   ======================== */

/* === Cấu hình: chỉnh URL API ở đây === */
const API_BASE = "https://localhost:7285/api"; 

/* ---------- UTILS ---------- */
function normKey(k){
  // normalize key: giữ nguyên, nhưng giúp map MaNguoiDung -> maNguoiDung, etc.
  return k;
}
function normalizeRecordKeys(obj){
  // convert server keys to camelCase lower to be safer
  const out = {};
  for(const k in obj){
    const lk = k.charAt(0).toLowerCase() + k.slice(1);
    out[lk] = obj[k];
  }
  return out;
}
function showMessage(el, msg, color='red'){
  el.textContent = msg;
  el.style.color = color;
}

/* ---------- INDEX (TRA CỨU) ---------- */
document.getElementById("btnScrollTracking")?.addEventListener("click", ()=>{
  document.getElementById("tracking")?.scrollIntoView({behavior:'smooth'});
});
document.getElementById("btnTrack")?.addEventListener("click", searchTracking);

async function searchTracking() {
  const code = (document.getElementById("trackingInput")?.value || '').trim();
  const container = document.getElementById("trackingResult");

  if (!code) {
    container.innerHTML = `<p style="color:#ffb3b3">Vui lòng nhập mã vận đơn.</p>`;
    return;
  }

  container.innerHTML = `<p>Đang tra cứu...</p>`;

  try {
    // ✅ GỌI ĐÚNG endpoint backend bạn đã định nghĩa
    const res = await fetch(`${API_BASE}/VanDon/tra-cuu/${encodeURIComponent(code)}`);
    if (!res.ok) {
      container.innerHTML = `<p style="color:#ffb3b3">Không tìm thấy vận đơn hoặc lỗi: ${res.status}</p>`;
      return;
    }

    const data = await res.json();

    // ✅ HIỂN THỊ ĐÚNG 10 TRƯỜNG bạn yêu cầu
    const rows = [
      ['Số vận đơn', data.soVanDon || data.maVanDon || '—'],
      ['Thông tin nhà xe', data.thongTinNhaXe || '—'],
      ['Trạng thái', data.trangThai || '—'],
      ['Loại hàng', data.loaiHang || '—'],
      ['Người gửi', data.tenKhachGui || data.nguoiGui || '—'],
      ['Người nhận', data.tenKhachNhan || data.nguoiNhan || '—'],
      ['Địa chỉ Lấy', data.diaChiLay || '—'],
      ['Địa chỉ giao', data.diaChiGiao || '—'],
      ['Tuyến đường', data.maTuyen || '—'],
      ['Giá trị khai báo', data.giaTriKhaiBao ? data.giaTriKhaiBao.toLocaleString() + ' đ' : '—'],
      ['Chứng từ', data.chungTu || '—'],
      ['Ngày tạo', data.ngayTao ? new Date(data.ngayTao).toLocaleString('vi-VN') : '—']
    ];

    // ✅ Tạo bảng hiển thị đẹp
    let html = `<table class="result-table"><tbody>`;
    for (const [label, value] of rows) {
      html += `<tr><th>${label}</th><td>${value}</td></tr>`;
    }
    html += `</tbody></table>`;

    container.innerHTML = html;

  } catch (err) {
    console.error("❌ Lỗi tra cứu vận đơn:", err);
    container.innerHTML = `<p style="color:#ffb3b3">Lỗi khi kết nối đến API.</p>`;
  }
}


/* ---------- AUTH: LOGIN & REGISTER & ROLE ---------- */
function getStoredUser(){
  try{ return JSON.parse(localStorage.getItem('user') || 'null'); }
  catch{ return null; }
}
function setStoredUser(u){ localStorage.setItem('user', JSON.stringify(u)); }

async function doLogin(){
  const userEl = document.getElementById('loginUsername');
  const passEl = document.getElementById('loginPassword');
  const roleSel = document.getElementById('loginRole'); // only used at register
  const msg = document.getElementById('loginMessage');
  msg.textContent = '';
  const username = (userEl?.value||'').trim();
  const password = (passEl?.value||'').trim();
  if(!username||!password){ showMessage(msg,'Vui lòng nhập tài khoản và mật khẩu'); return; }

  try{
    const res = await fetch(`${API_BASE}/NguoiDung/login`, {
      method:'POST',
      headers: {'Content-Type':'application/json'},
      body: JSON.stringify({ TenDangNhap: username, MatKhau: password })
    });
    if(!res.ok){
      const t = await res.json().catch(()=>null);
      showMessage(msg, t?.message || 'Sai tài khoản hoặc mật khẩu');
      return;
    }
    const data = await res.json();
    // data may contain user object in different shapes: attempt to find user
    const user = data.user || data || {};
    const norm = normalizeRecordKeys(user);
    setStoredUser(norm);
    showMessage(msg,'Đăng nhập thành công', 'green');
    // redirect by role (assume maVaiTro or maVaiTroName or role string)
    const roleVal = norm.maVaiTro ?? norm.maVaiTroName ?? norm.role;
    // normalize numeric codes: if code is numeric, map to names? try to use name if string
    // We'll check values: if number and equals 1 -> Admin (common), else map as best.
    if(typeof roleVal === 'number'){
      if(roleVal === 1) window.location.href = 'admin.html';
      else if(roleVal === 2 || roleVal === 3) window.location.href = 'dashboard.html';
      else window.location.href = 'index.html';
    } else {
      const r = (roleVal||'').toString().toLowerCase();
      if(r.includes('admin')) window.location.href = 'admin.html';
      else if(r.includes('dieu') || r.includes('dieuphoi') || r.includes('dieuphối')) window.location.href = 'dashboard.html';
      else if(r.includes('taixe') || r.includes('tài')) window.location.href = 'dashboard.html';
      else window.location.href = 'index.html';
    }
  }catch(err){
    console.error(err);
    showMessage(document.getElementById('loginMessage'),'Lỗi kết nối tới API');
  }
}

function toggleRegister(){
  const block = document.getElementById('registerBlock');
  block?.classList.toggle('hidden');
}

async function doRegister(){
  const un = document.getElementById('regUsername').value.trim();
  const pw = document.getElementById('regPassword').value.trim();
  const role = document.getElementById('regRole').value;
  if(!un||!pw){ alert('Nhập đủ tài khoản/mật khẩu'); return; }
  try{
    const payload = {
      MaNguoiDung: 'ND' + Date.now(),
      TenDangNhap: un,
      MatKhau: pw,
      HoTen: un,
      MaVaiTro: role // backend expects int; if API maps string -> handle it server side. If not, convert: map role string to int.
    };
    // If your backend expects numeric MaVaiTro, map here:
    const roleMap = { Admin:1, DieuPhoi:2, TaiXe:3, KhachHang:4 };
    if(roleMap[role]) payload.MaVaiTro = roleMap[role];

    const res = await fetch(`${API_BASE}/NguoiDung`, {
      method:'POST',
      headers:{'Content-Type':'application/json'},
      body: JSON.stringify(payload)
    });
    if(res.ok){ alert('Đăng ký thành công — chờ admin kích hoạt nếu cần'); toggleRegister(); }
    else {
      const txt = await res.text();
      alert('Đăng ký thất bại: ' + txt);
    }
  }catch(err){ console.error(err); alert('Lỗi kết nối'); }
}

/* ---------- LOGOUT ---------- */
function logout(){ localStorage.removeItem('user'); location.href='login.html' }

/* ---------- DASHBOARD: table CRUD generic ---------- */
let currentEntity = null;
let currentData = []; // last loaded array
let selectedRowIndex = null;

// --- Pagination ---
let page = 1;
let pageSize = 10;
let totalPages = 1;


function showWhoAmI(){
  const u = getStoredUser();
  const el = document.getElementById('whoami');
  if(!el) return;
  if(!u) el.textContent = 'Chưa đăng nhập';
  else el.textContent = `${u.hoTen || u.tenDangNhap || 'User'} (${u.maVaiTro || ''})`;
}
showWhoAmI();

async function loadTable(entity){
  currentEntity = entity;
  selectedRowIndex = null;
  document.getElementById('currentEntity') && (document.getElementById('currentEntity').textContent = entity);
  const wrap = document.getElementById('dataTable');
  wrap && (wrap.innerHTML = '<p>Đang tải...</p>');

  try{
    const res = await fetch(`${API_BASE}/${entity}`);
    if(!res.ok){ wrap.innerHTML = `<p style="color: #ffb3b3">Lỗi: ${res.status}</p>`; return; }
    const data = await res.json();
    currentData = Array.isArray(data) ? data.map(normalizeRecordKeys) : [];
    
    page = 1;
    renderTable();

    // adjust permissions: Admin & DieuPhoi can add/edit/delete; TaiXe only view and update status maybe
    const role = (getStoredUser()?.maVaiTro) || (getStoredUser()?.maVaiTroName) || null;
    const roleNum = Number(role) || null;
    const roleName = (roleNum === 1 ? 'Admin' : roleNum === 2 ? 'TaiXe' : roleNum === 3 ? 'DieuPhoi' : roleNum === 4 ? 'KhachHang' : (role || ''));

    // show/hide buttons according to role
    const canEdit = roleName==='Admin' || roleName==='DieuPhoi';
    const canAdd = canEdit;
    const canDelete = roleName==='Admin' || roleName==='DieuPhoi';
    document.getElementById('btnAdd') && (document.getElementById('btnAdd').style.display = canAdd ? 'inline-block' : 'none');
    document.getElementById('btnEdit') && (document.getElementById('btnEdit').style.display = canEdit ? 'inline-block' : 'none');
    document.getElementById('btnDel') && (document.getElementById('btnDel').style.display = canDelete ? 'inline-block' : 'none');

  }catch(err){ console.error(err); wrap.innerHTML = `<p style="color:#ffb3b3">Lỗi kết nối</p>`; }
}

const headerMap = {
  /* ========= BẢNG NGƯỜI DÙNG ========= */
  maNguoiDung: "Mã người dùng",
  tenDangNhap: "Tên đăng nhập",
  matKhau: "Mật khẩu",
  hoTen: "Họ tên",
  maVaiTro: "Mã vai trò",
  ngayTao: "Ngày tạo",

  /* ========= BẢNG KHÁCH HÀNG ========= */
  maKhachHang: "Mã khách hàng",
  tenKhachHang: "Tên khách hàng",
  soDienThoai: "Số điện thoại",
  email: "Email",

  /* ========= BẢNG ĐỊA CHỈ ========= */
  maDiaChi: "Mã địa chỉ",
  maKhachHang: "Mã khách hàng",
  diaChiChiTiet: "Địa chỉ chi tiết",
  thanhPho: "Thành phố",
  quanHuyen: "Quận / Huyện",
  maBuuDien: "Mã bưu điện",

  /* ========= BẢNG ĐƠN VẬN CHUYỂN ========= */
  maDon: "Mã đơn",
  maDonCode: "Mã đơn code",
  maVanDon: "Mã vận đơn",
  maKhachGui: "Mã khách gửi",
  maKhachNhan: "Mã khách nhận",
  maDiaChiLay: "Mã địa chỉ lấy hàng",
  maDiaChiGiao: "Mã địa chỉ giao hàng",
  loaiHang: "Loại hàng",
  khoiLuong: "Khối lượng (kg)",
  giaTriKhaiBao: "Giá trị khai báo",
  nguoiTao: "Người tạo",
  maTuyen: "Mã tuyến đường",
  trangThai: "Trạng thái đơn hàng",

  /* ========= BẢNG VẬN ĐƠN ========= */
  soVanDon: "Số vận đơn",
  ngayPhatHanh: "Ngày phát hành",
  thongTinNhaXe: "Thông tin nhà xe",

  /* ========= BẢNG TUYẾN ĐƯỜNG ========= */
  maTuyenCode: "Mã tuyến code",
  maTaiXe: "Mã tài xế",
  phuongTien: "Phương tiện",
  thoiGianBatDau: "Thời gian khởi hành",
  thoiGianKetThuc: "Thời gian kết thúc",
  maKhuVuc: "Mã khu vực",
  doanhThuUocTinh: "Doanh thu ước tính",

  /* ========= BẢNG ĐIỂM DỪNG ========= */
  maDiemDung: "Mã điểm dừng",
  thuTuDung: "Thứ tự dừng",
  duKienDen: "Dự kiến đến",
  thucTeDen: "Thực tế đến",

  /* ========= BẢNG SỰ KIỆN TRẠNG THÁI ========= */
  maSuKien: "Mã sự kiện",
  lyDo: "Lý do",
  thoiGian: "Thời gian cập nhật",
  nguoiCapNhat: "Người cập nhật",
  duLieuThem: "Dữ liệu thêm",
  maSuKienNgoai: "Mã sự kiện ngoài",
  khoaIdempotent: "Khóa Idempotent",

  /* ========= BẢNG GIAO DỊCH COD ========= */
  maGiaoDich: "Mã giao dịch",
  soTien: "Số tiền COD",
  nguoiThu: "Người thu",
  ngayThu: "Ngày thu",
  daDoiSoat: "Đã đối soát",
  ngayDoiSoat: "Ngày đối soát",
  soTienThanhToan: "Số tiền thanh toán",
  
  /* ========= BẢNG CHỨNG TỪ ========= */
  maChungTu: "Mã chứng từ",
  nguoiUpload: "Người upload",
  ngayUpload: "Ngày upload",
  kyNhan: "Ký nhận",
  duongDanThuNho: "Đường dẫn thu nhỏ",
  loaiKyNhan: "Loại ký nhận",

  /* ========= BẢNG ĐƠN YÊU CẦU ========= */
  maYeuCau: "Mã yêu cầu",
  tenNguoiGui: "Tên người gửi",
  sdtNguoiGui: "SĐT người gửi",
  emailNguoiGui: "Email người gửi",
  diaChiGui: "Địa chỉ gửi",
  tenNguoiNhan: "Tên người nhận",
  sdtNguoiNhan: "SĐT người nhận",
  emailNguoiNhan: "Email người nhận",
  diaChiNhan: "Địa chỉ nhận",
  ghiChu: "Ghi chú",
};

function renderTable() {
    const wrap = document.getElementById("dataTable");

    if (!currentData.length) {
        wrap.innerHTML = "<p>Không có dữ liệu</p>";
        return;
    }

    totalPages = Math.ceil(currentData.length / pageSize);
    if (page > totalPages) page = totalPages;

    const start = (page - 1) * pageSize;
    const pageData = currentData.slice(start, start + pageSize);
    const keys = Object.keys(pageData[0]);

    let html = `<table id="entityTable"><thead><tr>
    <th></th>${keys.map(k => `<th>${headerMap[k] || k}</th>`).join('')}
    </tr></thead><tbody>`;

    pageData.forEach((r, idx) => {
        const globalIndex = start + idx;
        html += `<tr data-idx="${globalIndex}" onclick="onRowClick(${globalIndex})">
            <td><input type='radio' name='selRow'></td>
            ${keys.map(k => `<td>${r[k] ?? ""}</td>`).join("")}
        </tr>`;
    });

    html += "</tbody></table>";
    wrap.innerHTML = html;

    renderPageNumbers();
}

function nextPage() {
    if (page < totalPages) {
        page++;
        renderTable();
    }
}

function prevPage() {
    if (page > 1) {
        page--;
        renderTable();
    }
}

function changePageSize() {
    const select = document.getElementById("pageSizeSelect");
    pageSize = Number(select.value);
    page = 1;
    renderTable();
}


function renderPageNumbers() {
    const container = document.getElementById("pageNumbers");
    if (!container) return;
    container.innerHTML = "";

    let maxPagesToShow = 5;
    let start = Math.max(1, page - 2);
    let end = Math.min(totalPages, start + maxPagesToShow - 1);

    if (end - start < 4) {
        start = Math.max(1, end - 4);
    }

    for (let i = start; i <= end; i++) {
        const btn = document.createElement("button");
        btn.textContent = i;
        btn.className = "page-number " + (i === page ? "active" : "");
        btn.onclick = () => { page = i; renderTable(); };
        container.appendChild(btn);
    }
}


function onRowClick(idx){
  selectedRowIndex = idx;
  // highlight selected row
  document.querySelectorAll('#entityTable tbody tr').forEach(tr=>tr.classList.remove('selected'));
  const tr = document.querySelector(`#entityTable tbody tr[data-idx="${idx}"]`);
  if(tr) tr.classList.add('selected');
}

/* ---------- CẤU HÌNH FORM TÙY THEO BẢNG ---------- */
const formTemplates = {

  KhachHang: [
    { key: 'MaKhachHang', label: 'Mã khách hàng' },
    { key: 'TenKhachHang', label: 'Tên khách hàng' },
    { key: 'SoDienThoai', label: 'Số điện thoại' },
    { key: 'Email', label: 'Email' },
    { key: 'NgayTao', label: 'Ngày tạo', type: 'datetime-local', readonly: true }
  ],

  DiaChi: [
    { key: 'MaDiaChi', label: 'Mã địa chỉ' },
    { key: 'MaKhachHang', label: 'Mã khách hàng' },
    { key: 'DiaChiChiTiet', label: 'Địa chỉ chi tiết' },
    { key: 'ThanhPho', label: 'Thành phố' },
    { key: 'QuanHuyen', label: 'Quận / Huyện' },
    { key: 'MaBuuDien', label: 'Mã bưu điện' }
  ],

  DonVanChuyen: [
    { key: 'MaDon', label: 'Mã đơn' },
    { key: 'MaDonCode', label: 'Mã đơn code' },
    { key: 'MaVanDon', label: 'Mã vận đơn (nếu có)' },
    { key: 'MaKhachGui', label: 'Mã khách gửi' },
    { key: 'MaKhachNhan', label: 'Mã khách nhận' },
    { key: 'MaDiaChiLay', label: 'Mã địa chỉ lấy hàng' },
    { key: 'MaDiaChiGiao', label: 'Mã địa chỉ giao hàng' },
    { key: 'LoaiHang', label: 'Loại hàng' },
    { key: 'KhoiLuong', label: 'Khối lượng (kg)', type: 'number', step: '0.001' },
    { key: 'GiaTriKhaiBao', label: 'Giá trị khai báo (VNĐ)', type: 'number', step: '0.01' },
    { key: 'NguoiTao', label: 'Người tạo (Mã người dùng)' },
    { key: 'MaTuyen', label: 'Tuyến đường' },
    { key: 'TrangThai', label: 'Trạng thái đơn hàng', default: 'Khởi tạo' },
    { key: 'NgayTao', label: 'Ngày tạo', type: 'datetime-local', readonly: true }
  ],

  VanDon: [
    { key: 'MaVanDon', label: 'Mã vận đơn' },
    { key: 'SoVanDon', label: 'Số vận đơn' },
    { key: 'MaDon', label: 'Mã đơn vận chuyển' },
    { key: 'NgayPhatHanh', label: 'Ngày phát hành', type: 'datetime-local' },
    { key: 'ThongTinNhaXe', label: 'Thông tin nhà xe' }
  ],

  TuyenDuong: [
    { key: 'MaTuyen', label: 'Mã tuyến' },
    { key: 'MaTuyenCode', label: 'Mã tuyến code' },
    { key: 'MaTaiXe', label: 'Mã tài xế' },
    { key: 'PhuongTien', label: 'Phương tiện' },
    { key: 'ThoiGianBatDau', label: 'Thời gian khởi hành', type: 'datetime-local' },
    { key: 'ThoiGianKetThuc', label: 'Thời gian kết thúc', type: 'datetime-local' },
    { key: 'MaKhuVuc', label: 'Mã khu vực' },
    { key: 'DoanhThuUocTinh', label: 'Doanh thu ước tính (VNĐ)', type: 'number', step: '0.01' },
    { key: 'NgayTao', label: 'Ngày tạo', type: 'datetime-local', readonly: true }
  ],

  DiemDung: [
    { key: 'MaDiemDung', label: 'Mã điểm dừng' },
    { key: 'MaTuyen', label: 'Mã tuyến' },
    { key: 'ThuTuDung', label: 'Thứ tự dừng', type: 'number' },
    { key: 'MaDon', label: 'Mã đơn vận chuyển' },
    { key: 'DuKienDen', label: 'Thời gian dự kiến đến', type: 'datetime-local' },
    { key: 'ThucTeDen', label: 'Thời gian thực tế đến', type: 'datetime-local' }
  ],

  SuKienTrangThai: [
    { key: 'MaSuKien', label: 'Mã sự kiện' },
    { key: 'MaDon', label: 'Mã đơn vận chuyển' },
    { key: 'TrangThai', label: 'Trạng thái (Đã lấy / Đang giao / Đã giao / Thất bại)' },
    { key: 'LyDo', label: 'Lý do (nếu thất bại)' },
    { key: 'ThoiGian', label: 'Thời gian cập nhật', type: 'datetime-local' },
    { key: 'NguoiCapNhat', label: 'Người cập nhật (Mã người dùng)' },
    { key: 'DuLieuThem', label: 'Dữ liệu thêm (JSON)' },
    { key: 'MaSuKienNgoai', label: 'Mã sự kiện ngoài (nếu có)' },
    { key: 'KhoaIdempotent', label: 'Khóa Idempotent' },
    { key: 'NgayTao', label: 'Ngày tạo', type: 'datetime-local', readonly: true }
  ],

  GiaoDichCOD: [
    { key: 'MaGiaoDich', label: 'Mã giao dịch' },
    { key: 'MaDon', label: 'Mã đơn vận chuyển' },
    { key: 'SoTien', label: 'Số tiền COD (VNĐ)', type: 'number', step: '0.01' },
    { key: 'NguoiThu', label: 'Người thu (Mã người dùng)' },
    { key: 'NgayThu', label: 'Ngày thu tiền', type: 'datetime-local' },
    { key: 'DaDoiSoat', label: 'Đã đối soát (1 = có, 0 = chưa)' },
    { key: 'NgayDoiSoat', label: 'Ngày đối soát', type: 'datetime-local' },
    { key: 'SoTienThanhToan', label: 'Số tiền thanh toán cho người gửi', type: 'number', step: '0.01' },
    { key: 'DuLieuThem', label: 'Dữ liệu thêm (JSON)' }
  ],

  ChungTu: [
    { key: 'MaChungTu', label: 'Mã chứng từ' },
    { key: 'MaDon', label: 'Mã đơn vận chuyển' },
    { key: 'NguoiUpload', label: 'Người upload (Mã người dùng)' },
    { key: 'NgayUpload', label: 'Ngày upload', type: 'datetime-local', readonly: true },
    { key: 'KyNhan', label: 'Ký nhận (văn bản / ảnh)' },
    { key: 'DuongDanThuNho', label: 'Đường dẫn thu nhỏ' },
    { key: 'LoaiKyNhan', label: 'Loại ký nhận (Ảnh / Chữ ký)' }
  ],

  DonYeuCau: [
    { key: "MaYeuCau", label: "Mã yêu cầu", readonly: true },
    { key: "TenNguoiGui", label: "Tên người gửi" },
    { key: "SDTNguoiGui", label: "SĐT người gửi" },
    { key: "EmailNguoiGui", label: "Email người gửi" },
    { key: "DiaChiGui", label: "Địa chỉ gửi" },

    { key: "TenNguoiNhan", label: "Tên người nhận" },
    { key: "SDTNguoiNhan", label: "SĐT người nhận" },
    { key: "EmailNguoiNhan", label: "Email người nhận" },
    { key: "DiaChiNhan", label: "Địa chỉ nhận" },

    { key: "LoaiHang", label: "Loại hàng" },
    { key: "KhoiLuong", label: "Khối lượng", type: "number", step: "0.001" },
    { key: "GiaTriKhaiBao", label: "Giá trị khai báo", type: "number" },
    { key: "GhiChu", label: "Ghi chú" },
    { key: 'NgayTao', label: 'Ngày tạo', type: 'datetime-local', readonly: true }
  ],

};


/* ---------- Generic Add/Edit/Delete handlers ---------- */
function onAdd(entity){
  const entityName = entity || currentEntity;
  if(!entityName){ alert('Chưa chọn bảng'); return; }

  // nếu có mẫu form riêng thì dùng, nếu không thì lấy keys từ dữ liệu
  const template = formTemplates[entityName];
  const keys = template ? template.map(f=>f.key) : (Object.keys(currentData[0]||{}));
  const labels = template ? Object.fromEntries(template.map(f=>[f.key,f.label])) : {};

  openModal(`Thêm ${entityName}`, keys, {}, async (formData)=>{
    const payload = mapPayload(formData);

    if (entityName === 'SuKienTrangThai') delete payload.MaSuKien;
    if (entityName === 'GiaoDichCOD') delete payload.MaGiaoDich;
    if (entityName === 'ChungTu') delete payload.MaChungTu;

    try{
      const res = await fetch(`${API_BASE}/${entityName}`, {
        method:'POST',
        headers:{'Content-Type':'application/json'},
        body: JSON.stringify(payload)
      });
      if(res.ok){
        alert('Thêm thành công');
        loadTable(entityName);
        closeModal();
      } else {
        const t = await res.text().catch(()=>null);
        alert('Thêm thất bại: ' + t);
      }
    }catch(err){ console.error(err); alert('Lỗi kết nối'); }
  }, labels);
}


function onEdit() {
  if (selectedRowIndex === null) {
    alert('Hãy chọn một dòng để sửa');
    return;
  }
  const row = currentData[selectedRowIndex];
  const entityName = currentEntity;
  const template = formTemplates[entityName];
  const keys = template ? template.map(f => f.key) : Object.keys(row);
  const labels = template ? Object.fromEntries(template.map(f => [f.key, f.label])) : {};

  openModal(`Sửa ${entityName}`, keys, row, async (formData) => {
    const payload = mapPayload(formData);
    const idKey = Object.keys(row)[0];
    const idVal = row[idKey];
    try {
      const res = await fetch(`${API_BASE}/${currentEntity}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(payload)
      });
      if (res.ok) {
        alert('Cập nhật thành công');
        loadTable(entityName);
        closeModal();
      } else {
        const t = await res.text().catch(() => null);
        alert('Cập nhật thất bại: ' + t);
      }
    } catch (err) {
      console.error(err);
      alert('Lỗi kết nối');
    }
  }, labels);
}


async function onDelete(){
  if(selectedRowIndex===null){ alert('Hãy chọn một dòng để xóa'); return; }
  if(!confirm('Bạn có chắc muốn xóa bản ghi này?')) return;
  const row = currentData[selectedRowIndex];
  const idKey = Object.keys(row)[0];
  const idVal = row[idKey];
  try{
    const res = await fetch(`${API_BASE}/${currentEntity}/${encodeURIComponent(idVal)}`, { method:'DELETE' });
    if(res.ok){ alert('Xóa thành công'); loadTable(currentEntity); }
    else { const t=await res.text().catch(()=>null); alert('Xóa thất bại: ' + t); }
  }catch(err){ console.error(err); alert('Lỗi kết nối'); }
}

/* ---------- Modal helper ---------- */
function openModal(title, keys, values = {}, onSave, labels = {}){
  const modal = document.getElementById('modalForm');
  const titleEl = document.getElementById('modalTitle');
  const form = document.getElementById('entityForm');
  titleEl.textContent = title;
  form.innerHTML = '';
  keys.forEach(k=>{
    const camel = k.charAt(0).toLowerCase() + k.slice(1);
    const val = values[k] ?? values[camel] ?? '';

    // first key treat as primary key: for POST allow to input but for PUT may be read-only
    const isPrimary = (k === keys[0]);
    const inputHTML = `
      <div class="form-row">
        <label style="font-weight:600">${labels[k] || k}</label>
        <input name="${k}" value="${val}" ${isPrimary && values[k] ? 'readonly' : ''} />
      </div>
    `;
    form.insertAdjacentHTML('beforeend', inputHTML);
  });
  modal.classList.remove('hidden');

  document.getElementById('modalCancel').onclick = ()=> closeModal();
  document.getElementById('modalSave').onclick = async (e)=>{
    e.preventDefault();
    // collect values
    const fd = {};
    form.querySelectorAll('input, select, textarea').forEach(inp => fd[inp.name] = inp.value);
    await onSave(fd);
  };
}
function closeModal(){ document.getElementById('modalForm').classList.add('hidden'); }
function closeModalSilent(){ document.getElementById('modalForm').classList.add('hidden'); }

/* map form payload keys -> may need PascalCase depending on backend expectation */
function mapPayload(formData) {
  const out = {};
  for (const k in formData) {
    if (k.toLowerCase() === "ngaytao") continue; // ❌ bỏ qua NgayTao khi gửi

    let val = formData[k];

    // ✅ Xử lý kiểu boolean cho trường DaDoiSoat
    if (k.toLowerCase() === "dadoisoat") {
      val = (val === "1" || val === "true" || val === true);
    }

    // ✅ Nếu có các trường ngày giờ (datetime-local) thì convert
    if (k.toLowerCase().includes("ngay") || k.toLowerCase().includes("thoigian")) {
      if (val) val = new Date(val);
    }

    const ps = k.charAt(0).toUpperCase() + k.slice(1);
    out[ps] = val; // ✅ gán giá trị đã xử lý
  }
  return out;
}


/* ---------- Admin: load users view ---------- */
async function loadUsers(){
  currentEntity = 'NguoiDung';
  selectedRowIndex = null;
  const wrap = document.getElementById('dataTable');
  wrap.innerHTML = 'Đang tải...';
  try{
    const res = await fetch(`${API_BASE}/NguoiDung`);
    const data = await res.json();
    currentData = Array.isArray(data) ? data.map(normalizeRecordKeys) : [];
    if(currentData.length === 0){ wrap.innerHTML = '<p>Không có tài khoản</p>'; return; }
    // build table like generic
    const keys = Object.keys(currentData[0]);
    let html = `<table id="entityTable"><thead><tr><th></th>${keys.map(k=>`<th>${headerMap[k] || k}</th>`).join('')}</tr></thead><tbody>`;
    currentData.forEach((r, idx)=>{
      html += `<tr data-idx="${idx}" onclick="onRowClick(${idx})"><td><input type="radio" name="selRow"></td>${keys.map(k=>`<td>${r[k]??''}</td>`).join('')}</tr>`;
    });
    html += '</tbody></table>';
    wrap.innerHTML = html;
  }catch(err){ console.error(err); wrap.innerHTML = '<p style="color:#ffb3b3">Lỗi kết nối</p>'; }
}

/* ---------- small helpers for admin page navigation ---------- */
function goDashboard(){ location.href='dashboard.html' }

/* ---------- Init on dashboard/admin pages ---------- */
document.addEventListener('DOMContentLoaded', ()=>{
  // auto fill whoami
  showWhoAmI();
  // If on admin page and user not admin, redirect
  if(location.pathname.endsWith('admin.html')){
    const u = getStoredUser();
    const role = u?.maVaiTro ?? u?.maVaiTroName ?? '';
    const roleName = (role === 1 || role === 'Admin') ? 'Admin' : role;
    if(!roleName || !(roleName===1 || roleName==='Admin' || roleName==='admin')){
      // not admin -> redirect to dashboard or login
      alert('Quyền truy cập bị từ chối. Chỉ Admin có quyền vào trang này.');
      location.href = 'login.html';
    } else {
      // load users by default
      loadUsers();
    }
  }
  // If on dashboard page, show whoami
  if(location.pathname.endsWith('dashboard.html')) showWhoAmI();
});



// Tìm kiếm
async function onSearch() {
  const entity = currentEntity;
  const keyword = document.getElementById("searchInput")?.value?.trim();
  const table = document.getElementById("dataTable");

  if (!entity) {
    alert("Vui lòng chọn bảng cần tìm trước!");
    return;
  }

  if (!keyword) {
    alert("Vui lòng nhập mã cần tìm!");
    return;
  }

  table.innerHTML = `<p>🔍 Đang tìm kiếm dữ liệu...</p>`;

  try {
    const res = await fetch(`${API_BASE}/${entity}/${encodeURIComponent(keyword)}`);
    if (!res.ok) {
      table.innerHTML = `<p style="color:red;">Không tìm thấy dữ liệu hoặc API lỗi (${res.status})</p>`;
      return;
    }

    const data = await res.json();

    // Chuẩn hóa dữ liệu thành mảng để hiển thị
    const list = Array.isArray(data) ? data : [data];

    if (!list.length) {
      table.innerHTML = `<p style="color:orange;">Không có kết quả phù hợp.</p>`;
      return;
    }

    // Tạo bảng HTML hiển thị kết quả
    currentData = list.map(normalizeRecordKeys);

    const headers = Object.keys(currentData[0]);
    let html = `<table id="entityTable"><thead><tr><th></th>`;
    html += headers.map(h => `<th>${h}</th>`).join('');
    html += `</tr></thead><tbody>`;

    currentData.forEach((row, idx) => {
      html += `
        <tr data-idx="${idx}" onclick="onRowClick(${idx})">
          <td><input type="radio" name="selRow"></td>
          ${headers.map(h => `<td>${row[h] ?? ''}</td>`).join('')}
        </tr>`;
    });

    html += `</tbody></table>`;
    table.innerHTML = html;

  } catch (err) {
    console.error("Lỗi khi kết nối API:", err);
    table.innerHTML = `<p style="color:red;">❌ Lỗi kết nối đến API backend.</p>`;
  }
}

// làm mới 
async function onRefresh() {
  if (!currentEntity) {
    alert("Vui lòng chọn bảng cần làm mới!");
    return;
  }

  const table = document.getElementById("dataTable");
  table.innerHTML = `<p>🔄 Đang tải dữ liệu...</p>`;

  try {
    await loadTable(currentEntity);
    document.getElementById("searchInput").value = ""; // Xóa ô tìm kiếm
  } catch (err) {
    console.error("Lỗi khi làm mới dữ liệu:", err);
    table.innerHTML = `<p style="color:red;">❌ Không thể tải lại dữ liệu.</p>`;
  }
}

// xuất excel
async function onExport() {
  const entity = currentEntity;
  if (!entity) {
    alert("Vui lòng chọn bảng cần xuất Excel!");
    return;
  }

  try {
    const res = await fetch(`${API_BASE}/${entity}/export-excel`);
    if (!res.ok) {
      alert("Không thể xuất Excel: " + res.status);
      return;
    }

    const blob = await res.blob();
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement("a");
    a.href = url;
    a.download = `${entity}.xlsx`;
    a.click();
    window.URL.revokeObjectURL(url);
  } catch (err) {
    console.error(err);
    alert("Lỗi kết nối API khi xuất Excel!");
  }
}

function openImport() {
    if (!currentEntity) {
        alert("❌ Chưa chọn loại dữ liệu để import!");
        return;
    }
    document.getElementById("importFile").click();
}

async function importExcel() {
    if (!currentEntity) {
        alert("❌ Bạn phải chọn bảng trước khi import!");
        return;
    }

    const fileInput = document.getElementById("importFile");
    if (!fileInput.files.length) {
        alert("❌ Bạn chưa chọn file Excel!");
        return;
    }

    const file = fileInput.files[0];
    const formData = new FormData();
    formData.append("file", file);

    try {
        const res = await fetch(`${API_BASE}/${currentEntity}/import-excel`, {
            method: "POST",
            body: formData
        });

        const txt = await res.text();

        if (!res.ok) {
            alert("❌ Import thất bại:\n" + txt);
            return;
        }

        alert("✅ Import thành công!");
        loadTable(currentEntity);

    } catch (err) {
        console.error(err);
        alert("❌ Lỗi kết nối server!");
    }
}


//khachhang

function toggleCustomerRegister() {
    const loginCard = document.getElementById("loginCard");
    const regBlock = document.getElementById("customerRegisterBlock");

    loginCard.classList.toggle("hidden");
    regBlock.classList.toggle("hidden");
}

async function registerCustomer() {
    const id = "ND" + Date.now();  // dùng cho cả người dùng và khách hàng

    const userPayload = {
        MaNguoiDung: id,
        TenDangNhap: document.getElementById("regUser").value,
        MatKhau: document.getElementById("regPass").value,
        HoTen: document.getElementById("regHoTen").value,
        Email: document.getElementById("regEmail").value,
        SoDienThoai: document.getElementById("regPhone").value,
        MaVaiTro: 4 // khách hàng
    };

    // 1) Tạo tài khoản người dùng
    const resUser = await fetch(`${API_BASE}/NguoiDung`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(userPayload)
    });

    if (!resUser.ok) {
        alert("❌ Lỗi tạo tài khoản người dùng");
        return;
    }

    // 2) Tạo bản ghi khách hàng
    const customerPayload = {
        MaKhachHang: id, // dùng chung ID với người dùng
        TenKhachHang: userPayload.HoTen,
        SoDienThoai: userPayload.SoDienThoai,
        Email: userPayload.Email
    };

    const resCustomer = await fetch(`${API_BASE}/KhachHang`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(customerPayload)
    });

    if (!resCustomer.ok) {
        alert("⚠ Đã tạo tài khoản nhưng tạo khách hàng thất bại!");
        return;
    }

    alert("🎉 Tạo tài khoản thành công!");
    toggleCustomerRegister(); // quay lại login
}



document.addEventListener("DOMContentLoaded", () => {
  const u = getStoredUser();
  const role = u?.maVaiTro;

  if (role == 4) {  
    document.getElementById("customerActions")?.classList.remove("hidden");
  }
});

function gotoCreateOrder() {
  window.location.href = "create-order.html";
}


document.addEventListener("DOMContentLoaded", () => {
    const user = getStoredUser();

    const btnLogin  = document.getElementById("btnLogin");
    const btnLogout = document.getElementById("btnLogout");
    const btnProfile = document.getElementById("btnProfile");

    if (!btnLogin) return;

    if (user) {
        // Đã đăng nhập
        btnLogin.classList.add("hidden");
        btnLogout.classList.remove("hidden");
        btnProfile.classList.remove("hidden");

        btnLogout.onclick = () => {
            localStorage.removeItem("user");
            alert("Đã đăng xuất!");
            window.location.href = "login.html"; 
        };

        btnProfile.onclick = () => location.href = "profile.html";

    } else {
        // Chưa đăng nhập
        btnLogin.classList.remove("hidden");
        btnLogout.classList.add("hidden");
        btnProfile.classList.add("hidden");
    }
});





