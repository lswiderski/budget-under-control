export function authHeader() {
    // return authorization header with jwt token
    let token = JSON.parse(localStorage.getItem('jwt-token'));

    if (token) {
        return { 'Authorization': 'Bearer ' + token};
    } else {
        return {};
    }
}