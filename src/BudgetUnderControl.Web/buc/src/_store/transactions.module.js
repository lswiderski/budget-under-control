import { transactionsService } from '../_services';
import router from '../_helpers/router';

export const transactions = {
    namespaced: true,
    state: {
        transactions: {}
    },
    actions: {
        getAll({ commit }) {
            commit('getAllRequest');

            transactionsService.getAll()
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
