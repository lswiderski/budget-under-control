import { transactionsService } from '../_services';

export const transactions = {
    namespaced: true,
    state: {
        transactions: {},
        transaction: {},
        editTransaction: {},
    },
    actions: {
        getAll({ commit }, filters) {
            commit('getAllRequest');

            transactionsService.getAll(filters)
                .then(
                    data => commit('getAllSuccess', data),
                    error => commit('getAllFailure', error)
                );
        },
        getTransaction({ commit }, id) {
            transactionsService.get(id)
                .then(
                    data => commit('getSuccess', data),
                    error => commit('getFailure', error)
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
        },
        getSuccess(state, data) {
            state.transaction = { data };
        },
        getFailure(state, error) {
            state.transaction = { error };
        }
    }
}
