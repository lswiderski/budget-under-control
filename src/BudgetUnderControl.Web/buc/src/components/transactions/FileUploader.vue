<template>
  <v-card-text>
    <v-container>
      <v-row>
        <v-col cols="8" class="control-section file-preview">
          <div class="control_wrapper">
            <!-- Initialize Uploader -->
           <input
          required
          type="file"
          class="form-control"
          name="Image"
          @change="fileChange"
          ref="image"
        />
          </div>
        </v-col>
      </v-row>
    </v-container>
  </v-card-text>
</template>

<script>

</script>

<style scoped>

</style>