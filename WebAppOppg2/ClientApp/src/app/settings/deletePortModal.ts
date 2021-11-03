import { Component } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  templateUrl: 'deletePortModal.html'
})
export class PortModal {
  constructor(public modal: NgbActiveModal) { }
}