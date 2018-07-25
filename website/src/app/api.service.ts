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
    return this.http.get(this.url + "/animals")
  }

  public addAnimals(animal : Animal){
    return this.http.post(this.url + "/add", animal)
  }

  public deleteAnimal(animal : Animal){
    return this.http.delete(this.url + "/animals/" + animal.name)
  }

  public test(form : FormData){
    return this.http.post(this.url + "/detect", form);
  }
}
