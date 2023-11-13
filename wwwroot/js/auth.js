export function Login(username, password, redirect) {
    document.getElementById("validationMessage").style.display = "none";

    var url = "/api/auth/signin";
    var xhr = new XMLHttpRequest();

    xhr.open("POST", url);
    xhr.setRequestHeader("Accept", "application/json");
    xhr.setRequestHeader("Content-Type", "application/json");

    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && this.status == 200) // 4 = DONE - Good response
        {
            console.log("Call '" + url + "'. Status " + xhr.status);
            if (redirect)
                location.replace(redirect);
        } else if (xhr.readyState == 4 && this.status == 400) {
            document.getElementById("validationMessage").style.display = "block";
            document.getElementById("validationMessage").innerHTML = "Invalid credentials! Please check your username and password and try again.";
        } else {
            document.getElementById("validationMessage").style.display = "block";
            document.getElementById("validationMessage").innerHTML = "An unknown error has occured: { " + this.responseText;
        }
    };

    var data = {
        username: username,
        password: password
    };

    xhr.send(JSON.stringify(data));
}

export function Logout(redirect) {
    var url = "/api/auth/signout";
    var xhr = new XMLHttpRequest();

    xhr.open("POST", url);
    xhr.setRequestHeader("Accept", "application/json");
    xhr.setRequestHeader("Content-Type", "application/json");

    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4) // 4=DONE
        {
            console.log("Call '" + url + "' Status " + xhr.status);
            if (redirect)
                location.replace(redirect);
        }
    };

    xhr.send();
}