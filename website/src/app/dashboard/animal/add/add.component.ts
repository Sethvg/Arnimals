import { Component, OnInit } from '@angular/core';
import {ApiService} from "../../../api.service";
import {Animal} from "../../../../models/animal";

@Component({
  selector: 'app-add',
  templateUrl: './add.component.html',
  styleUrls: ['./add.component.scss']
})
export class AddComponent implements OnInit {

  public animal : Animal = {name : '', type : '', img : ''};

  selecteFiles($event) {
    this.animal.training = $event.srcElement.files;
  }

  constructor(private http : ApiService) { }

  ngOnInit() {
  }

  public add(){
    this.http.addAnimals(this.animal).subscribe(result => {
      console.log(result);
    })
  }

}
