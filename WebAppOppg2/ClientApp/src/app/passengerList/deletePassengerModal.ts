import { Component } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  templateUrl: 'deletePassengerModal.html'
})
export class PassengerModal {
  constructor(public modal: NgbActiveModal) { }
}