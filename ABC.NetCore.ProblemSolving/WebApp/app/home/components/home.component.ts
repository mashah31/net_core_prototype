import { Component, OnInit } from '@angular/core';
import { DialogService, DialogOptions } from 'ng2-bootstrap-modal';
import { ThingService } from './../../core/services/thing-data.service';
import {
    CustomerGetAllResponse, Customer,
    SAPEmployee, SAPEmployeeGetAllResponse, SAPPart, SAPPartGetAllResponse
} from './../../models/thing';
import { CustomerRegistrationComponent } from './customer-register.component';
import { SAPPartSelectorComponent } from './sappart-selector.component';
import { GridDataResult, PageChangeEvent } from '@progress/kendo-angular-grid';
import { ExcelExportData } from '@progress/kendo-angular-excel-export';

@Component({
    selector: 'app-home-component',
    templateUrl: './home.component.html',
    styles: [
        `
            .main-container {
                margin: 0 auto;
            }
            .grid-container {
                height: 500px;
            }
        `
    ]
})

export class HomeComponent implements OnInit {

    public message: string;
    public customers: Customer[] = [];
    public sapemps: SAPEmployee[] = [];
    public sapparts: SAPPart[] = [];
    public sappartsnum: Array<string> = [];
    public customer: Customer = new Customer();

    public pageSize = 20;
    public skip = 0;
    public selectedEmps: string[] = [];
    public gridData: GridDataResult;
    public gridSettings = { checkboxOnly: false, mode: 'multiple' }

    constructor(
        private dataService: ThingService,
        private dialogService: DialogService) {
        this.message = 'Test Service';
    }

    ngOnInit() {
        this.loadEmployees();
    }

    private loadCustomers() {
        this.dataService.getAll().subscribe(
            res => {
                this.customers = res.values;
            },
            error => this.handleError(error),
            () => console.log('Get all complete')
        );
    }

    pageChange(event: PageChangeEvent): void {
        console.log('page changed called with ' + event.skip);
        this.skip = event.skip;
        this.loadEmployees();
    }

    loadEmployees() {
        this.dataService.getAllSAPEmployees(this.skip, this.pageSize).subscribe(res => {
            this.gridData = {
                data: res.values,
                total: res.size
            };
        });
    }

    loadAllEmployees() {
        console.log('Entered to the all Emps');
        this.dataService.getAllSAPEmployees(0, 99999).subscribe(res => {
            const allEmps: ExcelExportData = {
                data: res.values
            }
            return allEmps;
        }, error => { console.log('Something went wrong!!'); return null; });
    }

    public addOrUpdateCustomer(customerObj: Customer) {
        this.dialogService.addDialog(CustomerRegistrationComponent, { selected: customerObj })
            .subscribe(modal => {
                if (modal) {
                    if (customerObj != null) {
                        this.dataService.update(customerObj.id, modal).subscribe(
                            res => {
                                this.loadCustomers();
                            },
                            err => this.handleError(err));
                    } else {
                        this.dataService.add(modal).subscribe(
                            res => {
                                this.loadCustomers();
                            },
                            err => this.handleError(err));
                    }
                }
            });
    }

    public popupSAPPartReg(selected: string) {
        const selectedStrArr: string[] = [selected];
        this.dialogService.addDialog(SAPPartSelectorComponent, { alreadySelectedMaterials: selectedStrArr })
            .subscribe(modal => {
                console.log('recieved: ' + modal);
            })
    }

    public deleteCustomer(custObj: Customer) {
        this.dataService.delete(custObj.id).subscribe(
            res => {
                this.loadCustomers();
            },
            error => this.handleError(error));
    }

    private handleError(error: any) {
        if (error['statusText'] != null) {
            alert(error.status + ': ' + error.statusText);
        } else {
            alert(error || 'Something went wrong!! Please contact admin.');
        }
    }
}
