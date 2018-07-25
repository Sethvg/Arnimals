import {Component, OnDestroy, OnInit} from '@angular/core';
import {AnimalService} from "../../animal.service";
import {ActivatedRoute, NavigationEnd, Router} from "@angular/router";
import {Subscription} from "rxjs/internal/Subscription";

@Component({
  selector: 'app-all',
  templateUrl: './all.component.html',
  styleUrls: ['./all.component.scss']
})
export class AllComponent implements OnInit, OnDestroy {

  private subs : Subscription[] = [];
  public hasChild : boolean = false;

  constructor(private animalService : AnimalService, private router: Router) {

    this.subs.push(this.router.events.subscribe((event) => {
      if(event instanceof NavigationEnd) {
        var tree = this.router.parseUrl(event.urlAfterRedirects);
        var segments = tree.root.children.primary.segments;

        this.hasChild = segments.length > 3;
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

  getAnimals(){
    return this.animalService.getAllAnimals();
  }

}
