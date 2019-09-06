import { loginService } from '../_services';

export const accounts = {
    namespaced: true,
    state: {
        all: {}
    },
    actions: {
        getAll({ commit }) {
            commit('getAllRequest');

            loginService.getAll()
                .then(
                    accounts => commit('getAllSuccess', accounts),
                    error => commit('getAllFailure', error)
                );
        }
    },
    mutations: {
        getAllRequest(state) {
            state.all = { loading: true };
        },
        getAllSuccess(state, accounts) {
            state.all = { items: accounts };
        },
        getAllFailure(state, error) {
            state.all = { error };
        }
    }
}
