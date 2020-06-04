<template>
  <v-container class="grey lighten-5">
    <v-row>
      <v-col md="4">
        <v-card class="pa-2" outlined tile>
          <v-card-title> this Month </v-card-title>
          <CategoryPieChart :series="thisMonthSeries" :labels="thisMonthLabels" />
        </v-card>
      </v-col>
      <v-col md="4">
        <v-card class="pa-2" outlined tile>
          <v-card-title> previous Month </v-card-title>
          <CategoryPieChart :series="previousMonthSeries" :labels="previousMonthLabels" />
        </v-card>
      </v-col>
      <v-col md="4">
        <v-card class="pa-2" outlined tile>
          <v-card-title>Incomes and Expenses</v-card-title>
          <IncomesExpensesChart :series="incomesExpensesSeries" :labels="incomesExpensesLabels" />
        </v-card>
      </v-col>
    </v-row>
    <v-row>
       <v-col md="4">
        <v-card class="pa-2" outlined tile>
          <v-card-title>Status</v-card-title>
          <CurrentCurrencies />
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import CategoryPieChart from "@/components/charts/CategoryPieChart";
import IncomesExpensesChart from "@/components/charts/IncomesExpensesChart";
import CurrentCurrencies from "./CurrentCurrencies"

export default {
  name: "Dashboard",
  components: {
    CategoryPieChart,
    IncomesExpensesChart,
    CurrentCurrencies,
  },
  data: () => ({
    thisMonthSeries: [],
    thisMonthLabels: [],
    previousMonthSeries: [],
    previousMonthLabels: [],
    incomesExpensesSeries: [],
    incomesExpensesLabels: [],
  }),
  created() {
    this.refreshDashboard();
  },
  methods: {
    refreshDashboard: function() {
      this.$store.dispatch("reports/getDashboardData");
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
        // eslint-disable-next-line no-debugger
        debugger;
        this.thisMonthSeries =  this.$store.state.reports.dashboardData.items.thisMonthCategoryChart.map(
          serieValue => (Math.abs(serieValue.value))
        );
        this.thisMonthLabels = this.$store.state.reports.dashboardData.items.thisMonthCategoryChart.map(
          serieValue => (serieValue.category)
        );

        this.previousMonthSeries =  this.$store.state.reports.dashboardData.items.lastMonthCategoryChart.map(
          serieValue => (Math.abs(serieValue.value))
        );
        this.previousMonthLabels = this.$store.state.reports.dashboardData.items.lastMonthCategoryChart.map(
          serieValue => (serieValue.category)
        );
          this.incomesExpensesSeries =
          [
            {
              name: "income",
              data: Object.values(this.$store.state.reports.dashboardData.items.incomes)
            },
            {
              name: "expense",
              data: Object.values(this.$store.state.reports.dashboardData.items.expenses)
            }
          ];
          this.incomesExpensesLabels =  Object.keys(this.$store.state.reports.dashboardData.items.incomes).map(
            x => (new Date(x).toLocaleString('default', { month: 'long', year: 'numeric' }))
          );
      }
    }
  }
};
</script>

<style lang="sass" scoped>

</style>

