
export function handleResponse(response) {
    if(response.status !== 200)
    {
        if (response.status === 401) {
            // auto logout if 401 response returned from api
            logout();
            location.reload(true);
        }
        const error = (data && data.message) || response.statusText;
            return Promise.reject(error);
    }
    return response.data;
}