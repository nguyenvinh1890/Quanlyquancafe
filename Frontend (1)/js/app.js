// ========================================
// QUẢN LÝ QUÁN CAFE - JAVASCRIPT
// ========================================

// API Base URL - Thay đổi port nếu cần
const API_BASE_URL = 'https://localhost:7000/api'; // Điều chỉnh theo port của bạn

// ========================================
// UTILITY FUNCTIONS
// ========================================

// Hiển thị thông báo
function showAlert(message, type = 'info') {
    const alertContainer = document.getElementById('alertContainer');
    if (!alertContainer) return;

    const alert = document.createElement('div');
    alert.className = `alert alert-${type}`;
    alert.innerHTML = message;
    
    alertContainer.appendChild(alert);
    
    setTimeout(() => {
        alert.remove();
    }, 5000);
}

// Format tiền tệ VND
function formatCurrency(amount) {
    return new Intl.NumberFormat('vi-VN', {
        style: 'currency',
        currency: 'VND'
    }).format(amount);
}

// Format ngày giờ
function formatDateTime(dateString) {
    const date = new Date(dateString);
    return date.toLocaleString('vi-VN');
}

// Format ngày
function formatDate(dateString) {
    const date = new Date(dateString);
    return date.toLocaleDateString('vi-VN');
}

// Format giờ
function formatTime(timeString) {
    if (!timeString) return '';
    return timeString.substring(0, 5); // HH:mm
}

// ========================================
// MODAL FUNCTIONS
// ========================================

function openModal(modalId) {
    const modal = document.getElementById(modalId);
    if (modal) {
        modal.classList.add('active');
    }
}

function closeModal(modalId) {
    const modal = document.getElementById(modalId);
    if (modal) {
        modal.classList.remove('active');
    }
}

// Đóng modal khi click bên ngoài
window.onclick = function(event) {
    if (event.target.classList.contains('modal')) {
        event.target.classList.remove('active');
    }
}

// ========================================
// API CALL FUNCTIONS
// ========================================

// GET request
async function apiGet(endpoint) {
    try {
        const response = await fetch(`${API_BASE_URL}${endpoint}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${getAuthToken()}`
            }
        });
        
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        
        return await response.json();
    } catch (error) {
        console.error('API GET Error:', error);
        showAlert('Lỗi khi tải dữ liệu: ' + error.message, 'danger');
        return null;
    }
}

// POST request
async function apiPost(endpoint, data) {
    try {
        const response = await fetch(`${API_BASE_URL}${endpoint}`, {
            method: 'POST',
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
        console.error('API POST Error:', error);
        showAlert('Lỗi khi thêm dữ liệu: ' + error.message, 'danger');
        return null;
    }
}

// PUT request
async function apiPut(endpoint, data) {
    try {
        const response = await fetch(`${API_BASE_URL}${endpoint}`, {
            method: 'PUT',
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

// DELETE request
async function apiDelete(endpoint) {
    try {
        const response = await fetch(`${API_BASE_URL}${endpoint}`, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${getAuthToken()}`
            }
        });
        
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        
        return true;
    } catch (error) {
        console.error('API DELETE Error:', error);
        showAlert('Lỗi khi xóa dữ liệu: ' + error.message, 'danger');
        return false;
    }
}

// ========================================
// AUTHENTICATION
// ========================================

function getAuthToken() {
    return localStorage.getItem('authToken') || '';
}

function setAuthToken(token) {
    localStorage.setItem('authToken', token);
}

function getUserInfo() {
    const userInfo = localStorage.getItem('userInfo');
    return userInfo ? JSON.parse(userInfo) : null;
}

function setUserInfo(userInfo) {
    localStorage.setItem('userInfo', JSON.stringify(userInfo));
}

function logout() {
    localStorage.removeItem('authToken');
    localStorage.removeItem('userInfo');
    window.location.href = 'login.html';
}

// Kiểm tra đăng nhập
function checkAuth() {
    const token = getAuthToken();
    if (!token && !window.location.pathname.includes('login.html')) {
        window.location.href = 'login.html';
    }
}

// ========================================
// TABLE FUNCTIONS
// ========================================

// Tạo hàng cho bảng
function createTableRow(data, columns, actions) {
    const tr = document.createElement('tr');
    
    columns.forEach(col => {
        const td = document.createElement('td');
        
        if (col.render) {
            td.innerHTML = col.render(data);
        } else {
            td.textContent = data[col.field] || '';
        }
        
        tr.appendChild(td);
    });
    
    // Thêm cột actions
    if (actions) {
        const td = document.createElement('td');
        td.innerHTML = actions(data);
        tr.appendChild(td);
    }
    
    return tr;
}

// Render bảng
function renderTable(tableBodyId, data, columns, actions) {
    const tbody = document.getElementById(tableBodyId);
    if (!tbody) return;
    
    tbody.innerHTML = '';
    
    if (!data || data.length === 0) {
        const tr = document.createElement('tr');
        const td = document.createElement('td');
        td.colSpan = columns.length + (actions ? 1 : 0);
        td.textContent = 'Không có dữ liệu';
        td.style.textAlign = 'center';
        td.style.padding = '40px';
        td.style.color = 'var(--text-light)';
        tr.appendChild(td);
        tbody.appendChild(tr);
        return;
    }
    
    data.forEach(item => {
        const row = createTableRow(item, columns, actions);
        tbody.appendChild(row);
    });
}

// ========================================
// SEARCH & FILTER
// ========================================

function searchTable(inputId, tableBodyId) {
    const input = document.getElementById(inputId);
    const tbody = document.getElementById(tableBodyId);
    
    if (!input || !tbody) return;
    
    input.addEventListener('keyup', function() {
        const filter = this.value.toLowerCase();
        const rows = tbody.getElementsByTagName('tr');
        
        for (let i = 0; i < rows.length; i++) {
            const row = rows[i];
            const text = row.textContent.toLowerCase();
            
            if (text.indexOf(filter) > -1) {
                row.style.display = '';
            } else {
                row.style.display = 'none';
            }
        }
    });
}

// ========================================
// FORM FUNCTIONS
// ========================================

// Lấy dữ liệu từ form
function getFormData(formId) {
    const form = document.getElementById(formId);
    if (!form) return null;
    
    const formData = new FormData(form);
    const data = {};
    
    for (let [key, value] of formData.entries()) {
        data[key] = value;
    }
    
    return data;
}

// Reset form
function resetForm(formId) {
    const form = document.getElementById(formId);
    if (form) {
        form.reset();
    }
}

// Điền dữ liệu vào form
function fillForm(formId, data) {
    const form = document.getElementById(formId);
    if (!form) return;
    
    for (let key in data) {
        const input = form.elements[key];
        if (input) {
            input.value = data[key];
        }
    }
}

// ========================================
// CONFIRM DELETE
// ========================================

function confirmDelete(message = 'Bạn có chắc chắn muốn xóa?') {
    return confirm(message);
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
// MENU ACTIVE STATE
// ========================================

function setActiveMenu() {
    const currentPage = window.location.pathname.split('/').pop();
    const menuItems = document.querySelectorAll('.menu-item a');
    
    menuItems.forEach(item => {
        const href = item.getAttribute('href');
        if (href === currentPage) {
            item.classList.add('active');
        }
    });
}

// ========================================
// INITIALIZE ON PAGE LOAD
// ========================================

document.addEventListener('DOMContentLoaded', function() {
    // Check authentication (trừ trang login)
    if (!window.location.pathname.includes('login.html')) {
        checkAuth();
        
        // Render menu theo role (nếu có roles.js)
        if (typeof renderMenuByRole === 'function') {
            renderMenuByRole();
        } else {
            setActiveMenu();
        }
        
        // Kiểm tra quyền truy cập trang (nếu có roles.js)
        if (typeof checkPagePermission === 'function') {
            checkPagePermission();
        }
        
        // Hiển thị thông tin user
        const userInfo = getUserInfo();
        if (userInfo) {
            const userNameEl = document.getElementById('userName');
            const userRoleEl = document.getElementById('userRole');
            const userAvatarEl = document.getElementById('userAvatar');
            
            if (userNameEl) userNameEl.textContent = userInfo.hoTen;
            if (userRoleEl) userRoleEl.textContent = userInfo.tenVaiTro || 'Nhân viên';
            if (userAvatarEl) userAvatarEl.textContent = userInfo.hoTen.charAt(0).toUpperCase();
        }
    }
});

// ========================================
// EXPORT FUNCTIONS
// ========================================

// Export dữ liệu ra Excel (sử dụng SheetJS - cần thêm thư viện)
function exportToExcel(data, filename = 'export.xlsx') {
    // Placeholder - cần thêm thư viện SheetJS
    console.log('Export to Excel:', data);
    showAlert('Chức năng export đang được phát triển', 'info');
}

// In dữ liệu
function printData() {
    window.print();
}

