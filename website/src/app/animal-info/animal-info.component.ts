import {Component, Input, OnInit} from '@angular/core';
import {Animal} from "../../models/animal";

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

  constructor() { }

  ngOnInit() {
  }


  get animal(): Animal {
    return this._animal;
  }
}
