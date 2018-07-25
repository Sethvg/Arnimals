import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Animal} from "../models/animal";

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  private url : string = "http://mabriste-pvm0.westcentralus.cloudapp.azure.com:5000/api/animal"
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

  public test(file : File){
    return this.http.post(this.url + "/detect", file);
  }
}
