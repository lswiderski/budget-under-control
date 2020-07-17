import { authHeader } from '../_helpers';
import { handleResponse } from '../_helpers';
import { catchError } from '../_helpers';
import axios from 'axios';

export const accountsService = {
    getAll
};

function getAll() {

    return axios.get(`/accounts`, { params:{}, headers: authHeader()})
    .then(handleResponse)
    .then(data => {
        return data;
    }).catch(catchError);
}
