<template>
  <div>
    <div>
      <h2 class="d-flex justify-content-center">Moje filmy</h2>
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
            <th class="text-center">Tytuł</th>
            <th class="text-center">Rok produkcji</th>
            <th class="text-center">Opcje</th>
          </tr>
        </thead>
        <tbody class="table-secondary">
          <tr v-for="m in movies" :key="m.id">
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
            <div class="input-group mb-3">
              <span class="input-group-text">Tytuł</span>
              <input
                type="text"
                :class="{ 'is-invalid': $v.MovieTitle.$error }"
                class="form-control"
                v-model="$v.MovieTitle.$model"
              />
              <span class="input-group-text">Rok wydania</span>
              <input
                type="number"
                :class="{ 'is-invalid': $v.ReleaseYear.$error }"
                class="form-control"
                v-model="$v.ReleaseYear.$model"
              />
              <span v-if="!$v.MovieTitle.required" class="invalid-feedback"
                >Pole tytuł nie może być puste</span
              >
              <span v-if="!$v.MovieTitle.maxLength" class="invalid-feedback"
                >Pole tytuł nie może zawierać wiecej niż 200 znaków</span
              >
              <span v-if="!$v.ReleaseYear.maxLength" class="invalid-feedback"
                >Podaj poprawny rok</span
              >
              <span v-if="!$v.ReleaseYear.between" class="invalid-feedback"
                >Podaj rok pomiędzy 1900-2100</span
              >
            </div>
            <button
              type="button"
              v-if="
                MovieID == 0 &&
                $v.MovieTitle.required &&
                $v.MovieTitle.maxLength &&
                $v.ReleaseYear.maxLength &&
                $v.ReleaseYear.between
              "
              class="btn btn-primary"
              data-bs-dismiss="modal"
              @click="createClick()"
            >
              Dodaj
            </button>
            <button
              type="button"
              v-if="
                MovieID != 0 &&
                $v.MovieTitle.required &&
                $v.MovieTitle.maxLength &&
                $v.ReleaseYear.maxLength &&
                $v.ReleaseYear.between
              "
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
import { maxLength, between, required } from "vuelidate/lib/validators";

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
  validations: {
    MovieTitle: {
      required,
      maxLength: maxLength(200),
    },
    ReleaseYear: {
      maxLength: maxLength(4),
      between: between(1900, 2100),
    },
  },
  methods: {
    validationStatus: function (validation) {
      return typeof validation != "undefined" ? validation.$error : false;
    },
    refreshData() {
      axios
        .get('movie')
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
        .post('movie', {
          title: this.MovieTitle,
          release_year: this.ReleaseYear,
        })
        .then(() => {
          this.refreshData();
        })
        .catch((error) => {
          console.log(error.Response.data.error);
        });
    },
    updateClick() {
      axios
        .put('movie', {
          id: this.MovieID,
          title: this.MovieTitle,
          release_year: this.ReleaseYear,
        })
        .then(() => {
          this.refreshData();
        })
        .catch((error) => {
          console.log(error.Response.data.error);
        });
    },
    deleteClick(id) {
      if (!confirm("Czy na pewno chcesz to zrobić?")) {
        return;
      }
      axios
        .delete('movie/' + id)
        .then(() => {
          this.refreshData();
        })
        .catch((error) => {
          console.log(error.Response.data.error);
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