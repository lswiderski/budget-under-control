<template>
  <div>
    <div>
      <TransactionFilters v-on:filtersChanged="refreshGrid"></TransactionFilters>
    </div>
    <EditTransaction ref="editDialog"  />
    <v-data-table
      id="transactionsTable"
      :headers="headers"
      :items="transactions.items"
      :items-per-page="50"
    >

    <template v-slot:top>
       <v-toolbar flat color="white">
        <v-toolbar-title>Transactions</v-toolbar-title>
        <v-divider class="mx-6" inset vertical></v-divider>
        <div class="flex-grow-1"></div>
       </v-toolbar>
    </template>
      // eslint-disable-next-line vue/no-unused-vars
      <template v-slot:item.id="{ }">
        <div class="handle" style="max-width: 28px;">::</div>
      </template>
      <template v-slot:item.value="{ item }">
        <span :style="{color: getColor(item.value)}">{{ item.value }}</span>
      </template>
      <template v-slot:item.date="{ item }">
        <div>{{item.date | formatDate}}</div>
      </template>
      <template v-slot:item.tags="{ item }">
        <v-chip v-for="(tag, i) in item.tags" :key="i" class="ma-2" color="primary">{{tag.name}}</v-chip>
      </template>
      <template v-slot:item.action="{ item }">
        <v-icon small class="mr-2" @click="editItem(item)">mdi-pencil</v-icon>
        <v-icon small @click="deleteItem(item)">mdi-delete</v-icon>
      </template>
    </v-data-table>
  </div>
</template>

<script>
import Sortable from "sortablejs";
import moment from "moment";
import { authHeader } from "../../_helpers";
import { handleResponse } from "../../_helpers";
import { catchError } from "../../_helpers";
import axios from "axios";
import { transactionsService } from "../../_services";
import TransactionFilters from "./TransactionFilters";
import EditTransaction from "./EditTransaction";

export default {
  name: "Transactions",

  data: () => ({
    categories: [],
    accounts: [],
    errors: [],
    editedIndex: -1,
    types: [
      {
        text: "Income",
        value: 0
      },
      {
        text: "Expense",
        value: 1
      },
      {
        text: "Transfer",
        value: 2
      }
    ],
    headers: [
      { text: "", align: "left", sortable: false, value: "id" },
      { text: "Date", value: "date" },
      { text: "Account", value: "account" },
      { text: "Name", value: "name" },
      { text: "Value", value: "value" },
      { text: "Currency", value: "currencyCode" },
      { text: "Category", value: "category" },
      { text: "Tags", value: "tags" },
      { text: "Actions", sortable: false, value: "action" }
    ]
  }),
  components: {
    TransactionFilters,
    EditTransaction,
  },
  computed: {
     transaction() {
      return this.$store.state.transactions.transaction;
    },
    transactions() {
      return this.$store.state.transactions.transactions;
    },
    tags() {
      return this.$store.state.tags.tags.items;
    },
    formTitle() {
      return this.editedIndex === -1 ? "New Transaction" : "Edit Transaction";
    },
  },
  created() {
    this.$store.dispatch("tags/getAll");
  },
  methods: {
    refreshGrid: function() {
      this.$store.dispatch(
        "transactions/getAll",
        this.$store.state.transactionFilters
      );
    },
    getColor(value) {
      if (value < 0) return "red";
      else return "green";
    },
    deleteItem(item) {
      const index = this.transactions.items.indexOf(item);
      confirm("Are you sure you want to delete this transactions?") &&
        this.transactions.items.splice(index, 1) &&
        transactionsService.remove(item.externalId).then(() => {
          this.$store.dispatch("transactions/getAll", this.$store.state.transactionFilters);
        });
    },
    editItem(item) {
      this.$refs.editDialog.openDialog(item);
    },
    getAccountIndex(accountId) {
      for (let i = 0; i < this.accounts.length; i++) {
        if (this.accounts[i].id == accountId) {
          return i;
        }
      }

      return -1;
    },
  },
  mounted() {
    let table = document.querySelector("#transactionsTable tbody");
    this.$store.dispatch(
      "transactions/getAll",
      this.$store.state.transactionFilters
    );
    const _self = this;
    Sortable.create(table, {
      handle: ".handle",
      onEnd({ newIndex, oldIndex }) {
        const rowSelected = _self.transactions.items.splice(oldIndex, 1)[0];
        _self.transactions.items.splice(newIndex, 0, rowSelected);
      }
    });
    axios
      .get(`/categories`, { params: {}, headers: authHeader() })
      .then(handleResponse)
      .then(data => {
        _self.categories = data;
      })
      .catch(catchError);

    axios
      .get(`/accounts`, { params: {}, headers: authHeader() })
      .then(handleResponse)
      .then(data => {
        _self.accounts = data;
      })
      .catch(catchError);
  },
 
  filters: {
    formatDate: function(value) {
      if (value) {
        return moment(String(value)).format("DD/MM/YYYY hh:mm");
      }
    }
  }
};
</script>

<style scoped>

.handle {
  cursor: move !important;
  cursor: -webkit-grabbing !important;
}
</style>