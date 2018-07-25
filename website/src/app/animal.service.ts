import { Injectable } from '@angular/core';
import {Animal} from "../models/animal";
import {ApiService} from "./api.service";

@Injectable({
  providedIn: 'root'
})
export class AnimalService {

  private animals : Animal[];

  constructor(private apiService : ApiService) {
    this.apiService.getAnimals().subscribe((resp : Animal[]) => {
      this.animals = resp;
    })
  }

  getAllAnimals() {
    return this.animals;
  }

}
