import { Component, OnInit } from '@angular/core';
import { faFeatherAlt } from '@fortawesome/free-solid-svg-icons';
@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

  headerIcon = faFeatherAlt;

  constructor() { }

  ngOnInit() {
  }

}
