import Vue from 'vue';
import Vuex from 'vuex';

import { alert } from './alert.module';
import { authentication } from './authentication.module';
import { login } from './login.module';
import { categories } from './categories.module';
import { accounts } from './accounts.module';
import { transactions } from './transactions.module';
import { tags } from './tags.module';
Vue.use(Vuex);

export default new Vuex.Store({
    modules: {
        alert,
        authentication,
        login,
        categories,
        accounts,
        transactions,
        tags
    }
});
