import { authHeader } from '../_helpers';
import { handleResponse } from '../_helpers';
import { catchError } from '../_helpers';
import axios from 'axios';

export const reportsService = {
    getMovingSumData
};

function getMovingSumData(filters) {
    const Qs = require('qs');
    return axios.get(`/reports/movingsum`, { params:filters, headers: authHeader(),paramsSerializer: function(params) {
        return Qs.stringify(params, {arrayFormat: 'repeat'})
    }})
    .then(handleResponse)
    .catch(catchError);
}
