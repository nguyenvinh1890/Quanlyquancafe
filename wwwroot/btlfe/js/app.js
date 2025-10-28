// ========================================
// QUẢN LÝ QUÁN CAFE - JAVASCRIPT
// ========================================


//- Sử dụng LocalStorage để lưu thông tin đăng nhập

// Địa chỉ API Backend 
const API_BASE_URL = 'https://localhost:44390/api';




// Hàm hiển thị thông báo (success/danger/warning/info)
// Dùng: showAlert('Thêm thành công!', 'success')
function showAlert(message, type = 'info') {
    const alertContainer = document.getElementById('alertContainer');
    if (!alertContainer) return;  // nếu không tìm thấy container thì thoát

    const alert = document.createElement('div');  // tạo thẻ div mới
    alert.className = `alert alert-${type}`;      // thêm class CSS
    alert.innerHTML = message;                     // nội dung thông báo
    
    alertContainer.appendChild(alert);             // thêm vào trang
    
    // Tự động xóa sau 5 giây
    setTimeout(() => {
        alert.remove();
    }, 5000);
}

// Hàm format tiền VNĐ - Ví dụ: 50000 → 50.000 ₫
function formatCurrency(amount) {
    return new Intl.NumberFormat('vi-VN', {
        style: 'currency',       // kiểu tiền tệ
        currency: 'VND'          // đồng Việt Nam
    }).format(amount);
}

// Format ngày giờ đầy đủ - Ví dụ: "26/10/2025 20:30:00"
function formatDateTime(dateString) {
    const date = new Date(dateString);
    return date.toLocaleString('vi-VN');  // format theo kiểu Việt Nam
}

// Format chỉ ngày - Ví dụ: "26/10/2025"
function formatDate(dateString) {
    const date = new Date(dateString);
    return date.toLocaleDateString('vi-VN');
}

// Format chỉ giờ - Ví dụ: "08:30" (cắt bỏ giây)
function formatTime(timeString) {
    if (!timeString) return '';           // nếu rỗng thì return ''
    return timeString.substring(0, 5);    // lấy 5 ký tự đầu: HH:mm
}

// ========================================
// MODAL FUNCTIONS - Các hàm mở/đóng popup
// ========================================

// Mở modal (popup) - truyền vào ID của modal

function openModal(modalId) {
    const modal = document.getElementById(modalId);
    if (modal) {
        modal.classList.add('active');  // thêm class 'active' để hiển thị
    }
}

// Đóng modal
function closeModal(modalId) {
    const modal = document.getElementById(modalId);
    if (modal) {
        modal.classList.remove('active');  // xóa class 'active' để ẩn đi
    }
}

// Đóng modal khi click vào nền đen phía sau
window.onclick = function(event) {
    if (event.target.classList.contains('modal')) {
        event.target.classList.remove('active');
    }
}

// ========================================
// API CALL FUNCTIONS - Các hàm gọi API
// Sử dụng Fetch API để gọi backend
// ========================================

// Hàm GET - Lấy dữ liệu từ server
// Ví dụ: const data = await apiGet('/KhuVuc');
async function apiGet(endpoint) {
    try {
        const response = await fetch(`${API_BASE_URL}${endpoint}`, {
            method: 'GET',                    // phương thức GET
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${getAuthToken()}`  // token xác thực
            }
        });
        
        // Kiểm tra response có OK không (status 200-299)
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        
        return await response.json();  // chuyển response thành JSON
    } catch (error) {
        console.error('API GET Error:', error);
        showAlert('Lỗi khi tải dữ liệu: ' + error.message, 'danger');
        return null;  // trả về null nếu lỗi
    }
}

// Hàm POST - Thêm dữ liệu mới vào server
// Ví dụ: await apiPost('/KhuVuc', {tenKV: 'Tầng 1', moTa: 'Khu vực tầng 1'});
async function apiPost(endpoint, data) {
    try {
        const response = await fetch(`${API_BASE_URL}${endpoint}`, {
            method: 'POST',                    // phương thức POST
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${getAuthToken()}`
            },
            body: JSON.stringify(data)         // chuyển object thành JSON string
        });
        
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        
        return await response.json();
    } catch (error) {
        console.error('API POST Error:', error);
        showAlert('Lỗi khi thêm dữ liệu: ' + error.message, 'danger');
        return null;
    }
}

// Hàm PUT - Cập nhật dữ liệu đã có
// Ví dụ: await apiPut('/KhuVuc/1', {tenKV: 'Tầng 1 Mới'});
async function apiPut(endpoint, data) {
    try {
        const response = await fetch(`${API_BASE_URL}${endpoint}`, {
            method: 'PUT',                     // phương thức PUT (update)
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${getAuthToken()}`
            },
            body: JSON.stringify(data)
        });
        
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        
        return await response.json();
    } catch (error) {
        console.error('API PUT Error:', error);
        showAlert('Lỗi khi cập nhật dữ liệu: ' + error.message, 'danger');
        return null;
    }
}

// Hàm DELETE - Xóa dữ liệu
// Ví dụ: await apiDelete('/KhuVuc/1');
async function apiDelete(endpoint) {
    try {
        const response = await fetch(`${API_BASE_URL}${endpoint}`, {
            method: 'DELETE',                  // phương thức DELETE (xóa)
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${getAuthToken()}`
            }
        });
        
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        
        return true;  // trả về true nếu xóa thành công
    } catch (error) {
        console.error('API DELETE Error:', error);
        showAlert('Lỗi khi xóa dữ liệu: ' + error.message, 'danger');
        return false;  // trả về false nếu lỗi
    }
}

// ========================================
// AUTHENTICATION - Xác thực người dùng
// Sử dụng LocalStorage để lưu token và thông tin user
// ========================================

// Lấy token từ LocalStorage - dùng để xác thực với API
function getAuthToken() {
    return localStorage.getItem('authToken') || '';  // trả về '' nếu không có
}

// Lưu token vào LocalStorage - sau khi đăng nhập thành công
function setAuthToken(token) {
    localStorage.setItem('authToken', token);
}

// Lấy thông tin user đã lưu (tên, vai trò...)
function getUserInfo() {
    const userInfo = localStorage.getItem('userInfo');
    return userInfo ? JSON.parse(userInfo) : null;  // chuyển từ JSON string về object
}

// Lưu thông tin user
function setUserInfo(userInfo) {
    localStorage.setItem('userInfo', JSON.stringify(userInfo));  // chuyển object thành JSON string
}

// Đăng xuất - xóa token và chuyển về trang login
function logout() {
    localStorage.removeItem('authToken');    // xóa token
    localStorage.removeItem('userInfo');     // xóa thông tin user
    window.location.href = 'login.html';     // chuyển về trang login
}

// Kiểm tra đã đăng nhập chưa - nếu chưa thì chuyển về login
function checkAuth() {
    const token = getAuthToken();
    // Nếu không có token VÀ không phải trang login → chuyển về login
    if (!token && !window.location.pathname.includes('login.html')) {
        window.location.href = 'login.html';
    }
}

// ========================================
// TABLE FUNCTIONS - Các hàm xử lý bảng dữ liệu
// ========================================

// Tạo 1 hàng (row) cho bảng - hàm này để tái sử dụng
function createTableRow(data, columns, actions) {
    const tr = document.createElement('tr');  // tạo thẻ <tr>
    
    // Duyệt qua từng cột
    columns.forEach(col => {
        const td = document.createElement('td');  // tạo thẻ <td>
        
        // Nếu cột có hàm render riêng (ví dụ: format tiền, badge màu) thì dùng
        if (col.render) {
            td.innerHTML = col.render(data);
        } else {
            // Không thì chỉ hiển thị text thuần
            td.textContent = data[col.field] || '';
        }
        
        tr.appendChild(td);  // thêm <td> vào <tr>
    });
    
    // Thêm cột "Thao tác" (nút Sửa, Xóa)
    if (actions) {
        const td = document.createElement('td');
        td.innerHTML = actions(data);  // actions là hàm trả về HTML nút bấm
        tr.appendChild(td);
    }
    
    return tr;  // trả về <tr> hoàn chỉnh
}

// Render toàn bộ bảng - hàm này dùng ở mọi trang quản lý
function renderTable(tableBodyId, data, columns, actions) {
    const tbody = document.getElementById(tableBodyId);
    if (!tbody) return;  // không tìm thấy tbody thì thoát
    
    tbody.innerHTML = '';  // xóa hết dữ liệu cũ
    
    // Nếu không có dữ liệu thì hiển thị "Không có dữ liệu"
    if (!data || data.length === 0) {
        const tr = document.createElement('tr');
        const td = document.createElement('td');
        td.colSpan = columns.length + (actions ? 1 : 0);  // gộp tất cả cột
        td.textContent = 'Không có dữ liệu';
        td.style.textAlign = 'center';
        td.style.padding = '40px';
        td.style.color = 'var(--text-light)';
        tr.appendChild(td);
        tbody.appendChild(tr);
        return;
    }
    
    // Có dữ liệu thì render từng dòng
    data.forEach(item => {
        const row = createTableRow(item, columns, actions);
        tbody.appendChild(row);  // thêm dòng vào tbody
    });
}

// ========================================
// SEARCH & FILTER - Tìm kiếm trong bảng
// ========================================

// Hàm tìm kiếm real-time - gõ vào ô search là lọc ngay
function searchTable(inputId, tableBodyId) {
    const input = document.getElementById(inputId);    // ô input search
    const tbody = document.getElementById(tableBodyId); // tbody của bảng
    
    if (!input || !tbody) return;  // không tìm thấy thì thoát
    
    
    input.addEventListener('keyup', function() {
        const filter = this.value.toLowerCase();  // chuyển text search về chữ thường
        const rows = tbody.getElementsByTagName('tr');  // lấy tất cả dòng
        
        // Duyệt qua từng dòng
        for (let i = 0; i < rows.length; i++) {
            const row = rows[i];
            const text = row.textContent.toLowerCase();  // nội dung cả dòng
            
            // Nếu dòng có chứa text search → hiển thị, không thì ẩn
            if (text.indexOf(filter) > -1) {
                row.style.display = '';      // hiển thị
            } else {
                row.style.display = 'none';  // ẩn
            }
        }
    });
}

// ========================================
// FORM FUNCTIONS - Các hàm xử lý form
// ========================================

// Lấy dữ liệu từ form thành object
// Ví dụ: {tenKV: 'Tầng 1', moTa: 'Mô tả...'}
function getFormData(formId) {
    const form = document.getElementById(formId);
    if (!form) return null;
    
    const formData = new FormData(form);  // lấy tất cả input trong form
    const data = {};                       // object rỗng để chứa dữ liệu
    
    // Duyệt qua từng cặp key-value
    for (let [key, value] of formData.entries()) {
        data[key] = value;  // gán vào object
    }
    
    return data;  // trả về object
}

// Reset form - xóa hết dữ liệu đã nhập
function resetForm(formId) {
    const form = document.getElementById(formId);
    if (form) {
        form.reset();  // method có sẵn của form
    }
}

// Điền dữ liệu vào form (khi sửa)
function fillForm(formId, data) {
    const form = document.getElementById(formId);
    if (!form) return;
    
    // Duyệt qua từng field trong data
    for (let key in data) {
        const input = form.elements[key];  // tìm input có name=key
        if (input) {
            input.value = data[key];  // gán giá trị vào input
        }
    }
}

// Xác nhận xóa - hiện popup confirm
function confirmDelete(message = 'Bạn có chắc chắn muốn xóa?') {
    return confirm(message);  // confirm là hàm có sẵn của JavaScript
}

// ========================================
// LOADING SPINNER
// ========================================

function showLoading(containerId) {
    const container = document.getElementById(containerId);
    if (container) {
        container.innerHTML = '<div class="spinner"></div>';
    }
}

function hideLoading(containerId) {
    const container = document.getElementById(containerId);
    if (container) {
        container.innerHTML = '';
    }
}

// ========================================
// MENU ACTIVE STATE - Đánh dấu menu đang active
// ========================================

// Hàm tô sáng menu item đang active
function setActiveMenu() {
    const currentPage = window.location.pathname.split('/').pop();  // lấy tên file hiện tại
    const menuItems = document.querySelectorAll('.menu-item a');    // lấy tất cả menu item
    
    menuItems.forEach(item => {
        const href = item.getAttribute('href');  // lấy href của menu item
        if (href === currentPage) {
            item.classList.add('active');  // thêm class 'active' nếu đúng trang
        }
    });
}


// INITIALIZE ON PAGE LOAD - Khởi tạo khi trang load
// Đây là phần quan trọng - chạy đầu tiên khi mở trang


document.addEventListener('DOMContentLoaded', function() {
    // DOMContentLoaded = sự kiện khi HTML đã load xong
    
    // Kiểm tra xác thực (trừ trang login)
    if (!window.location.pathname.includes('login.html')) {
        checkAuth();  // kiểm tra đã đăng nhập chưa
        
        // Render menu theo role (nếu có file roles.js)
        if (typeof renderMenuByRole === 'function') {
            renderMenuByRole();  // render menu theo quyền
        } else {
            setActiveMenu();     // không có phân quyền thì chỉ tô sáng menu
        }
        
        // Kiểm tra quyền truy cập trang (nếu có roles.js)
        if (typeof checkPagePermission === 'function') {
            checkPagePermission();  // kiểm tra có quyền xem trang này không
        }
        
        // Hiển thị thông tin user ở header (tên, vai trò, avatar)
        const userInfo = getUserInfo();
        if (userInfo) {
            const userNameEl = document.getElementById('userName');
            const userRoleEl = document.getElementById('userRole');
            const userAvatarEl = document.getElementById('userAvatar');
            
            // Điền thông tin vào các element
            if (userNameEl) userNameEl.textContent = userInfo.hoTen;
            if (userRoleEl) userRoleEl.textContent = userInfo.tenVaiTro || 'Nhân viên';
            if (userAvatarEl) userAvatarEl.textContent = userInfo.hoTen.charAt(0).toUpperCase();  // chữ cái đầu
        }
    }
});

// ========================================
// EXPORT FUNCTIONS - Xuất dữ liệu (chưa hoàn thiện)
// ========================================

// Export dữ liệu ra Excel - cần thêm thư viện SheetJS (xlsx.js)
function exportToExcel(data, filename = 'export.xlsx') {
    // Placeholder - để sau này bổ sung
    console.log('Export to Excel:', data);
    showAlert('Chức năng export đang được phát triển', 'info');
}

// In dữ liệu - gọi hàm print có sẵn của trình duyệt
function printData() {
    window.print();  // mở dialog in
}

