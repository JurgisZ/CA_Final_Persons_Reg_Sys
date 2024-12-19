//Account details
const username = document.getElementById("input-username");
const password = document.getElementById("input-password");

const password_re = document.getElementById("input-password-re");

//Personal info
const _name = document.getElementById("input-name");                    //name nera keyword, kodel braukia? - name is depricated
const lastname = document.getElementById("input-lastname");
const personalcode = document.getElementById("input-personal-code");
const phone = document.getElementById("input-phone");
const email = document.getElementById("input-email");
//const picture = document.getElementById("input-picture").files[0];    //file upload - UNDEFINED GI...

//Address details
const city = document.getElementById("input-city-name");
const street = document.getElementById("input-street-name");
const house = document.getElementById("input-house-number");
const apartment = document.getElementById("input-apartment-number");

//Return to Index
const homeBtn = document.getElementById("home-button");
homeBtn.addEventListener("click", () => document.location.href = _CONFIG_indexUrl);

//Submit button
const submitBtn = document.getElementById("submit-button");
submitBtn.addEventListener("click", submitValues);


function BuildUserRequestFormData() {
    const userData =
        {
            "UserName": username.value,
            "Password": password.value,
            "userPersonalDataRequest": {
              "Name": _name.value,
              "LastName": lastname.value,
              "PersonalCode": personalcode.value,
              "PhoneNumber": phone.value,
              "Email": email.value,
              "ProfilePicture": "placeholder",
              "CityName": city.value,
              "StreetName": street.value,
              "HouseNumber": house.value,
              "ApartmentNumber": apartment.value
            }
          }

    const formData = new FormData();
    formData.append('requestStr', JSON.stringify(userData));

    const picture = document.getElementById("input-picture").files[0];

    if (!picture) {
        return; //handlinimas
    }
    
    if (picture && picture instanceof File) {
        formData.append('File', picture);
    } else {
        console.log("Selected file is not a valid File object.");
    }
    return formData;
}

async function submitValues() {
    const fetchUrlStr = `${_CONFIG_ApiUsers}`;
    try {
        const response = await fetch(fetchUrlStr, {
            method: "POST",
            body: BuildUserRequestFormData() //FormData
        });

        if (response.ok) {
            document.location.href = _CONFIG_indexUrl;
            const data = await response.json();
            console.log('Success:', data);
        } else {
            const errorData = await response.json();
            console.log('Error:', errorData);
        }
    }
    catch (err) {
        console.log('Error during request:', err.message);
    }
}