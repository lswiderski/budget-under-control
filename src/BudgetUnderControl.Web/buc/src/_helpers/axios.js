import Vue from 'vue'
import axios from 'axios'
import VueAxios from 'vue-axios'
import Configuration from '../_helpers/configuration';

axios.defaults.baseURL = Configuration.value('backendHost');

Vue.use(VueAxios, axios)