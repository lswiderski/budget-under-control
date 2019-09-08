<template>
  <v-app id="inspire">
    <v-navigation-drawer
      v-model="drawer"
      app
      v-if="IsLogged"
    >
      <v-list dense>
        <v-list-item  to="/">
          <v-list-item-action>
            <v-icon>mdi-home</v-icon>
          </v-list-item-action>
          <v-list-item-content>
            <v-list-item-title>Home</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
         <v-list-item  to="/transactions">
          <v-list-item-action>
            <v-icon>mdi-file-document-box-check-outline</v-icon>
          </v-list-item-action>
          <v-list-item-content>
            <v-list-item-title>Transactions</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
          <v-list-item  to="/accounts">
          <v-list-item-action>
            <v-icon>mdi-cash-100</v-icon>
          </v-list-item-action>
          <v-list-item-content>
            <v-list-item-title>Accounts</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
         <v-list-item  to="/categories">
          <v-list-item-action>
            <v-icon>mdi-shape</v-icon>
          </v-list-item-action>
          <v-list-item-content>
            <v-list-item-title>Categories</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
        <v-list-item to="/about">
          <v-list-item-action>
            <v-icon>mdi-contact-mail</v-icon>
          </v-list-item-action>
          <v-list-item-content>
            <v-list-item-title>About</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
      </v-list>
      <template v-slot:append>
        <div class="pa-2">
          <v-btn block  v-if="IsLogged" to="/login">Logout</v-btn>
        </div>
      </template>
    </v-navigation-drawer>

    <v-app-bar v-if="IsLogged"
      app
      color="indigo"
      dark
    >
      <v-app-bar-nav-icon @click.stop="drawer = !drawer"></v-app-bar-nav-icon>
      <v-toolbar-title>Business Under Control</v-toolbar-title>
    </v-app-bar>

    <v-content>
      <v-container
        fluid
      >
      <router-view></router-view>
      </v-container>
    </v-content>
    <v-footer
      color="indigo"
      app
    >
      <span class="white--text">&copy; 2019</span>
    </v-footer>
  </v-app>
</template>

<script>


export default {
  name: 'App',
  props: {
      source: String,
    },
   computed: {
     IsLogged () {
            return this.$store.state.authentication.token
        }
    },
    watch:{
        $route (to, from){
            // clear alert on location change
            this.$store.dispatch('alert/clear');
        }
    },
  data: () => ({
     drawer: null,
  }),
};
</script>