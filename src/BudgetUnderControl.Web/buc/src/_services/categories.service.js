import config from 'config';
import { authHeader } from '../_helpers';

export const categoriesService = {
    getAll
};

function getAll() {
    const requestOptions = {
        method: 'GET',
        headers: authHeader()
    };

    return fetch(`${config.apiUrl}/categories`, requestOptions)
    .then(handleResponse)
    .then(text => {

       let categories =  JSON.parse(text);

       return categories;
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