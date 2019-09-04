import config from 'config';
import { authHeader } from '../_helpers';

export const userService = {
    login,
    logout,
    getAll
};

function login(username, password) {
    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ username, password })
    };

    return fetch(`${config.apiUrl}/Login/Authenticate`, requestOptions)
        .then(handleResponse)
        .then(token=> {
            // login successful if there's a jwt token in the response
            debugger;
            if (token) {
                // store user details and jwt token in local storage to keep user logged in between page refreshes
                localStorage.setItem('jwt-token', JSON.stringify(token));
            }

            return token;
        });
}

function logout() {
    // remove user from local storage to log user out
    localStorage.removeItem('jwt-token');
}

function getAll() {
    const requestOptions = {
        method: 'GET',
        headers: authHeader()
    };

    return fetch(`${config.apiUrl}/accounts`, requestOptions)
    .then(handleResponse)
    .then(text => {

       let accounts =  JSON.parse(text);

       return accounts;
    });
}

function handleResponse(response) {
    return response.text().then(text => {
        const data = text;// && JSON.parse(text);
        if (!response.ok) {
          
            if (response.status === 401) {
                // auto logout if 401 response returned from api
                logout();
                location.reload(true);
            }
            const error = (data && data.message) || response.statusText;
            return Promise.reject(error);
        }

        return data;
    });
}