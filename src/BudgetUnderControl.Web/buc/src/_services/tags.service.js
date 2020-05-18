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

    return axios.get(`/tags`, { params:{}, headers: authHeader()})
    .then(handleResponse)
    .catch(catchError);
}

function add(tag) {

    return axios.post(`/tags`, tag, { headers: authHeader()})
    .then(handleResponse)
    .catch(catchError);
}

function get(guid) {

    return axios.get(`/tags/${guid}`, { params:{}, headers: authHeader()})
    .then(handleResponse)
.catch(catchError);
}

function edit(guid, tag) {

    return axios.put(`/tags/${guid}`,tag, { params:{}, headers: authHeader()})
    .then(handleResponse)
    .catch(catchError);
}

function remove(guid) {

    return axios.delete(`/tags/${guid}`, { params:{}, headers: authHeader()})
    .then(handleResponse)
    .catch(catchError);
}

