// 
// QUẢN LÝ VAI TRÒ VÀ PHÂN QUYỀN
// 
// Định nghĩa quyền cho từng vai trò
const ROLES = {
    QUAN_LY: {
        id: 1,
        name: 'Quản lý',
        permissions: [
            'dashboard',
            'khu-vuc',
            'ban',
            'mon',
            'size',
            'topping',
            'don-hang',
            'hoa-don',
            'thanh-toan',
            'nguoi-dung',
            'ca-lam',
            'vai-tro'
        ]
    },
    THU_NGAN: {
        id: 2,
        name: 'Thu ngân',
        permissions: [
            'dashboard',
            'ban',
            'mon',
            'don-hang',
            'hoa-don',
            'thanh-toan'
        ]
    },
    PHUC_VU: {
        id: 3,
        name: 'Phục vụ',
        permissions: [
            'dashboard',
            'ban',
            'mon',
            'don-hang'
        ]
    },
    PHA_CHE: {
        id: 4,
        name: 'Pha chế',
        permissions: [
            'dashboard',
            'don-hang',
            'mon',
            'size',
            'topping'
        ]
    }
};

// Menu configuration với quyền truy cập
const MENU_ITEMS = [
    {
        id: 'dashboard',
        icon: '🏠',
        text: 'Dashboard',
        href: 'index.html',
        permission: 'dashboard'
    },
    {
        id: 'khu-vuc',
        icon: '📍',
        text: 'Khu vực',
        href: 'khu-vuc.html',
        permission: 'khu-vuc'
    },
    {
        id: 'ban',
        icon: '🪑',
        text: 'Bàn',
        href: 'ban.html',
        permission: 'ban'
    },
    {
        id: 'mon',
        icon: '☕',
        text: 'Món',
        href: 'mon.html',
        permission: 'mon'
    },
    {
        id: 'size',
        icon: '📏',
        text: 'Size món',
        href: 'size.html',
        permission: 'size'
    },
    {
        id: 'topping',
        icon: '🧊',
        text: 'Topping',
        href: 'topping.html',
        permission: 'topping'
    },
    {
        id: 'don-hang',
        icon: '📋',
        text: 'Đơn hàng',
        href: 'don-hang.html',
        permission: 'don-hang'
    },
    {
        id: 'hoa-don',
        icon: '🧾',
        text: 'Hóa đơn',
        href: 'hoa-don.html',
        permission: 'hoa-don'
    },
    {
        id: 'thanh-toan',
        icon: '💰',
        text: 'Thanh toán',
        href: 'thanh-toan.html',
        permission: 'thanh-toan'
    },
    {
        id: 'nguoi-dung',
        icon: '👥',
        text: 'Nhân viên',
        href: 'nguoi-dung.html',
        permission: 'nguoi-dung'
    },
    {
        id: 'ca-lam',
        icon: '🕐',
        text: 'Ca làm',
        href: 'calam.html',
        permission: 'ca-lam'
    },
    {
        id: 'vai-tro',
        icon: '🔑',
        text: 'Vai trò',
        href: 'vai-tro.html',
        permission: 'vai-tro'
    }
];

// Kiểm tra quyền truy cập
function hasPermission(permission) {
    const userInfo = getUserInfo();
    if (!userInfo || !userInfo.maVT) {
        return false;
    }

    // Tìm role object dựa vào maVT
    const role = Object.values(ROLES).find(r => r.id === parseInt(userInfo.maVT));
    if (!role) {
        return false;
    }

    return role.permissions.includes(permission);
}

// Lấy tất cả quyền của user hiện tại
function getUserPermissions() {
    const userInfo = getUserInfo();
    if (!userInfo || !userInfo.maVT) {
        return [];
    }

    const role = Object.values(ROLES).find(r => r.id === parseInt(userInfo.maVT));
    return role ? role.permissions : [];
}

// Render menu theo quyền
function renderMenuByRole() {
    const menuContainer = document.querySelector('.sidebar-menu');
    if (!menuContainer) return;

    const userPermissions = getUserPermissions();

    // Clear menu hiện tại
    menuContainer.innerHTML = '';

    // Render menu items có quyền
    MENU_ITEMS.forEach(item => {
        if (userPermissions.includes(item.permission)) {
            const li = document.createElement('li');
            li.className = 'menu-item';
            li.innerHTML = `
                <a href="${item.href}">
                    <span class="icon">${item.icon}</span>
                    <span>${item.text}</span>
                </a>
            `;
            menuContainer.appendChild(li);
        }
    });

    // Thêm nút đăng xuất (luôn hiển thị)
    const logoutLi = document.createElement('li');
    logoutLi.className = 'menu-item';
    logoutLi.innerHTML = `
        <a href="#" onclick="logout(); return false;">
            <span class="icon">🚪</span>
            <span>Đăng xuất</span>
        </a>
    `;
    menuContainer.appendChild(logoutLi);

    // Set active menu
    setActiveMenu();
}

// Kiểm tra quyền truy cập trang hiện tại
function checkPagePermission() {
    const currentPage = window.location.pathname.split('/').pop();

    // Trang login không cần check
    if (currentPage === 'login.html' || currentPage === '') {
        return true;
    }

    // Tìm menu item của trang hiện tại
    const pageWithoutExt = currentPage.replace('.html', '');
    const menuItem = MENU_ITEMS.find(item => item.href === currentPage);

    if (!menuItem) {
        // Nếu không tìm thấy trong menu, cho phép truy cập (có thể là trang đặc biệt)
        return true;
    }

    // Kiểm tra quyền
    if (!hasPermission(menuItem.permission)) {
        // Không có quyền - chuyển về dashboard hoặc trang đầu tiên có quyền
        showAlert('Bạn không có quyền truy cập trang này!', 'danger');
        setTimeout(() => {
            const firstAllowedPage = MENU_ITEMS.find(item => hasPermission(item.permission));
            if (firstAllowedPage) {
                window.location.href = firstAllowedPage.href;
            } else {
                logout();
            }
        }, 1500);
        return false;
    }

    return true;
}

// Lấy role object từ ID
function getRoleById(roleId) {
    return Object.values(ROLES).find(r => r.id === parseInt(roleId));
}

// Lấy role name từ ID
function getRoleName(roleId) {
    const role = getRoleById(roleId);
    return role ? role.name : 'Không xác định';
}

