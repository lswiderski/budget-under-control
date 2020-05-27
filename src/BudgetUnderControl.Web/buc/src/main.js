import Vue from 'vue'
import App from './App.vue'
import store from './_store/index'
import router from './_helpers/router'
import vuetify from './plugins/vuetify';
import 'leaflet/dist/leaflet.css';

Vue.config.productionTip = false

new Vue({
  store,
  router,
  vuetify,
  devtool: 'source-map',
  render: h => h(App),
}).$mount('#app')