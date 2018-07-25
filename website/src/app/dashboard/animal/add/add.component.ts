import { Component, OnInit } from '@angular/core';
import {ApiService} from "../../../api.service";
import {Animal} from "../../../../models/animal";
import {AnimalService} from "../../../animal.service";
import {NavigationEnd, Router} from "@angular/router";
import {Subscription} from "rxjs/internal/Subscription";

@Component({
  selector: 'app-add',
  templateUrl: './add.component.html',
  styleUrls: ['./add.component.scss']
})
export class AddComponent implements OnInit {

  public animal : Animal = {id: '', name : '', type : '', img : ''};
  private subs : Subscription[] = [];
  private name: string;

  selecteFiles($event) {
    this.animal.training = $event.srcElement.files;
  }

  constructor(private http : ApiService, private animalService : AnimalService, private router : Router) {
    this.subs.push(this.router.events.subscribe((event) => {
      if(event instanceof NavigationEnd) {
        var curUrl = event.urlAfterRedirects;
        var tree = this.router.parseUrl(curUrl);
        var segments = tree.root.children.primary.segments;

        this.name = segments.length > 2 ? segments[2].path : "";
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

  public add(){
    this.http.addAnimals(this.animal).subscribe((result : Animal) => {
      this.animalService.getAllAnimals().push(result);
      this.router.navigate(['dashboard', this.name, 'animals']);
    })
  }

}
