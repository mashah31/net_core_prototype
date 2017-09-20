import { Component, OnInit } from '@angular/core';
import { Customer, CustomerCreteModel } from './../../models/thing';
import { DialogComponent, DialogService, DialogOptions } from 'ng2-bootstrap-modal';

export interface RegisterPromptModel {
    selected: Customer;
}

@Component({
    selector: 'app-customer-register',
    templateUrl: './customer-register.component.html',
    styles: [
        `
            .modal-dialog {
	            overflow-y: initial !important;
            }
            .modal-body{
                height: 325px;
                overflow-y: 500px;
            }
        `
    ]
})
export class CustomerRegistrationComponent
    extends DialogComponent<RegisterPromptModel, CustomerCreteModel>
    implements RegisterPromptModel, OnInit {

    selected: Customer;
    public message = 'Register Customer';
    public submitBtn = 'Register';

    public custObj: Customer = new Customer();

    constructor(private dialog: DialogService) {
        super(dialog);
    }

    ngOnInit() {
        // In case of not null selected customer
        // Ask for update of existing record
        // Fill in the information from recieved object
        if (this.selected != null) {
            this.message = 'Update Customer';
            this.submitBtn = 'Update';

            this.custObj.name = this.selected.name;
            this.custObj.location = this.selected.location;
        }
    }

    submit() {
        this.result = {
            name: this.custObj.name,
            location: this.custObj.location
        };
        this.close();
    }

    cancel() {
        this.result = null;
        this.close();
    }
}
