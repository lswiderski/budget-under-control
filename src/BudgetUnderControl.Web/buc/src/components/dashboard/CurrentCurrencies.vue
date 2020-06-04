<template>
  <v-data-table
  :headers="headers"
    :items="currencies"
    :items-per-page="15"
  ></v-data-table>
</template>

<script>
export default {
    name: "CurrentCurrencies",
    data() {
        return {
        headers: [
           {text: "Name", value: "currency"},
           {text: "Balance", value: "balance"},
        ],
        currencies:[]
        }
    },
    computed: {
        dataSource() {
      return this.$store.state.reports.dashboardData;
    }
    },
         watch: {
    dataSource(newValue) {

      if (newValue.items) {
          this.currencies = this.$store.state.reports.dashboardData.items.actualStatus;

      }
    },
},
};
</script>

<style>

</style>