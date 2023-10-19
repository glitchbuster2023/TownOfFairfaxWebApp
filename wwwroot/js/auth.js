export function Login(username, password, redirect) {
    var url = "/api/auth/signin";
    var xhr = new XMLHttpRequest();

    xhr.open("POST", url);
    xhr.setRequestHeader("Accept", "application/json");
    xhr.setRequestHeader("Content-Type", "application/json");

    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4) // 4 = DONE
        {
            console.log("Call '" + url + "'. Status " + xhr.status);
            if (redirect)
                location.replace(redirect);
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