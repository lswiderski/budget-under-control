import Vue from 'vue';
import Router from 'vue-router';

import Home from '../views/Home';
import LoginPage from '../components/login/LoginPage'
import Categories from '../components/categories/Categories'
import Accounts from '../components/accounts/Accounts'
import Transactions from '../components/transactions/Transactions'
import Tags from '../components/tags/Tags'
import MovingSumReport from '../components/reports/MovingSumReport'
import Map from '../components/maps/Map'
Vue.use(Router);

let router = new Router({
  mode: 'history',
  routes: [
    { path: '/', component: Home },
    { path: '/login', component: LoginPage },
    { path: '/categories', component: Categories },
    { path: '/accounts', component: Accounts },
    { path: '/transactions', component: Transactions },
    { path: '/tags', component: Tags },
    { path: '/report/movingsum', component: MovingSumReport },
    {path: '/map', component: Map},
    {  path: '/about',
    name: 'about',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import(/* webpackChunkName: "about" */ '../views/About.vue') },

    // otherwise redirect to home
    { path: '*', redirect: '/' }
  ]
});
export default router
router.beforeEach((to, from, next) => {
  // redirect to login page if not logged in and trying to access a restricted page
  const publicPages = ['/login'];
  const authRequired = !publicPages.includes(to.path);
  const loggedIn = localStorage.getItem('jwt-token');

  if (authRequired && !loggedIn) {
    return next('/login');
  }

  next();
})