import Vue from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'
import "../node_modules/bootstrap/dist/css/bootstrap.css";
import "../node_modules/bootstrap/dist/js/bootstrap.bundle";
import axios from 'axios';
import "./style.css";

Vue.config.productionTip = false

const api = axios.create({
  baseURL: "http://localhost:8080/"
})
const axiosPlugin = {
  install(Vue){
    Vue.prototype.$api = api;
  }
}
Vue.use(axiosPlugin);

new Vue({
  router,
  store,
  render: h => h(App)
}).$mount('#app')
