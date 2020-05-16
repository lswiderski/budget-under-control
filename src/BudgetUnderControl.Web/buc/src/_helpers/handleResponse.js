
export function handleResponse(response) {
    if (response.status !== 200 && response.status !== 204 && response.status !== 201) {
        if (response.status === 401) {
            // auto logout if 401 response returned from api
            logout();
            location.reload(true);
        }
        const error = (response && response.message) || response.statusText;
        return Promise.reject(error);
    }
    return response.data;
}

export function catchError(e) {
    if (e.response.status === 401) {
        // auto logout if 401 response returned from api
        logout();
        location.reload(true);

    }
    if (e.response.status === 400) {
        let errors = [];
        var obj = e.response.data;
        Object.getOwnPropertyNames(obj).forEach(function (val) {
            errors.push(val + ' -> ' + obj[val].join("; "))
        });

        return Promise.reject(errors);
    }
    const error = (e && e.message) || e.statusText;
    return Promise.reject(error);
}

function logout() {
    // remove user from local storage to log user out
    localStorage.removeItem('jwt-token');
}