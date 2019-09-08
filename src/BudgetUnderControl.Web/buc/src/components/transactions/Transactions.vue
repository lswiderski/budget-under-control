<template>
  <v-data-table id="transactionsTable"
  :headers="headers"
    :items="transactions.items"
    :items-per-page="5"
  >
    <template v-slot:item.id="{ item }">
        <div class="handle" style="max-width: 28px;">::</div>
    </template>
      <template v-slot:item.value="{ item }">
        <span :style="{color: getColor(item.value)}">{{ item.value }}</span>
    </template>
    <template v-slot:item.date="{ item }">
        <div >{{item.date | formatDate}}</div>
    </template>
 </v-data-table>
</template>

<script>
import Sortable from "sortablejs"
import moment from 'moment'
export default {

    data() {
        return {
        headers: [
            {text:"", align: "left", sortable: false,  value: "id"},
            {text: "Date", value: "date"},
           {text: "Name", value: "name"},
           {text: "Value", value: "value"},
           {text: "Currency", value: "currencyCode"}
        ]
        }
    },
    computed: {
         transactions () {
            return this.$store.state.transactions.transactions;
        }
    },
    created () {
        this.$store.dispatch('transactions/getAll');
    },
    methods: {
        getColor (value) {
            if (value < 0) return 'red'
            else return 'green'
      },
    },
    mounted(){
        let table = document.querySelector("#transactionsTable tbody");
        const _self = this;
        Sortable.create(table, {
            handle: ".handle",
            onEnd({newIndex, oldIndex}){
                const rowSelected = _self.transactions.items.splice(oldIndex,1)[0];
                _self.transactions.items.splice(newIndex, 0, rowSelected);
            }
        });
    },
    filters: {
        formatDate: function(value) {
  if (value) {
    return moment(String(value)).format('DD/MM/YYYY hh:mm')
  }
}
    }
};
</script>

<style scoped>
.handle{
    cursor:move !important;
    cursor: -webkit-grabbing !important;
}
</style>