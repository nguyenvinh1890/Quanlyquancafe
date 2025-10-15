// 🔗 Địa chỉ API backend (dùng cho các request sau này)
const API_URL = "https://localhost:44322/api/NguoiDung"; // backend ASP.NET 

document.addEventListener("DOMContentLoaded", () => {
  const loginForm = document.getElementById("loginForm");
  const usernameInput = document.getElementById("username");
  const passwordInput = document.getElementById("password");
  const rememberCheckbox = document.getElementById("rememberMe");

  if (!loginForm) return;

  // 🧠 Khi người dùng submit form đăng nhập
  loginForm.addEventListener("submit", async (e) => {
    e.preventDefault();

    const username = usernameInput.value.trim();
    const password = passwordInput.value.trim();

    if (!username || !password) {
      alert("Vui lòng nhập đầy đủ Tên đăng nhập và Mật khẩu!");
      return;
    }

    // 💡 Giả lập kiểm tra tài khoản
    // (Sau này bạn có thể thay bằng gọi API thật: fetch(`${API_URL}/TaiKhoans/login`, {...}))
    let role = "";

    if (username.toLowerCase() === "admin" && password === "123") {
      role = "admin";
    } else if (username.toLowerCase() === "thungan" && password === "123") {
      role = "cashier";
    } else if (username.toLowerCase() === "phucvu" && password === "123") {
      role = "server";
    } else if (username.toLowerCase() === "bep" && password === "123") {
      role = "kitchen";
    } else if (username.toLowerCase() === "khach" && password === "123") {
      role = "customer";
    } else {
      alert("Tên đăng nhập hoặc mật khẩu không đúng!");
      return;
    }

    // ✅ Lưu role & username vào localStorage
    localStorage.setItem("role", role);
    localStorage.setItem("username", username);

    // ✅ Ghi nhớ đăng nhập nếu được chọn
    if (rememberCheckbox && rememberCheckbox.checked) {
      localStorage.setItem("rememberUser", username);
    } else {
      localStorage.removeItem("rememberUser");
    }

    // ✅ Chuyển trang theo vai trò
    switch (role) {
      case "admin":
        window.location.href = "admin.html";
        break;
      case "cashier":
      case "server":
      case "kitchen":
        window.location.href = "foh.html";
        break;
      case "customer":
        window.location.href = "index.html";
        break;
      default:
        window.location.href = "index.html";
        break;
    }
  });

  // 🔁 Tự động điền lại tên đăng nhập nếu có "Ghi nhớ đăng nhập"
  const rememberedUser = localStorage.getItem("rememberUser");
  if (rememberedUser) {
    usernameInput.value = rememberedUser;
    rememberCheckbox.checked = true;
  }
});
