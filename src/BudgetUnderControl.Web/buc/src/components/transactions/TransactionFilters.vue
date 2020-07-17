<template>
<div><v-row>
     <v-col cols="4" md="2">
                      <v-menu
                        v-model="dateFromMenu"
                        :close-on-content-click="false"
                        :nudge-right="40"
                        transition="scale-transition"
                        offset-y
                        min-width="290px"
                      >
                        <template v-slot:activator="{ on }">
                          <v-text-field
                            v-model="dateFrom"
                            label="From"
                            prepend-icon="mdi-calendar"
                            readonly
                            v-on="on"
                            clearable
                            @click:clear="dateFrom = null"
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
                    <v-col cols="4" md="2">
                      <v-select
                        v-model="categoryIds"
                        :items="categories"
                        item-text="name"
                        item-value="id"
                        label="Categories"
                        multiple
                        chips
                        hint="include all if empty"
                        persistent-hint
                      ></v-select>
                    </v-col>
                    <v-col cols="4" md="2">
                      <v-select
                        v-model="tagIds"
                        :items="tags"
                        item-text="name"
                        item-value="id"
                        label="Tags"
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
                    this.$emit('filtersChanged');
            }
        },
        dateTo: {
            get: function () {return this.$store.state.transactionFilters.toDate },
            set: function (value){
                    this.$store.dispatch("transactionFilters/setTo", value);
                    this.$emit('filtersChanged');
            },
        },
        accountsIds: {
            get: function () {return this.$store.state.transactionFilters.accountsIds },
            set: function (value){
                    this.$store.dispatch("transactionFilters/setAccountsIds", value);
                    this.$emit('filtersChanged');
            }
        },
        categoryIds: {
            get: function () {return this.$store.state.transactionFilters.categoryIds },
            set: function (value){
                    this.$store.dispatch("transactionFilters/setCategoryIds", value);
                    this.$emit('filtersChanged');
            }
        },
        tagIds: {
            get: function () {return this.$store.state.transactionFilters.tagIds },
            set: function (value){
                    this.$store.dispatch("transactionFilters/setTagIds", value);
                    this.$emit('filtersChanged');
            }
        },
        accounts() {
      return this.$store.state.accounts.accounts.items;
    },
        categories() {
      return this.$store.state.categories.categories.items;
    },
        tags() {
      return this.$store.state.tags.tags.items;
    },
    },
    created () {
        var date = new Date();
        var firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
        var lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);

         this.$store.dispatch("transactionFilters/setFrom", firstDay.toISOString().substr(0, 10));
         this.$store.dispatch("transactionFilters/setTo", lastDay.toISOString().substr(0, 10));
         this.$store.dispatch("accounts/getAll");
          this.$store.dispatch("categories/getAll");
           this.$store.dispatch("tags/getAll");
    },
    methods: {
    }
};
</script>