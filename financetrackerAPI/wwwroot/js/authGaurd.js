const token = localStorage.getItem("token");

if (!token) {
    alert("Please log in or register your account");
    window.location.href = "/AuthPage/Login";
}