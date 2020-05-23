<template>
<div>
    <TransactionFilters v-on:filtersChanged="refreshChart"/>

    <MovingSum />

</div>
</template>
<script>

import MovingSum from "./MovingSum"
import TransactionFilters from "@/components/transactions/TransactionFilters"

export default {
    name: "MovingSumReport",
    components: {
        MovingSum,
        TransactionFilters,
    },
    created() {
       this.refreshChart();
    },
    methods: {
        refreshChart: function() {
             this.$store.dispatch("reports/getMovingSumData",this.$store.state.transactionFilters)
        }
    },
}
</script>
<style lang="sass" scoped>

</style>