console.log("AUTH GUARD IS RUNNING");

const token = localStorage.getItem("token");

console.log("TOKEN:", token);

if (!token) {
    if (token) {
        console.log("NO TOKEN - REDIRECTING");
        localStorage.removeItem("token");
    } else {
        console.log("INVALID TOKEN - REDIRECTING");
    }
    console.log("NO TOKEN - REDIRECTING");
    alert("Your session has expired, please log in again.");
    window.location.href = "/AuthPage/Login";
}