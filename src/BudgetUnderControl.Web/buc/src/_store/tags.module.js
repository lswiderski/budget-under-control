import { tagsService } from '../_services';
export const tags = {
    namespaced: true,
    state: {
        tags: {}
    },
    actions: {
        getAll({ commit }) {
            commit('getAllRequest');

            tagsService.getAll()
                .then(
                    data => commit('getAllSuccess', data),
                    error => commit('getAllFailure', error)
                );
        }
    },
    mutations: {
        getAllRequest(state) {
            state.tags = { loading: true };
        },
        getAllSuccess(state, data) {
            state.tags = { items: data};
        },
        getAllFailure(state, error) {
            state.tags = { error };
        }
    }
}
