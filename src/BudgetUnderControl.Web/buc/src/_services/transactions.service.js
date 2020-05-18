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

function getAll(filters) {
    const Qs = require('qs');
    return axios.get(`/transactions`, { params:filters, headers: authHeader(),paramsSerializer: function(params) {
        return Qs.stringify(params, {arrayFormat: 'repeat'})
    }})
    .then(handleResponse)
    .then(data => {
        return data;
    }).catch(catchError);
}

function add(transaction) {

    return axios.post(`/transactions`, transaction, { headers: authHeader()})
    .then(handleResponse)
    .catch(catchError);
}

function get(guid) {

    return axios.get(`/transactions/${guid}`, { params:{}, headers: authHeader()})
    .then(handleResponse)
    .catch(catchError);
}

function edit(guid, transaction) {

    return axios.put(`/transactions/${guid}`,transaction, { params:{}, headers: authHeader()})
    .then(handleResponse)
    .catch(catchError);
}

function remove(guid) {

    return axios.delete(`/transactions/${guid}`, { params:{}, headers: authHeader()})
    .then(handleResponse)
    .catch(catchError);
}

