document.getElementById("loginform").addEventListener("submit", async function(event) {
    event.preventDefault();

    const email = document.getElementById("login-email").value;
    const password = document.getElementById("login-password").value;
    const errorDiv = document.getElementById("login-error");

    try {
        const response = await fetch("http://localhost:5000/api/auth/login", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ email, password })
        });

        if (!response.ok) {
            throw new Error("Wrong email or password");
        }

        const data = await response.json();

        // Gem JWT-token og redirect
        localStorage.setItem("token", data.token);
        window.location.href = "Index.html"; // redirect til hovedsiden
    } catch (err) {
        errorDiv.textContent = err.message;
    }
});
