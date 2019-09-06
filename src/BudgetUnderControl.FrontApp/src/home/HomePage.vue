<template>
    <div>
        <h1>You're JWT Token {{user}}!</h1>
        <p>Business Under Control</p>
        <h3>Accounts from secure api end point:</h3>
        <em v-if="accounts.loading">Loading accounts...</em>
        <span v-if="accounts.error" class="text-danger">ERROR: {{accounts.error}}</span>
        <ul v-if="accounts.items">
            <li v-for="account in accounts.items" :key="account.id">
                {{account.name+ ' ' + account.balance}}.
            </li>
        </ul>
        <p>
            <router-link to="/login">Logout</router-link>
        </p>
    </div>
</template>

<script>
export default {
    computed: {
        user () {
            return this.$store.state.authentication.user;
        },
        accounts () {
            return this.$store.state.accounts.all;
        }
    },
    created () {
        this.$store.dispatch('accounts/getAll');
    }
};
</script>