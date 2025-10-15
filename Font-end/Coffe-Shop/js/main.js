document.addEventListener("DOMContentLoaded", () => {
  const accountBtn = document.getElementById("accountBtn");
  const accountMenu = document.getElementById("accountMenu");
  const accountRole = document.getElementById("accountRole");
  const accountFunctions = document.getElementById("accountFunctions");
  const logoutBtn = document.getElementById("logoutBtn");

  if (!accountBtn || !accountMenu) return;

  const role = localStorage.getItem("role") || "guest";

  // 💡 Phân quyền hiển thị theo biểu đồ Use Case bạn gửi
  const roleData = {
    admin: {
      label: "Quản trị viên (Admin)",
      functions: [
        "Quản lý nhân viên",
        "Quản lý menu",
        "Quản lý nhà cung cấp",
        "Quản lý order",
        "Thống kê báo cáo"
      ]
    },
    cashier: {
      label: "Thu ngân",
      functions: [
        "Quản lý bán hàng",
        "Thanh toán",
        "Xử lý khuyến mãi"
      ]
    },
    server: {
      label: "Phục vụ",
      functions: [
        "Sắp xếp, dọn bàn"
      ]
    },
    kitchen: {
      label: "Nhân viên bếp",
      functions: [
        "Xem ticket",
        "Cập nhật món"
      ]
    },
    customer: {
      label: "Khách hàng",
      functions: [
        "Xem menu",
        "Đặt bàn"
      ]
    },
    guest: {
      label: "Khách chưa đăng nhập",
      functions: ["Đăng nhập để sử dụng chức năng"]
    }
  };

  // Lấy thông tin theo vai trò
  const current = roleData[role] || roleData.guest;
  accountRole.textContent = `Tài khoản: ${current.label}`;
  accountFunctions.innerHTML = current.functions.map(f => `<li>${f}</li>`).join("");

  // Hiển thị/ẩn menu khi click icon 👤
  accountBtn.addEventListener("click", (e) => {
    e.preventDefault();
    accountMenu.classList.toggle("hidden");
  });

  // Ẩn menu khi click ra ngoài vùng tài khoản
  document.addEventListener("click", (e) => {
    if (!accountBtn.contains(e.target) && !accountMenu.contains(e.target)) {
      accountMenu.classList.add("hidden");
    }
  });

  // Đăng xuất
  if (logoutBtn) {
    logoutBtn.addEventListener("click", () => {
      localStorage.removeItem("role");
      localStorage.removeItem("username");
      window.location.href = "login.html";
    });
  }
});
