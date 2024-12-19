//Verify token, if invalid redirect to index
const token = localStorage.getItem("jwtToken");
if (token) {
    const decodedToken = JSON.parse(atob(token.split('.')[1]));
    const exp = decodedToken.exp * 1000;

    const jwtUserId = decodedToken.userId;
    localStorage.setItem("userId", jwtUserId);

    const jwtUserName = decodedToken.userName;
    localStorage.setItem("userName", jwtUserName);
    document.getElementById('display-username').innerText = jwtUserName;

    if (Date.now() >= exp) {    //if expired
        document.location.href = _CONFIG_indexUrl;
    }
} else {
    document.location.href = _CONFIG_indexUrl;
}

const userPicture = document.getElementById('user-picture');
userPicture.src = `${_CONFIG_ApiUsers}/${localStorage.getItem("userId")}/Picture`;

document.getElementById("logout-button").addEventListener("click", () =>  {
    localStorage.clear();
    document.location.href = _CONFIG_indexUrl 
});

//picture upload field editinimas
const inputUploadFileBtn = document.getElementById("upload-picture-button");
const inputFieldFile = document.getElementById("input-picture");
inputFieldFile.addEventListener("change", () => {
    if (inputFieldFile.files.length > 0) {
        inputUploadFileBtn.disabled = false;
    } else {
        inputUploadFileBtn.disabled = true;
    }
})

function enableEditing(inputId) {
    const inputField = document.getElementById(inputId);
    const saveButton = inputField.nextElementSibling.nextElementSibling;

    inputField.disabled = false;
    saveButton.disabled = false;
}

function enableEditingPassword(passId1, passId2) {  //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    const passwordField = document.getElementById(passId1);
    const passwordFieldRe = document.getElementById(passId2);
    const saveButton = passwordFieldRe.nextElementSibling.nextElementSibling;

    passwordField.disabled = false;
    passwordFieldRe.disabled = false;
    saveButton.disabled = false;
}

async function saveField(inputId, endpoint) {
    const inputField = document.getElementById(inputId);
    const saveButton = inputField.nextElementSibling.nextElementSibling;
    const fieldValue = inputField.value;

    //Connection string
    const connectionString = `${_CONFIG_ApiUsers}/${localStorage.getItem("userId")}/${endpoint}`;

    if (!fieldValue.trim()) {
        alert("Field cannot be empty.");
        return;
    }
    
    try {
        const response = await fetch(connectionString, {
            method: "PUT",
            headers: {
                "Authorization": `Bearer ${token}`,
                "Content-Type": "application/json"
            },
            body: JSON.stringify(fieldValue)
        });
        
        if (response.ok) {
            inputField.disabled = true;
            saveButton.disabled = true;
        } else {
            const errorData = await response.json();
            alert(`Error: ${errorData.message || "Failed to save field"}`);
        }
    } catch (error) {
        alert(`Error: ${error.message}`);
    }
}

//Soriukas, tikrai ne mano rasytas metodas... copy paste
function resizeImage(file, maxWidth, maxHeight) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.onload = function (e) {
            const img = new Image();
            img.onload = function () {
                const canvas = document.createElement("canvas");
                const ctx = canvas.getContext("2d");

                let width = img.width;
                let height = img.height;

                if (width > maxWidth || height > maxHeight) {
                    if (width > height) {
                        height = height * (maxWidth / width);
                        width = maxWidth;
                    } else {
                        width = width * (maxHeight / height);
                        height = maxHeight;
                    }
                }

                canvas.width = width;
                canvas.height = height;
                ctx.drawImage(img, 0, 0, width, height);
                canvas.toBlob((blob) => resolve(blob), "image/jpeg", 0.8); // kokybe 80%
            };
            img.src = e.target.result;
        };
        reader.onerror = reject;
        reader.readAsDataURL(file);
    });
}

async function uploadPicture(inputId, endpoint) {
    const formData = new FormData();
    const picture = document.getElementById(inputId).files[0];
    if (!picture) {
        return;
    }
    
    if (picture && picture instanceof File) {
        const resizedBlob = await resizeImage(picture, 200, 200);
        formData.append('File', resizedBlob, "file.jpg");
    } else {
        return;
    }

    const fetchUrlStr = `${_CONFIG_ApiUsers}/${localStorage.getItem("userId")}/${endpoint}`;      ///PAKEISTI I ACTUAL ID!!!!!!!!!!!!!!!!!!!!!!!!!!
    try {
        const response = await fetch(fetchUrlStr, {
            method: "PUT",
            headers: {
                'Authorization': `Bearer ${token}`
            },
            body: formData // Build the FormData with user data and file
        });

        if (response.ok) {
            const data = await response.json(); // Get the JSON response body
            console.log('Success:', data);

            //Catche busteris su timestamp. Api neskaito timestampo
            const timestamp = new Date().getTime();
            userPicture.src = `${_CONFIG_ApiUsers}/${localStorage.getItem("userId")}/Picture?t=${timestamp}`;       
        } else {
            const errorData = await response.json();
            //console.log('Error:', errorData);
        }
    }
    catch (err) {
        //console.log('Error during request:', err.message);
    }

}

async function savePassword(passId1, passId2, endpoint) {
    const passwordField = document.getElementById(passId1);
    const passwordFieldRe = document.getElementById(passId2);

    if(passwordField.value !== passwordFieldRe.value) {
        alert("Passwords do not match!");
        passwordFieldRe.value = "";
        return;
    } else {
        const connectionString = `${_CONFIG_ApiUsers}/${localStorage.getItem("userId")}/${endpoint}`;   

        if (!passwordField.value.trim() || !passwordFieldRe.value.trim() || passwordField.value.length <= 1 || passwordFieldRe.value.length <= 1) {
            alert("Field cannot be empty.");
            return;
        }
    
        try {
            const response = await fetch(connectionString, {
                method: "PUT",
                headers: {
                    'Authorization': `Bearer ${token}`,
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(passwordField.value)
            });
            
            if (response.ok) {
                //alert("Field saved successfully!");
                passwordField.disabled = true;
                passwordFieldRe.disabled = true;
            } else {
                const errorData = await response.json();
                alert(`Error: ${errorData.message || "Failed to save field"}`);
            }
        } catch (error) {
            
        }
    }


        
}