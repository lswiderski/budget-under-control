import { loginService } from '../_services';
import router from '../_helpers/router';

const token = JSON.parse(localStorage.getItem('jwt-token'));
const initialState = token
    ? { status: { loggedIn: true }, token }
    : { status: {}, token: null };

export const authentication = {
    namespaced: true,
    state: initialState,
    actions: {
        login({ dispatch, commit }, { username, password }) {
            commit('loginRequest', { username });

            loginService.login(username, password)
                .then(
                    token => {
                        commit('loginSuccess', token);
                        router.push('/');
                    },
                    error => {
                        commit('loginFailure', error);
                        dispatch('alert/error', error, { root: true });
                    }
                );
        },
        logout({ commit }) {
            loginService.logout();
            commit('logout');
        }
    },
    mutations: {
        loginRequest(state, token) {
            state.status = { loggingIn: true };
            state.token = token;
        },
        loginSuccess(state, token) {
            state.status = { loggedIn: true };
            state.token = token;
        },
        loginFailure(state) {
            state.status = {};
            state.token = null;
        },
        logout(state) {
            state.status = {};
            state.token = null;
        }
    }
}
