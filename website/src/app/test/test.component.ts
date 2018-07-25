import { Component, OnInit } from '@angular/core';
import {ApiService} from "../api.service";
import {Animal} from "../../models/animal";

@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.scss']
})
export class TestComponent implements OnInit {

  constructor(private apiService : ApiService) { }

  public files : FileList;

  public map : {[i : string] : Animal};

  ngOnInit() {
  }

  selecteFiles($event) {
    this.files = $event.srcElement.files;
  }

  test(){

    this.map = {};

    for(var a = 0; a < this.files.length; a++){
      let file = this.files[a];
      let formData = new FormData();
      formData.append('files', file, file.name);
      this.apiService.test(formData).subscribe((result : Animal) => {
        this.map[file.name] = result;
      });
    };
  }

  public getKeys(){
    return this.map != null ? Object.keys(this.map) : [];
  }
}
