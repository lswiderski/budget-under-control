<template>
  <div>
    <apexchart type="donut" :options="chartOptions" :series="series" ></apexchart>
  </div>
</template>

<script>
import Vue from "vue";
import VueApexCharts from "vue-apexcharts";

Vue.component("apexchart", VueApexCharts);

export default {
  name: "CategoryPieChart",
  components: {
    apexchart: VueApexCharts
  },
  props: {
      series:  Array,
      labels:  Array,
  },
  data:()=> ( {
     
  }),
  computed: {
     chartOptions() {
       return {
         labels: this.labels,
        responsive: [
          {
            breakpoint: 480,
            options: {
              chart: {
                width: 200
              },
              legend: {
                position: "bottom"
              }
            }
          }
        ]
        }
      }
  }
};
</script>

<style lang="sass" scoped>

</style>