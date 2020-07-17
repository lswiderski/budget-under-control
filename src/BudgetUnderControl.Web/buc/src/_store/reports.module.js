import { reportsService } from '../_services';

export const reports = {
    namespaced: true,
    state: {
        movingSumDataSource: {},
        dashboardData: {},
    },
    actions: {
        getMovingSumData({ commit }, filters) {
            commit('getMovingSumDataRequest');
            reportsService.getMovingSumData(filters)
                .then(
                    data => commit('getMovingSumDataSuccess', data),
                    error => commit('getMovingSumDataFailure', error)
                );
        },
        getDashboardData({ commit }) {
            commit('getDashboardDataRequest');

            reportsService.getDashboardData()
                .then(
                    data => commit('getDashboardDataSuccess', data),
                    error => commit('getDashboardDataFailure', error)
                );
        }
    },
    mutations: {
        getMovingSumDataRequest(state) {
            state.movingSumDataSource = { loading: true };
        },
        getMovingSumDataSuccess(state, data) {
            state.movingSumDataSource = { items: data};
        },
        getMovingSumDataFailure(state, error) {
            state.movingSumDataSource = { error };
        },

        getDashboardDataRequest(state) {
            state.dashboardData = { loading: true };
        },
        getDashboardDataSuccess(state, data) {
            state.dashboardData = { items: data};
        },
        getDashboardDataFailure(state, error) {
            state.dashboardData = { error };
        }
    }
}
