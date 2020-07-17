<template>
  <v-data-table
  :headers="headers"
    :items="accounts.items"
    :items-per-page="15"
  ></v-data-table>
</template>

<script>
export default {

    data() {
        return {
        headers: [
           {text: "Name", value: "name"},
           {text: "Balance", value: "amountWithCurrency"},
           {text: "Currency", value: "currency"}
        ]
        }
    },
    computed: {
         accounts () {
            return this.$store.state.accounts.accounts;
        }
    },
    created () {
        this.$store.dispatch('accounts/getAll');
    },
    methods: {
    }
};
</script>