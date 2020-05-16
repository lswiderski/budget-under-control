import { transactionsService } from '../_services';

export const transactions = {
    namespaced: true,
    state: {
        transactions: {}
    },
    actions: {
        getAll({ commit },filters) {
            commit('getAllRequest');

            transactionsService.getAll(filters)
                .then(
                    data => commit('getAllSuccess', data),
                    error => commit('getAllFailure', error)
                );
        }
    },
    mutations: {
        getAllRequest(state) {
            state.transactions = { loading: true };
        },
        getAllSuccess(state, data) {
            state.transactions = { items: data};
        },
        getAllFailure(state, error) {
            state.transactions = { error };
        }
    }
}
