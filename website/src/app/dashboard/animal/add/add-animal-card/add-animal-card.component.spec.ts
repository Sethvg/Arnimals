import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddAnimalCardComponent } from './add-animal-card.component';

describe('AddAnimalCardComponent', () => {
  let component: AddAnimalCardComponent;
  let fixture: ComponentFixture<AddAnimalCardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddAnimalCardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddAnimalCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
