import config from 'config';
import { authHeader } from '../_helpers';
import { handleResponse } from '../_helpers';
import axios from 'axios';

export const categoriesService = {
    getAll
};

function getAll() {

    return axios.get(`${config.apiUrl}/categories`, { params:{}, headers: authHeader()})
    .then(handleResponse)
    .then(data => {
        let categories =  data;
        return categories;
    }).catch(e => {
        return Promise.reject(e);
      });
}
