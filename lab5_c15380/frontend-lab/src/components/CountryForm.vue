<template>
  <div class="d-flex justify-content-center align-items-center vh-100">
    <div class="card p-4 shadow" style="max-width: 400px; width: 100%">
      <h3 class="text-center">Formulario de creación de países</h3>
      <form @submit.prevent="saveCountry">
        <div class="form-group">
          <label>Nombre:</label>
          <input
            v-model="formData.Name"
            type="text"
            class="form-control"
            required />
        </div>
        <div class="form-group">
          <label for="continente">Continente:</label>
          <select
            v-model="formData.Continent"
            id="continente"
            required
            class="form-control">
            <option disabled value="">Seleccione un continente</option>
            <option>África</option>
            <option>Asia</option>
            <option>Europa</option>
            <option>América</option>
            <option>Oceanía</option>
            <option>Antártida</option>
          </select>
        </div>
        <div class="form-group">
          <label>Idioma:</label>
          <input
            v-model="formData.Language"
            type="text"
            class="form-control"
            required />
        </div>
        <div>
            <button type="submit" class="btn btn-success btn-block">
                Guardar
            </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script>
import axios from "axios";

export default {
  data() {
    return {
      formData: { Name: "", Continent: "", Language: "" },
    };
  },
  methods: {
    saveCountry() {
      console.log("Datos a guardar:", this.formData);
      axios
        .post("http://localhost:5279/api/Country", this.formData)
        .then(function (response) {
            console.log(response);
            window.location.href = "/";
        })
        .catch(function (error) {
            console.log("Error: ", error);
        });
    }
  }
};
</script>
