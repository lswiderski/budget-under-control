import { authHeader } from '../_helpers';
import { handleResponse } from '../_helpers';
import { catchError } from '../_helpers';
import axios from 'axios';
import Configuration from '../_helpers/configuration';

export const tagsService = {
    getAll,
    add,
    get,
    edit,
    remove
};

function getAll() {

    return axios.get(`${Configuration.value('backendHost')}/tags`, { params:{}, headers: authHeader()})
    .then(handleResponse)
    .catch(catchError);
}

function add(tag) {

    return axios.post(`${Configuration.value('backendHost')}/tags`, tag, { headers: authHeader()})
    .then(handleResponse)
    .catch(catchError);
}

function get(guid) {

    return axios.get(`${Configuration.value('backendHost')}/tags/${guid}`, { params:{}, headers: authHeader()})
    .then(handleResponse)
.catch(catchError);
}

function edit(guid, tag) {

    return axios.put(`${Configuration.value('backendHost')}/tags/${guid}`,tag, { params:{}, headers: authHeader()})
    .then(handleResponse)
    .catch(catchError);
}

function remove(guid) {

    return axios.delete(`${Configuration.value('backendHost')}/tags/${guid}`, { params:{}, headers: authHeader()})
    .then(handleResponse)
    .catch(catchError);
}

