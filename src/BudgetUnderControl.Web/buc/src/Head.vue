<template>
<div></div>
</template>
<script>
import Vue from 'vue'
import VueRouter from 'vue-router'
import VueHead from 'vue-head'

Vue.use(VueHead)
Vue.use(VueRouter)

export default {
    head:{
        link: [
     // { rel: 'stylesheet', href: 'https://api.tiles.mapbox.com/mapbox-gl-js/v0.53.0/mapbox-gl.css'},
     //   { rel: 'stylesheet', href: 'https://cdn.jsdelivr.net/npm/vue-mapbox@latest/dist/vue-mapbox.css'},
        ],
        script:[
        //    { src: 'https://api.tiles.mapbox.com/mapbox-gl-js/v0.53.0/mapbox-gl.js' },
         //   { type: 'text/javascript', src: 'https://cdn.jsdelivr.net/npm/vue-mapbox@latest/dist/vue-mapbox.min.js' }
        ] 
    }
}
</script>