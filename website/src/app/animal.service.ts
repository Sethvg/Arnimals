import { Injectable } from '@angular/core';
import {Animal} from "../models/animal";

@Injectable({
  providedIn: 'root'
})
export class AnimalService {

  private animals : Animal[] = [
    {
      name: 'Billy',
      img: 'http://1.bp.blogspot.com/-0gjLb0IZpXk/TnNoAkrGEsI/AAAAAAAABLQ/vNf-cJwipPA/s1600/Funny+Goat+Photos-2.jpg',
      type:'goat',
      age:20,
      weight:"50lbs",
      height: "2 feet 10 inches",
      hitCount: 1002,
      description: 'This is a billy.  He is a goat.  Goats float, and like moats.  They drive boats and wear totes.  Take notes'
    },
    {
      name: 'Bob',
      img: 'http://1.bp.blogspot.com/-0gjLb0IZpXk/TnNoAkrGEsI/AAAAAAAABLQ/vNf-cJwipPA/s1600/Funny+Goat+Photos-2.jpg',
      type: 'goat'
    },
    {
      name:'Snickers',
      img: 'http://1.bp.blogspot.com/-0gjLb0IZpXk/TnNoAkrGEsI/AAAAAAAABLQ/vNf-cJwipPA/s1600/Funny+Goat+Photos-2.jpg',
      type:'goat'
    }];

  constructor() {
  }

  getAllAnimals() {
    return this.animals;
  }

}
