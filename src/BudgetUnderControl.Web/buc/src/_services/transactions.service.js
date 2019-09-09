import config from 'config';
import { authHeader } from '../_helpers';
import { handleResponse } from '../_helpers';
import { catchError } from '../_helpers';
import axios from 'axios';

export const transactionsService = {
    getAll,
    add,
    get,
    edit,
    remove
};

function getAll() {

    return axios.get(`${config.apiUrl}/transactions`, { params:{}, headers: authHeader()})
    .then(handleResponse)
    .then(data => {
        return data;
    }).catch(catchError);
}

function add(transaction) {

    return axios.post(`${config.apiUrl}/transactions`, transaction, { headers: authHeader()})
    .then(handleResponse)
    .then(data => {
        debugger;
        return data;
    }).catch(catchError);
}

function get(guid) {

    return axios.get(`${config.apiUrl}/transactions/${guid}`, { params:{}, headers: authHeader()})
    .then(handleResponse)
    .then(data => {
        return data;
    }).catch(catchError);
}

function edit(guid, transaction) {

    return axios.put(`${config.apiUrl}/transactions/${guid}`,transaction, { params:{}, headers: authHeader()})
    .then(handleResponse)
    .then(data => {
        return data;
    }).catch(catchError);
}

function remove(guid) {

    return axios.delete(`${config.apiUrl}/transactions/${guid}`, { params:{}, headers: authHeader()})
    .then(handleResponse)
    .then(data => {
        return data;
    }).catch(catchError);
}

