console.log("AUTH GUARD IS RUNNING");

const token = localStorage.getItem("token");

console.log("TOKEN:", token);

if (!token) {
    console.log("NO TOKEN - REDIRECTING");
    window.location.href = "/AuthPage/Login";
}