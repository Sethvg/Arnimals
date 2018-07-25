import {Component, Input, OnInit} from '@angular/core';
import {Animal} from "../../models/animal";
import {DomSanitizer} from '@angular/platform-browser'
import {environment} from "../../environments/environment";

@Component({
  selector: 'app-animal-info',
  templateUrl: './animal-info.component.html',
  styleUrls: ['./animal-info.component.scss']
})
export class AnimalInfoComponent implements OnInit {

  @Input()
  private _animal : Animal;

  @Input()
  public min : boolean = false;

  @Input()
  public hasDelete : boolean = false;

  private _img : any;

  constructor(private sanitization : DomSanitizer) { }

  ngOnInit() {
    this.img = this.sanitization.bypassSecurityTrustUrl(environment.apiBase + this.animal.img);
  }


  get animal(): Animal {
    return this._animal;
  }


  get img(): any {
    return this._img;
  }

  set img(value: any) {
    this._img = value;
  }
}
