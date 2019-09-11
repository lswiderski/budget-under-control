
export const transactionFilters = {
    namespaced: true,
    state: {
        fromDate: null,
        toDate: null,
        accountsIds: []
    },
    actions: {
        setFrom({ commit }, date ){
            commit('setFrom', date);
        },
        setTo({ commit }, date ){
            commit('setTo', date);
        },
        setAccountsIds({ commit }, ids ){
            commit('setAccountsIds', ids);
        }
    },
    mutations: {
        setFrom(state, date) {
            state.fromDate = date;
        },
        setTo(state, date) {
            state.toDate = date;
        },
        setAccountsIds(state, ids) {
            state.accountsIds = ids;
        }
       
    }
}
