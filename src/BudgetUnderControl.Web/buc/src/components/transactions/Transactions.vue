<template>
  <div>
    <div>
      <TransactionFilters v-on:filtersChanged="refreshGrid"></TransactionFilters>
    </div>
    <v-data-table
      id="transactionsTable"
      :headers="headers"
      :items="transactions.items"
      :items-per-page="50"
    >
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
import OnePointMap from "../maps/OnePointMap";
import { UploaderComponent, UploaderPlugin } from "@syncfusion/ej2-vue-inputs";
import Vue from "vue";

Vue.component(UploaderPlugin.name, UploaderComponent);

export default {
  name: "Transactions",

  data: () => ({
    dialog: false,
    dateMenu: false,
    menuTimePicker: false,
    dateTransferMenu: false,
    menuTransferTimePicker: false,
    categories: [],
    accounts: [],
    errors: [],
    editedIndex: -1,
    editedItem: null,
    defaultItem: {
      name: "",
      accountId: 3,
      categoryId: 0,
      amount: 0,
      date: new Date(Date.now()).toISOString().substr(0, 10),
      time: new Date().getHours() + ":" + new Date().getMinutes(),
      comment: "",
      type: 1,
      transferAccountId: 4,
      transferDate: new Date(Date.now()).toISOString().substr(0, 10),
      transferTime: new Date().getHours() + ":" + new Date().getMinutes(),
      transferAmount: 0,
      rate: 1,
      tags: null,
      latitude: null,
      longitude: null
    },
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
    ],
    tab: "tab-general"
  }),
  components: {
    TransactionFilters,
    OnePointMap
  },
  computed: {
    transactions() {
      return this.$store.state.transactions.transactions;
    },
    tags() {
      return this.$store.state.tags.tags.items;
    },
    formTitle() {
      return this.editedIndex === -1 ? "New Transaction" : "Edit Transaction";
    },
    isTransferInOtherCurrency() {
      let accountIndex = this.getAccountIndex(this.editedItem.accountId);
      let transferAccountIndex = this.getAccountIndex(
        this.editedItem.transferAccountId
      );
      return (
        transferAccountIndex > -1 &&
        this.accounts[accountIndex].currencyId !=
          this.accounts[transferAccountIndex].currencyId
      );
    }
  },
  created() {
    this.$store.dispatch("tags/getAll");
    this.editedItem = this.defaultItem;
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
          this.$store.dispatch("transactions/getAll");
        });
    },
    onMapClick(value) {
      this.editedItem.latitude = value.lat;
      this.editedItem.longitude = value.lng;
    },
    editItem(item) {
      const _self = this;
      transactionsService
        .get(item.externalId)
        .then(data => {
          let dto = _self.mapAPIDTOToEditDTO(data);
          _self.editedIndex = _self.transactions.items.indexOf(item);
          _self.editedItem = Object.assign({}, dto);
          _self.dialog = true;
          setTimeout(() => {
            this.$refs.transacionMap.invalideSize();
          }, 100);
        })
        .catch(errors => {
          _self.errors = errors;
        });
    },
    mapEditDTOToAPIDTO(data) {
      let dto = Object.assign({}, data);
      dto.date = new Date(dto.date + " " + dto.time);
      dto.transferDate = new Date(dto.transferDate + " " + dto.transferTime);
      if (dto.amount > 0) {
        dto.amount *= -1;
      }

      if (dto.transferAmount < 0) {
        dto.transferAmount *= -1;
      }
      return dto;
    },
    mapAPIDTOToEditDTO(data) {
      let dto = Object.assign({}, data);
      dto.type = dto.extendedType;
      dto.amount = Math.abs(dto.amount);
      if (dto.transferAmount != null) {
        dto.transferAmount = Math.abs(dto.transferAmount);
      }

      dto.time =
        new Date(dto.date).getHours() + ":" + new Date(dto.date).getMinutes();
      dto.date = new Date(dto.date).toISOString().substr(0, 10);
      dto.transferTime =
        new Date(dto.transferDate).getHours() +
        ":" +
        new Date(dto.transferDate).getMinutes();
      dto.transferDate = new Date(dto.transferDate).toISOString().substr(0, 10);
      return dto;
    },
    getAccountIndex(accountId) {
      for (let i = 0; i < this.accounts.length; i++) {
        if (this.accounts[i].id == accountId) {
          return i;
        }
      }

      return -1;
    },

    transferAmountChanged() {
      if (
        this.editedItem.transferAmount != 0 &&
        this.editedItem.transferAmount != "0"
      ) {
        this.editedItem.rate =
          this.editedItem.amount / this.editedItem.transferAmount;
      }
    },
    close() {
      this.dialog = false;
      this.errors = [];
      setTimeout(() => {
        this.editedItem = Object.assign({}, this.defaultItem);
        this.editedIndex = -1;
      }, 300);
    },

    save() {
      const _self = this;
      let dto = _self.mapEditDTOToAPIDTO(this.editedItem);

      if (this.editedIndex > -1) {
        transactionsService
          .edit(this.editedItem.externalId, dto)
          .then(() => {
            this.$store.dispatch("transactions/getAll");
            this.close();
          })
          .catch(data => {
            _self.errors = data;
          });
      } else {
        transactionsService
          .add(dto)
          .then(() => {
            this.transactions.items.push(_self.editedItem);
            this.$store.dispatch("transactions/getAll");
            this.close();
          })
          .catch(errors => {
            _self.errors = errors;
          });
      }
    }
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
  watch: {
    dialog(val) {
      val || this.close();
    }
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
@import "../../../node_modules/@syncfusion/ej2-base/styles/material.css";
@import "../../../node_modules/@syncfusion/ej2-buttons/styles/material.css";
@import "../../../node_modules/@syncfusion/ej2-vue-inputs/styles/material.css";
.handle {
  cursor: move !important;
  cursor: -webkit-grabbing !important;
}
</style>