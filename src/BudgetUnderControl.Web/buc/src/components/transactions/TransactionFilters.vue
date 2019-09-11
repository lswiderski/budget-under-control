<template>
<div><v-row>
     <v-col cols="4" md="2">
                      <v-menu
                        v-model="dateFromMenu"
                        :close-on-content-click="false"
                        :nudge-right="40"
                        transition="scale-transition"
                        offset-y
                        full-width
                        min-width="290px"
                      >
                        <template v-slot:activator="{ on }">
                          <v-text-field
                            v-model="dateFrom"
                            label="From"
                            prepend-icon="mdi-calendar"
                            readonly
                            v-on="on"
                          ></v-text-field>
                        </template>
                        <v-date-picker v-model="dateFrom" @input="dateFromMenu = false"></v-date-picker>
                      </v-menu>
                    </v-col>
                     <v-col cols="4" md="2">
                      <v-menu
                        v-model="dateToMenu"
                        :close-on-content-click="false"
                        :nudge-right="40"
                        transition="scale-transition"
                        offset-y
                        full-width
                        min-width="290px"
                      >
                        <template v-slot:activator="{ on }">
                          <v-text-field
                            v-model="dateTo"
                            label="To"
                            prepend-icon="mdi-calendar"
                            readonly
                            v-on="on"
                          ></v-text-field>
                        </template>
                        <v-date-picker v-model="dateTo" @input="dateToMenu = false"></v-date-picker>
                      </v-menu>
                    </v-col>
                     <v-col cols="4" md="2">
                      <v-select
                        v-model="accountsIds"
                        :items="accounts"
                        item-text="name"
                        item-value="id"
                        label="Accounts"
                        multiple
                        chips
                        hint="include all if empty"
                        persistent-hint
                      ></v-select>
                    </v-col>
                    </v-row>
                    </div>
</template>

<script>

export default {
    name: "TransactionFilters",
    data() {
        return {
            dateFromMenu: false,
            dateToMenu: false,
        }
    },
    computed: {
        dateFrom: {
            get: function () {return this.$store.state.transactionFilters.fromDate },
            set: function (value){
                    this.$store.dispatch("transactionFilters/setFrom", value);
                    this.$store.dispatch("transactions/getAll",this.$store.state.transactionFilters);
            }
        },
        dateTo: {
            get: function () {return this.$store.state.transactionFilters.toDate },
            set: function (value){
                    this.$store.dispatch("transactionFilters/setTo", value);
                    this.$store.dispatch("transactions/getAll",this.$store.state.transactionFilters);
            },
        },
        accountsIds: {
            get: function () {return this.$store.state.transactionFilters.accountsIds },
            set: function (value){
                    this.$store.dispatch("transactionFilters/setAccountsIds", value);
                    this.$store.dispatch("transactions/getAll",this.$store.state.transactionFilters);
            }
        },
        accounts() {
      return this.$store.state.accounts.accounts.items;
    },
    },
    created () {
        var date = new Date();
        var firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
        var lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);

         this.$store.dispatch("transactionFilters/setFrom", firstDay.toISOString().substr(0, 10));
         this.$store.dispatch("transactionFilters/setTo", lastDay.toISOString().substr(0, 10));
         this.$store.dispatch("accounts/getAll");
    },
    methods: {
    }
};
</script>