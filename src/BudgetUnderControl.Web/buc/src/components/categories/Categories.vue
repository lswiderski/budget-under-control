<template>
  <v-simple-table>
    <thead>
      <tr>
        <th class="text-left">Name</th>
      </tr>
    </thead>
    <tbody>
      <tr v-for="item in categories.items" :key="item.Id">
        <td>{{ item.name }}</td>
      </tr>
    </tbody>
  </v-simple-table>
</template>

<script>
export default {
    computed: {
         categories () {
            return this.$store.state.categories.categories;
        }
    },
    created () {
        this.$store.dispatch('categories/getAll');
    },
    methods: {
    }
};
</script>