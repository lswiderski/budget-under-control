import { categoriesService } from '../_services';

export const categories = {
    namespaced: true,
    state: {
        categories: {}
    },
    actions: {
        getAll({ commit }) {
            commit('getAllRequest');

            categoriesService.getAll()
                .then(
                    data => commit('getAllSuccess', data),
                    error => commit('getAllFailure', error)
                );
        }
    },
    mutations: {
        getAllRequest(state) {
            state.categories = { loading: true };
        },
        getAllSuccess(state, data) {
            state.categories = { items: data};
        },
        getAllFailure(state, error) {
            state.categories = { error };
        }
    }
}
