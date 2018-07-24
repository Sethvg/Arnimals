import {Component, OnDestroy, OnInit} from '@angular/core';
import {AnimalService} from "../../animal.service";
import {NavigationEnd, Router} from "@angular/router";
import {Subscription} from "rxjs/internal/Subscription";
import {Animal} from "../../../models/animal";

@Component({
  selector: 'app-animal',
  templateUrl: './animal.component.html',
  styleUrls: ['./animal.component.scss']
})
export class AnimalComponent implements OnInit, OnDestroy {

  private subs : Subscription[] = [];
  private animal: Animal;

  constructor(private animalService : AnimalService, private router: Router) {

    this.subs.push(this.router.events.subscribe((event) => {
      if(event instanceof NavigationEnd) {
        var tree = this.router.parseUrl(event.urlAfterRedirects);
        var segments = tree.root.children.primary.segments;

        for(let a of animalService.getAllAnimals()){
          if(a.name == segments[3].path){
            this.animal = a;
          }
        }
      }
    }));


  }

  ngOnInit() {
  }

  ngOnDestroy(): void {
    if(this.subs != null){
      this.subs.forEach(sub => {
        sub.unsubscribe();
      })
    }
  }

  getAnimal() {
    return this.animal;
  }
}
