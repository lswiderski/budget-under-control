import { authHeader } from '../_helpers';
import { handleResponse } from '../_helpers';
import { catchError } from '../_helpers';
import axios from 'axios';

export const reportsService = {
    getMovingSumData,
    getDashboardData
};

function getMovingSumData(filters) {
    const Qs = require('qs');
    return axios.get(`/reports/movingsum`, { params:filters, headers: authHeader(),paramsSerializer: function(params) {
        return Qs.stringify(params, {arrayFormat: 'repeat'})
    }})
    .then(handleResponse)
    .catch(catchError);
}

function getDashboardData() {
    return axios.get(`/reports/dashboard`, { headers: authHeader() })
    .then(handleResponse)
    .catch(catchError);
}
