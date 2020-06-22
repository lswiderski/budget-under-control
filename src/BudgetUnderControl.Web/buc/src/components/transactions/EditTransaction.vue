<template>
  <v-dialog v-model="dialog" max-width="1000px">
    <template v-slot:activator="{ on }">
      <v-btn color="primary" dark class="mb-2" v-on="on" @click="openEmptyDialog" >New transaction</v-btn>
    </template>
    <v-card>
      <v-card-title>
        <span class="headline">{{ formTitle }}</span>
      </v-card-title>
      <v-tabs v-model="tab">
        <v-tabs-slider></v-tabs-slider>
        <v-tab href="#tab-general">General</v-tab>
        <v-tab href="#tab-files">Files</v-tab>
      </v-tabs>
      <v-tabs-items v-model="tab">
        <v-tab-item :value="'tab-general'">
          <v-card-text>
            <v-container>
              <v-row>
                <v-col cols="12" md="6">
                  <v-row>
                    <v-col cols="12" md="12">
                      <div v-if="errors.length">
                        <b>Please correct the following error(s):</b>
                        <ul>
                          <li v-for="(error, i) in errors" :key="i">{{ error }}</li>
                        </ul>
                      </div>
                    </v-col>
                    <v-col cols="12" md="12">
                      <v-select :items="types" :value="1" v-model="editedItem.type" label="Type"></v-select>
                    </v-col>
                    <v-col cols="12" md="6">
                      <v-text-field v-model="editedItem.amount" label="Amount"></v-text-field>
                    </v-col>
                    <v-col cols="12" md="6">
                      <v-text-field v-model="editedItem.name" label="name"></v-text-field>
                    </v-col>

                    <v-col cols="12" md="6">
                      <v-select
                        v-model="editedItem.accountId"
                        :items="accounts"
                        item-text="name"
                        item-value="id"
                        label="Account"
                      ></v-select>
                    </v-col>
                    <v-col cols="12" md="6">
                      <v-select
                        :items="categories"
                        v-model="editedItem.categoryId"
                        item-text="name"
                        item-value="id"
                        label="Category"
                      ></v-select>
                    </v-col>
                    <v-col cols="12" md="6">
                      <v-menu
                        v-model="dateMenu"
                        :close-on-content-click="false"
                        :nudge-right="40"
                        transition="scale-transition"
                        offset-y
                        min-width="290px"
                      >
                        <template v-slot:activator="{ on }">
                          <v-text-field
                            v-model="editedItem.date"
                            label="Date"
                            prepend-icon="mdi-calendar"
                            readonly
                            v-on="on"
                          ></v-text-field>
                        </template>
                        <v-date-picker v-model="editedItem.date" @input="dateMenu = false"></v-date-picker>
                      </v-menu>
                    </v-col>
                    <v-col cols="12" md="6">
                      <v-menu
                        ref="menu"
                        v-model="menuTimePicker"
                        :close-on-content-click="false"
                        :nudge-right="40"
                        :return-value.sync="editedItem.time"
                        transition="scale-transition"
                        offset-y
                        max-width="290px"
                        min-width="290px"
                      >
                        <template v-slot:activator="{ on }">
                          <v-text-field
                            v-model="editedItem.time"
                            label="Time"
                            prepend-icon="mdi-clock-outline"
                            readonly
                            v-on="on"
                          ></v-text-field>
                        </template>
                        <v-time-picker
                          v-if="menuTimePicker"
                          v-model="editedItem.time"
                          @click:minute="$refs.menu.save(editedItem.time)"
                        ></v-time-picker>
                      </v-menu>
                    </v-col>

                    <v-col cols="12" md="6" v-if="editedItem.type === 2">
                      <v-menu
                        v-model="dateTransferMenu"
                        :close-on-content-click="false"
                        :nudge-right="40"
                        transition="scale-transition"
                        offset-y
                        min-width="290px"
                      >
                        <template v-slot:activator="{ on }">
                          <v-text-field
                            v-model="editedItem.transferDate"
                            label="Transfer Date"
                            prepend-icon="mdi-calendar"
                            readonly
                            v-on="on"
                          ></v-text-field>
                        </template>
                        <v-date-picker
                          v-model="editedItem.transferDate"
                          @input="dateTransferMenu = false"
                        ></v-date-picker>
                      </v-menu>
                    </v-col>
                    <v-col cols="12" md="6" v-if="editedItem.type === 2">
                      <v-menu
                        ref="menu2"
                        v-model="menuTransferTimePicker"
                        :close-on-content-click="false"
                        :nudge-right="40"
                        :return-value.sync="editedItem.transferTime"
                        transition="scale-transition"
                        offset-y
                        max-width="290px"
                        min-width="290px"
                      >
                        <template v-slot:activator="{ on }">
                          <v-text-field
                            v-model="editedItem.transferTime"
                            label="Transfer Time"
                            prepend-icon="mdi-clock-outline"
                            readonly
                            v-on="on"
                          ></v-text-field>
                        </template>
                        <v-time-picker
                          v-if="menuTransferTimePicker"
                          v-model="editedItem.transferTime"
                          @click:minute="$refs.menu2.save(editedItem.transferTime)"
                        ></v-time-picker>
                      </v-menu>
                    </v-col>
                    <v-col cols="12" md="6" v-if="editedItem.type === 2">
                      <v-select
                        v-model="editedItem.transferAccountId"
                        :items="accounts"
                        item-text="name"
                        item-value="id"
                        label="Transfer Account"
                      ></v-select>
                    </v-col>
                    <v-col cols="12" md="6" v-if="editedItem.type === 2">
                      <v-text-field
                        v-model="editedItem.transferAmount"
                        label="Transfer Amount"
                        @change="transferAmountChanged"
                      ></v-text-field>
                    </v-col>
                    <v-col
                      cols="12"
                      md="6"
                      v-if="editedItem.type === 2 && isTransferInOtherCurrency"
                    >
                      <v-text-field v-model="editedItem.rate" label="Rate"></v-text-field>
                    </v-col>
                    <v-col cols="12" md="12">
                      <v-select
                        v-model="editedItem.tags"
                        :items="tags"
                        item-text="name"
                        item-value="id"
                        label="Tags"
                        multiple
                        chips
                        hint="multiple selects"
                        persistent-hint
                      ></v-select>
                    </v-col>
                  </v-row>
                </v-col>
                <v-col cols="12" md="6">
                  <v-row>
                    <v-col cols="12" md="6">
                      <v-text-field v-model="editedItem.latitude" label="latitude"></v-text-field>
                    </v-col>
                    <v-col cols="12" md="6">
                      <v-text-field v-model="editedItem.longitude" label="Longitude"></v-text-field>
                    </v-col>
                    <v-col cols="12" md="12">
                      <OnePointMap
                        :latitude="editedItem.latitude"
                        :longitude="editedItem.longitude"
                        :centerLatitude="centerLatitude"
                        :centerLongitude="centerLongitude"
                        v-on:coordsChanged="onMapClick"
                        ref="transacionMap"
                      />
                    </v-col>
                    <v-col cols="12" md="12">
                      <v-textarea v-model="editedItem.comment" label="Comment"></v-textarea>
                    </v-col>
                  </v-row>
                </v-col>
              </v-row>
            </v-container>
          </v-card-text>
        </v-tab-item>
        <v-tab-item :value="'tab-files'">
          <div></div>
        </v-tab-item>
      </v-tabs-items>
      <v-card-actions>
        <div class="flex-grow-1"></div>
        <v-btn color="blue darken-1" text @click="close">Cancel</v-btn>
        <v-btn color="blue darken-1" text @click="save">Save</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>
<script>
import { authHeader } from "../../_helpers";
import { handleResponse } from "../../_helpers";
import { catchError } from "../../_helpers";
import axios from "axios";
import OnePointMap from "../maps/OnePointMap";
import { transactionsService } from "../../_services";
import { UploaderComponent, UploaderPlugin } from "@syncfusion/ej2-vue-inputs";
import Vue from "vue";
Vue.component(UploaderPlugin.name, UploaderComponent);

export default {
  name: "EditTransaction",
  components: {
    OnePointMap
  },
  data: () => ({
    dialog: false,
    dateMenu: false,
    menuTimePicker: false,
    dateTransferMenu: false,
    menuTransferTimePicker: false,
    categories: [],
    accounts: [],
    errors: [],
    editedItem: {},
    isExist: false,
    defaultItem: {
      name: "",
      accountId: 3,
      categoryId: 0,
      amount: 0,
      date: new Date(Date.now()).toISOString().substr(0, 10),
      time: new Date().getHours() + ":" + new Date().getMinutes(),
      comment: "",
      type: 1,
      transferAccountId: null,
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
    tab: "tab-general"
  }),
  computed: {
    centerLatitude() {
      return this.editedItem.latitude ?? 52.183411;
    },
    centerLongitude() {
      return this.editedItem.longitude ?? 21.018550;
    },
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
      return this.isExist ? "Edit Transaction" : "New Transaction";
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
    onMapClick(value) {
      this.editedItem.latitude = value.lat;
      this.editedItem.longitude = value.lng;
    },
    mapEditDTOToAPIDTO(data) {
      let dto = Object.assign({}, data);
      dto.extendedType = data.type;
      dto.date = new Date(dto.date + " " + dto.time);
      dto.transferDate = new Date(dto.transferDate + " " + dto.transferTime);
      if (dto.amount > 0) {
        dto.amount *= -1;
      }
        dto.transferAmount = +dto.transferAmount;
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
    openEmptyDialog() {
      this.editedItem = Object.assign({}, this.defaultItem);
      this.errors = [];
      this.dialog = true;
       setTimeout(() => {
            this.$refs.transacionMap.invalideSize();
          }, 100);
    },

    openDialog(item) {
      const _self = this;
      this.isExist = item !== null;
      transactionsService
        .get(item.externalId)
        .then(data => {
          let dto = _self.mapAPIDTOToEditDTO(data);
          _self.editedIndex = _self.transactions.items.indexOf(item);
          this.editedItem = Object.assign({}, dto);
          this.dialog = true;
          setTimeout(() => {
            this.$refs.transacionMap.invalideSize();
          }, 100);
        })
        .catch(errors => {
          _self.errors = errors;
        });
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
      }, 300);
    },
    save() {
      const _self = this;
      let dto = _self.mapEditDTOToAPIDTO(this.editedItem);
      // eslint-disable-next-line no-debugger
      debugger;
      if (this.isExist) {
        transactionsService
          .edit(this.editedItem.externalId, dto)
          .then(() => {
            this.$store.dispatch(
              "transactions/getAll",
              this.$store.state.transactionFilters
            );
            this.close();
          })
          .catch(data => {
            _self.errors = data;
          });
      } else {
        transactionsService
          .add(dto)
          .then(() => {
            this.$store.dispatch(
              "transactions/getAll",
              this.$store.state.transactionFilters
            );
            this.close();
          })
          .catch(errors => {
            _self.errors = errors;
          });
      }
    }
  },
  mounted() {
    const _self = this;

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
  }
};
</script>
<style scoped>
@import "../../../node_modules/@syncfusion/ej2-base/styles/material.css";
@import "../../../node_modules/@syncfusion/ej2-buttons/styles/material.css";
@import "../../../node_modules/@syncfusion/ej2-vue-inputs/styles/material.css";
</style>