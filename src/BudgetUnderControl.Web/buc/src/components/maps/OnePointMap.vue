<template>
  <div style="height: 200px; width: 100%">
    <l-map
      ref="transactionMap"
      :zoom="zoom"
      :center="center"
      :options="mapOptions"
      @click="innerClick"
    >
      <l-tile-layer :url="url" :attribution="attribution" />
      <l-marker :lat-lng="markerLocation">
      </l-marker>
    </l-map>
  </div>
</template>

<script>
import { latLng } from "leaflet";
import { LMap, LTileLayer, LMarker } from "vue2-leaflet";

export default {
  name: "OnePointMap",
  components: {
    LMap,
    LTileLayer,
    LMarker,
  },
  props: {
    latitude: Number,
    longitude: Number,
    centerLatitude: Number,
    centerLongitude: Number,
  },

  data() {
    return {
      zoom: 13,
      lat: 52,
      lng: 21,
      url: "https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png",
      attribution:
        '&copy; <a href="http://osm.org/copyright">OpenStreetMap</a> contributors',
      mapOptions: {
        zoomSnap: 0.5
      }
    };
  },
  computed: {
    markerVisible() {
      return this.lat !== null;
    },
    markerLocation() {
      return latLng(this.lat, this.lng);
    },
    center() {
      return latLng(this.centerLatitude, this.centerLongitude);
    }
  },
  methods: {
    innerClick(e) {
      this.lat = e.latlng.lat;
      this.lng = e.latlng.lng;
      this.$emit("coordsChanged", { lat: this.lat, lng: this.lng });
    },

    invalideSize() {  
      this.lat = this.latitude;
      this.lng = this.longitude;
      this.$refs.transactionMap.mapObject.invalidateSize();
    }
  }
};
</script>
<style lang="sass" scoped>

</style>