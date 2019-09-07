import { loginService } from '../_services';

export const login = {
    namespaced: true,
    state: {
        all: {}
    },
    actions: {
        getAll({ commit }) {
            commit('getAllRequest');

            loginService.getAll()
                .then(
                    login => commit('getAllSuccess', login),
                    error => commit('getAllFailure', error)
                );
        }
    },
    mutations: {
        getAllRequest(state) {
            state.all = { loading: true };
        },
        getAllSuccess(state, login) {
            state.all = { items: login };
        },
        getAllFailure(state, error) {
            state.all = { error };
        }
    }
}
