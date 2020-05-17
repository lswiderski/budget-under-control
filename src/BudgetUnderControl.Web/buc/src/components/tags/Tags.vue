<template>
  <v-data-table :headers="headers" :items="tags.items" :items-per-page="5">
    <template v-slot:item.action="{ item }">
      <v-icon small class="mr-2" @click="editItem(item)">mdi-pencil</v-icon>
      <!--<v-icon small @click="deleteItem(item)">mdi-delete</v-icon>-->
    </template>

    <template v-slot:top>
      <v-toolbar flat color="white">
        <v-toolbar-title>Tags</v-toolbar-title>
        <v-divider class="mx-6" inset vertical></v-divider>
        <div class="flex-grow-1"></div>
        <v-dialog v-model="dialog" max-width="500px">
          <template v-slot:activator="{ on }">
            <v-btn color="primary" dark class="mb-2" v-on="on">New tag</v-btn>
          </template>
          <v-card>
            <v-card-title>
              <span class="headline">{{ formTitle }}</span>
            </v-card-title>

            <v-card-text>
              <v-container>
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
                    <v-text-field v-model="editedItem.name" label="Name"></v-text-field>
                  </v-col>
                </v-row>
              </v-container>
            </v-card-text>

            <v-card-actions>
              <div class="flex-grow-1"></div>
              <v-btn color="blue darken-1" text @click="close">Cancel</v-btn>
              <v-btn color="blue darken-1" text @click="save">Save</v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>
      </v-toolbar>
    </template>
  </v-data-table>
</template>

<script>
import { tagsService } from "../../_services";

export default {
  data() {
    return {
      dialog: false,
      errors: [],
      editedIndex: -1,
      editedItem: null,
      defaultItem: {
        name: "",
        Id: null,
      },
      headers: [
        { text: "Name", value: "name" },
        { text: "Actions", sortable: false, value: "action" }
      ]
    };
  },
  computed: {
    tags() {
      return this.$store.state.tags.tags;
    },
    formTitle() {
      return this.editedIndex === -1 ? "New Tag" : "Edit Tag";
    }
  },
  created() {
    this.$store.dispatch("tags/getAll");
    this.editedItem = this.defaultItem;
  },
  methods: {
    deleteItem(item) {
      const index = this.tags.items.indexOf(item);
      confirm("Are you sure you want to delete this tag?") &&
        this.tags.items.splice(index, 1) &&
        tagsService.remove(item.externalId).then( () => {
          this.$store.dispatch("tags/getAll");
        });
    },
    editItem(item) {
      const _self = this;
      tagsService
        .get(item.externalId)
        .then(dto => {
          _self.editedIndex = _self.tags.items.indexOf(item);
          _self.editedItem = Object.assign({}, dto);
          _self.dialog = true;
        })
        .catch(errors => {
          _self.errors = errors;
        });
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
      let dto = this.editedItem;

      if (this.editedIndex > -1) {
        tagsService
          .edit(this.editedItem.externalId, dto)
          .then(() => {
            this.$store.dispatch("tags/getAll");
            this.close();
          })
          .catch(data => {
            _self.errors = data;
          });
      } else {
        tagsService
          .add(dto)
          .then( () => {
            this.tags.items.push(_self.editedItem);
            this.$store.dispatch("tags/getAll");
            this.close();
          })
          .catch(errors => {
            _self.errors = errors;
          });
      }
    }
  },
  watch: {
    dialog(val) {
      val || this.close();
    }
  }
};
</script>