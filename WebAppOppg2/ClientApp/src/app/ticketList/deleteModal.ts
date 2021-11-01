import { Component } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  templateUrl: 'deleteModal.html'
})
export class Modal {
  constructor(public modal: NgbActiveModal) { }
}