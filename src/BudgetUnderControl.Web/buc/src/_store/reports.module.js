import { reportsService } from '../_services';

export const reports = {
    namespaced: true,
    state: {
        movingSumDataSource: {}
    },
    actions: {
        getMovingSumData({ commit }, filters) {
            commit('getMovingSumDataRequest');

            reportsService.getMovingSumData(filters)
                .then(
                    data => commit('getMovingSumDataSuccess', data),
                    error => commit('getMovingSumDataFailure', error)
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
        }
    }
}
