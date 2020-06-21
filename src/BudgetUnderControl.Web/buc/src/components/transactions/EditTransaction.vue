<template>
  <div>
    <template v-slot:top>
      <v-toolbar flat color="white">
        <v-toolbar-title>Transactions</v-toolbar-title>
        <v-divider class="mx-6" inset vertical></v-divider>
        <div class="flex-grow-1"></div>
        <v-dialog v-model="dialog" max-width="1000px">
          <template v-slot:activator="{ on }">
            <v-btn color="primary" dark class="mb-2" v-on="on">New transaction</v-btn>
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
                            <v-select
                              :items="types"
                              :value="1"
                              v-model="editedItem.type"
                              label="Type"
                            ></v-select>
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
                <div>
                  <div class="col-lg-8 control-section file-preview">
                    <div class="control_wrapper">
                      <!-- Initialize Uploader -->
                      <div
                        id="dropArea"
                        class="uploader-image-preview-drop-area"
                        style="height: auto; overflow: auto"
                      >
                        <span id="dropPreview" class="uploader-image-preview-drop-preview">
                          Drop image (JPG, PNG) files here or
                          <a href id="browse">
                            <u>Browse</u>
                          </a>
                        </span>
                        <ejs-uploader
                          id="imagePreview"
                          name="UploadFiles"
                          :asyncSettings="path"
                          ref="uploadObj"
                          :allowedExtensions="extensions"
                          :dropArea="dropElement"
                          :selected="onFileSelect"
                          :progress="onFileUpload"
                          :success="onUploadSuccess"
                          :failure="onUploadFailed"
                          :removing="onFileRemove"
                          cssClass="uploader-preview"
                        ></ejs-uploader>
                      </div>
                    </div>
                  </div>
                  <div class="col-lg-4 property-section">
                    <div id="property" title="Properties">
                      <div style="margin-left: 50px; padding-top:25px;">
                        <ejs-button id="clearbtn" style="width:130px">Clear All</ejs-button>
                      </div>
                      <div style="margin-left: 50px; padding-top:25px;">
                        <ejs-button id="uploadbtn" style="width:130px">Upload All</ejs-button>
                      </div>
                    </div>
                  </div>
                </div>
              </v-tab-item>
            </v-tabs-items>
            <v-card-actions>
              <div class="flex-grow-1"></div>
              <v-btn color="blue darken-1" text @click="close">Cancel</v-btn>
              <v-btn color="blue darken-1" text @click="save">Save</v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>
      </v-toolbar>
    </template>
  </div>
</template>
<script>
export default {
  name: "EditTransaction"
};
</script>
<style scoped>
</style>