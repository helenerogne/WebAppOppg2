import { Component } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  templateUrl: 'deleteTicketModal.html'
})
export class TicketModal {
  constructor(public modal: NgbActiveModal) { }
}