<template>
<div>
  <apexchart width="800" type="area" :options="options" :series="series"></apexchart>
</div>
</template>

<script>
import Vue from 'vue';
import VueApexCharts from 'vue-apexcharts'

Vue.component('apexchart', VueApexCharts)

export default {
     name: "MovingSum",
  data: function() {
    return {
      options: {
        chart:{
              type: 'area',
              stacked: false,
              zoom: {
                type: 'x',
                enabled: true,
                autoScaleYaxis: true
              },
              toolbar: {
                autoSelected: 'zoom'
              },
              id: 'movingSumChart',
            },
             maintainAspectRatio: false,
           
            dataLabels: {
              enabled: false
            },
            markers: {
              size: 0,
            },
            fill: {
              type: 'gradient',
              gradient: {
                shadeIntensity: 1,
                inverseColors: false,
                opacityFrom: 0.5,
                opacityTo: 0,
                stops: [0, 90, 100]
              },
            },
            yaxis: {
              title: {
                text: 'Budget'
              },
            },
        xaxis: {
          type: 'datetime'
        },
        stroke: {
          curve: 'straight',
        }
      },
      series: [{
        name: 'PLN',
        data: []
      }]
    }
  },
  computed: {
    dataSource() {
      return this.$store.state.reports.movingSumDataSource;
    }
  },
  watch:{
    dataSource(newValue) {
      if(newValue.items)
      {

      const data = this.$store.state.reports.movingSumDataSource.items.map(serieValue => ({ x: serieValue.date, y: serieValue.value }));
      this.series = [{
        name: 'PLN',
        data
      }]
      }
  }
  }
};
</script>