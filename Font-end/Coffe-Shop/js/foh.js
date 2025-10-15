const API_URL = "https://localhost:44322/api";

fetch(`${API_URL}/Bans`)
  .then(res => res.json())
  .then(data => console.log(data))
  .catch(err => console.error("Lỗi API:", err));

document.addEventListener("DOMContentLoaded", () => {
  const tables = [
    { id: 1, name: "Bàn 1" },
    { id: 2, name: "Bàn 2" },
    { id: 3, name: "Bàn 3" },
    { id: 4, name: "Bàn 4" },
    { id: 5, name: "Bàn 5" },
    { id: 6, name: "Bàn 6" }
  ];

  const menuItems = [
    { id: 1, name: "Cà phê đen", price: 30000 },
    { id: 2, name: "Cà phê sữa", price: 35000 },
    { id: 3, name: "Trà đào cam sả", price: 35000 },
    { id: 4, name: "Trà sữa trân châu", price: 40000 },
    { id: 5, name: "Bạc xỉu", price: 35000 }
  ];

  const tableGrid = document.getElementById("tableGrid");
  const menuGrid = document.getElementById("menuItems");
  const orderList = document.getElementById("orderList");

  let selectedTable = null;
  let order = [];

  // Render bàn
  tables.forEach(table => {
    const div = document.createElement("div");
    div.className = "table";
    div.textContent = table.name;
    div.addEventListener("click", () => {
      document.querySelectorAll(".table").forEach(t => t.classList.remove("active"));
      div.classList.add("active");
      selectedTable = table;
      order = [];
      renderOrder();
    });
    tableGrid.appendChild(div);
  });

  // Render menu
  menuItems.forEach(item => {
    const div = document.createElement("div");
    div.className = "menu-item";
    div.innerHTML = `<strong>${item.name}</strong><br>${item.price.toLocaleString()}đ`;
    div.addEventListener("click", () => {
      if (!selectedTable) {
        alert("Chọn bàn trước khi order!");
        return;
      }
      order.push(item);
      renderOrder();
    });
    menuGrid.appendChild(div);
  });

  // Render order
  function renderOrder() {
    orderList.innerHTML = "";
    order.forEach((item, index) => {
      const li = document.createElement("li");
      li.textContent = `${item.name} - ${item.price.toLocaleString()}đ`;
      orderList.appendChild(li);
    });
  }

  document.getElementById("sendToKitchen").addEventListener("click", () => {
    if (!selectedTable || order.length === 0) {
      alert("Chưa chọn bàn hoặc order trống!");
      return;
    }
    alert(`Đã gửi ${order.length} món của ${selectedTable.name} xuống bếp.`);
    order = [];
    renderOrder();
  });

  document.getElementById("logoutBtn").addEventListener("click", () => {
    localStorage.removeItem("role");
    window.location.href = "login.html";
  });

  document.getElementById("payBtn").addEventListener("click", () => {
    if (order.length === 0) {
      alert("Không có món nào để thanh toán!");
      return;
    }
    const total = order.reduce((sum, i) => sum + i.price, 0);
    alert(`Tổng hóa đơn: ${total.toLocaleString()}đ\nĐã thanh toán!`);
    order = [];
    renderOrder();
  });
});
// === Hiển thị menu tài khoản ===
document.addEventListener("DOMContentLoaded", () => {
  const accountBtn = document.getElementById("accountBtn");
  const accountMenu = document.getElementById("accountMenu");
  const accountRole = document.getElementById("accountRole");
  const accountFunctions = document.getElementById("accountFunctions");
  const logoutBtn = document.getElementById("logoutBtn");

  const role = localStorage.getItem("role") || "guest";

  // Danh sách chức năng theo vai trò
  const roleData = {
    admin: {
      label: "Admin",
      functions: [
        "Quản lý nhân viên",
        "Quản lý menu / món",
        "Báo cáo doanh thu",
        "Thống kê món bán chạy"
      ]
    },
    cashier: {
      label: "Thu ngân",
      functions: [
        "Quản lý order",
        "Tách / ghép hóa đơn",
        "Xử lý thanh toán"
      ]
    },
    server: {
      label: "Phục vụ",
      functions: [
        "Chọn bàn / order món",
        "Chuyển bàn / ghép bàn",
        "Gửi order xuống bếp"
      ]
    },
    kitchen: {
      label: "Bếp / Pha chế",
      functions: [
        "Nhận ticket món",
        "Cập nhật trạng thái hoàn thành",
        "Báo nguyên liệu thiếu"
      ]
    },
    customer: {
      label: "Khách hàng",
      functions: [
        "Xem menu",
        "Thêm vào giỏ hàng",
        "Đặt món / Thanh toán online"
      ]
    },
    guest: {
      label: "Khách chưa đăng nhập",
      functions: ["Đăng nhập để sử dụng các chức năng"]
    }
  };

  const current = roleData[role] || roleData.guest;
  accountRole.textContent = `Tài khoản: ${current.label}`;
  accountFunctions.innerHTML = current.functions
    .map(f => `<li>${f}</li>`)
    .join("");

  // Toggle menu hiển thị
  accountBtn.addEventListener("click", (e) => {
    e.preventDefault();
    accountMenu.classList.toggle("hidden");
  });

  // Đăng xuất
  logoutBtn.addEventListener("click", () => {
    localStorage.removeItem("role");
    window.location.href = "login.html";
  });

  // Ẩn menu khi click ra ngoài
  document.addEventListener("click", (e) => {
    if (!accountBtn.contains(e.target) && !accountMenu.contains(e.target)) {
      accountMenu.classList.add("hidden");
    }
  });
});
