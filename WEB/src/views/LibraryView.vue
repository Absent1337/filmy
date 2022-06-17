<template>
  <div>
    <div>
      <!--<h1>Strona w trakcie budowy</h1>-->
      <h2 class="d-flex justify-content-center">Lista Filmów</h2>
      <div class="d-flex justify-content-center">
        <button
          class="btn btn-outline-primary m-2"
          data-bs-toggle="modal"
          data-bs-target="#exampleModal"
          @click="addClick()"
        >
          Dodaj film
        </button>
      </div>
    </div>
    <div class="container">
      <table class="table table-striped">
        <thead class="table-dark">
          <tr>
            <!--<th>ID</th> -->
            <th class="text-center">Tytuł</th>
            <th class="text-center">Rok wydania</th>
            <th class="text-center">Opcje</th>
          </tr>
        </thead>
        <tbody class="table-secondary">
          <tr v-for="m in movies" :key="m.id">
            <!-- <td>{{ m.id }}</td> -->
            <td class="text-center">{{ m.title }}</td>
            <td class="text-center">{{ m.release_year }}</td>
            <td class="text-center">
              <button
                class="btn btn-outline-success mr-1"
                data-bs-toggle="modal"
                data-bs-target="#exampleModal"
                @click="editClick(m)"
              >
                Edytuj
              </button>
              <button
                class="btn btn-outline-danger mr-1"
                @click="deleteClick(m.id)"
              >
                Usuń
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
    <div
      class="modal fade"
      id="exampleModal"
      tabindex="-1"
      aria-labelledby="exampleModalLabel"
      aria-hidden="true"
    >
      <div class="modal-dialog modal-lg modal-dialog-centered">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="exampleModalLabel">{{ modalTitle }}</h5>
            <button
              class="btn-close"
              data-bs-dismiss="modal"
              aria-label="Close"
            ></button>
          </div>
          <div class="modal-body">
            <div class="input-group mb-3" v-on:submit.prevent="submit">
              <span class="input-group-text">Tytuł</span>
              <input type="text" :class="{'is-invalid': validationStatus($v.MovieTitle)}" class="form-control" v-model="$v.MovieTitle.$model" />
              <span class="input-group-text">Rok wydania</span>
              <input type="number" class="form-control" v-model="$v.ReleaseYear.$model" />
              <div v-if="!$v.MovieTitle.required" class="invalid-feedback">Pole tytuł nie może być puste</div>
            </div>
            <button
              type="button"
              v-if="MovieID == 0 && $v.MovieTitle.required"
              class="btn btn-primary"
              data-bs-dismiss="modal"
              @click="createClick()"
            >
              Dodaj
            </button>
            <button
              type="button"
              v-if="MovieID != 0 && $v.MovieTitle.required"
              class="btn btn-primary"
              data-bs-dismiss="modal"
              @click="updateClick()"
            >
              Edytuj
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import axios from "axios";
import {minLength, maxLength, between, required} from 'vuelidate/lib/validators';

export default {
  name: "LibraryView",
  data() {
    return {
      movies: [],
      modalTitle: "",
      MovieTitle: "",
      ReleaseYear: "",
      MovieID: "",
    };
  },
  validations:{
    MovieTitle: {
      required,
      minLength: minLength(1),
      maxLength: maxLength(200),
    },
    ReleaseYear:{
      minLength: minLength(1),
      maxLength: maxLength(4),
      between: between(1900,2100)
    }

  },
  methods: {
    validationStatus: function(validation){
      return typeof validation != "undefined" ? validation.$error : false;
    },
    refreshData() {
      axios
        .get("https://localhost:7017/api/Movie")
        .then((Response) => {
          this.movies = Response.data;
        })
        .catch((error) => {
          console.log(error.Response.data.error);
        });
    },
    addClick() {
      this.modalTitle = "Dodaj film";
      this.MovieID = 0;
      this.MovieTitle = "";
      this.ReleaseYear = "";
    },
    editClick(m) {
      this.modalTitle = "Edytuj film";
      this.MovieID = m.id;
      this.MovieTitle = m.title;
      this.ReleaseYear = m.release_year;
    },
    createClick() {
      axios
        .post("https://localhost:7017/api/Movie", {
          title: this.MovieTitle,
          release_year: this.ReleaseYear,
        })
        .then(() => {
          this.refreshData();
        });
    },
    updateClick() {
      axios
        .put("https://localhost:7017/api/Movie", {
          id: this.MovieID,
          title: this.MovieTitle,
          release_year: this.ReleaseYear,
        })
        .then(() => {
          this.refreshData();
        });
    },
    deleteClick(id) {
      if (!confirm("Czy na pewno chcesz to zrobić?")) {
        return;
      }
      axios.delete("https://localhost:7017/api/Movie/" + id).then(() => {
        this.refreshData();
      });
    },
  },
  mounted: function () {
    this.refreshData();
  },
  components: {},
};
</script>
<style>
</style>