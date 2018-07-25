import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Animal} from "../models/animal";
import {environment} from "../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  private url : string = environment.apiBase;
  constructor(private http : HttpClient) { }

  public getAnimals(){
    return this.http.get(this.url)
  }

  public addAnimals(animal : Animal){
    let formData = new FormData();
    for(var a = 0; a < animal.training.length; a++){
      var file = animal.training.item(a);
      formData.append('files', file, file.name);
    }
    var newAnimal = JSON.parse(JSON.stringify(animal));
    newAnimal.training = null;
    formData.append('animal', JSON.stringify(newAnimal));
    return this.http.post(this.url + "/add", formData)
  }

  public deleteAnimal(animal : Animal){
    return this.http.delete(this.url + "/animals/" + animal.name)
  }

  public test(form : FormData){
    return this.http.post(this.url + "/detect", form);
  }
}
