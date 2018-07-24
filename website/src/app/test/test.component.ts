import { Component, OnInit } from '@angular/core';
import {ApiService} from "../api.service";

@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.scss']
})
export class TestComponent implements OnInit {

  constructor(private apiService : ApiService) { }

  public files : File[] = [];

  ngOnInit() {
  }

  selecteFiles($event) {
    this.files = $event.srcElement.files;
  }
}
