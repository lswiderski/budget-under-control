import config from 'config';
import { authHeader } from '../_helpers';
import { handleResponse } from '../_helpers';
import { catchError } from '../_helpers';
import axios from 'axios';

export const tagsService = {
    getAll,
    add,
    get,
    edit,
    remove
};

function getAll() {

    return axios.get(`${config.apiUrl}/tags`, { params:{}, headers: authHeader()})
    .then(handleResponse)
    .catch(catchError);
}

function add(tag) {

    return axios.post(`${config.apiUrl}/tags`, tag, { headers: authHeader()})
    .then(handleResponse)
    .catch(catchError);
}

function get(guid) {

    return axios.get(`${config.apiUrl}/tags/${guid}`, { params:{}, headers: authHeader()})
    .then(handleResponse)
.catch(catchError);
}

function edit(guid, tag) {

    return axios.put(`${config.apiUrl}/tags/${guid}`,tag, { params:{}, headers: authHeader()})
    .then(handleResponse)
    .catch(catchError);
}

function remove(guid) {

    return axios.delete(`${config.apiUrl}/tags/${guid}`, { params:{}, headers: authHeader()})
    .then(handleResponse)
    .catch(catchError);
}

