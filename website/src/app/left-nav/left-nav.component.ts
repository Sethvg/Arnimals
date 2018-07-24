import {Component, OnDestroy, OnInit} from '@angular/core';
import {Animal} from "../../models/animal";
import {ActivatedRoute, NavigationEnd, NavigationStart, Router, UrlSegment} from '@angular/router';
import {Subscription} from "rxjs/internal/Subscription";
import {AnimalService} from "../animal.service";


@Component({
  selector: 'app-left-nav',
  templateUrl: './left-nav.component.html',
  styleUrls: ['./left-nav.component.scss']
})
export class LeftNavComponent implements OnInit, OnDestroy {
  private curUrl: string;

  subs : Subscription[] = [];
  private r1: string;
  private r2: string;

  constructor(private router: Router, private animalService : AnimalService) {

    this.subs.push(this.router.events.subscribe((event) => {
      if(event instanceof NavigationEnd) {
        this.curUrl = event.urlAfterRedirects;
        var tree = this.router.parseUrl(this.curUrl);
        var segments = tree.root.children.primary.segments;

         this.r1 = segments.length > 2 ? segments[2].path : "";
         this.r2 = segments.length > 3 ? segments[3].path : "";
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

  public isSelected(query : string, ){
    return this.r1 == query;
  }

  public isActive(q1 : string, q2 : string){
    return this.r1 == q1 && q2 == this.r2;
  }

  public getAnimals() : Animal[]{
    return this.animalService.getAllAnimals();
  }

}
