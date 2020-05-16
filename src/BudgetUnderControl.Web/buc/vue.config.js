var path = require('path');
var webpack = require('webpack');

module.exports = {
    chainWebpack: config =>{
        config.externals({
            // global app config object
            config: JSON.stringify({
                apiUrl: process.env.VUE_APP_BACKEND_API_URL
            })
        })
    },
      devServer: {
        historyApiFallback: true,
        headers: {
            "Access-Control-Allow-Origin": "*",
            "Access-Control-Allow-Methods": "GET, POST, PUT, DELETE, PATCH, OPTIONS",
            "Access-Control-Allow-Headers": "X-Requested-With, content-type, Authorization"
          }
    }
}