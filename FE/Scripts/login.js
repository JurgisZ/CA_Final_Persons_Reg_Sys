let loginForm = document.getElementById("login-form");
//loginForm.addEventListener("submit", onRegisterSubmit);

let submitBtn = document.getElementById("input-submit");
submitBtn.addEventListener("click", onRegisterSubmit)


async function onRegisterSubmit() {
    let username = document.getElementById("input-username").value;
    let password = document.getElementById("input-password").value;
    let fetchUrlStr = _CONFIG_ApiLogin;
    
    console.log(_CONFIG_ApiLogin);
    try {
        const response = await fetch(fetchUrlStr, {
            method: "POST",
            headers: {
                "content-type": "application/json"
            },
            body: JSON.stringify(
                {
                    UserName: username,
                    Password: password
                }
            )

        });

        console.log(`Response: ${response.status}`);

        if(response.ok) {
            const data = await response.json();
            //set token and id
            localStorage.setItem('jwtToken', data.token);
            localStorage.setItem('userId' , data.id);
            document.location.href = _CONFIG_userHomeUrl;

        }
    }
    catch(err) {
        console.log(err.message);
    }
    
}


let registerBtn = document.getElementById("register-button");
registerBtn.addEventListener("click", () => document.location.href = _CONFIG_registerUrl);

