
export const transactionFilters = {
    namespaced: true,
    state: {
        fromDate: null,
        toDate: null,
        accountsIds: [],
        categoryIds: [],
        tagIds: [],
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
        },
        setCategoryIds({ commit }, ids ){
            commit('setCategoryIds', ids);
        },
        setTagIds({ commit }, ids ){
            commit('setTagIds', ids);
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
        },
        setCategoryIds(state, ids) {
            state.categoryIds = ids;
        },
        setTagIds(state, ids) {
            state.tagIds = ids;
        }
       
    }
}
