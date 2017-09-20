var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { Component } from '@angular/core';
import { DialogService } from 'ng2-bootstrap-modal';
import { ThingService } from './../../core/services/thing-data.service';
import { Customer } from './../../models/thing';
import { CustomerRegistrationComponent } from './customer-register.component';
import { SAPPartSelectorComponent } from './sappart-selector.component';
var HomeComponent = (function () {
    function HomeComponent(dataService, dialogService) {
        this.dataService = dataService;
        this.dialogService = dialogService;
        this.customers = [];
        this.sapemps = [];
        this.sapparts = [];
        this.sappartsnum = [];
        this.customer = new Customer();
        this.pageSize = 20;
        this.skip = 0;
        this.selectedEmps = [];
        this.gridSettings = { checkboxOnly: false, mode: 'multiple' };
        this.message = 'Test Service';
    }
    HomeComponent.prototype.ngOnInit = function () {
        this.loadEmployees();
    };
    HomeComponent.prototype.loadCustomers = function () {
        var _this = this;
        this.dataService.getAll().subscribe(function (res) {
            _this.customers = res.values;
        }, function (error) { return _this.handleError(error); }, function () { return console.log('Get all complete'); });
    };
    HomeComponent.prototype.pageChange = function (event) {
        console.log('page changed called with ' + event.skip);
        this.skip = event.skip;
        this.loadEmployees();
    };
    HomeComponent.prototype.loadEmployees = function () {
        var _this = this;
        this.dataService.getAllSAPEmployees(this.skip, this.pageSize).subscribe(function (res) {
            _this.gridData = {
                data: res.values,
                total: res.size
            };
        });
    };
    HomeComponent.prototype.loadAllEmployees = function () {
        console.log('Entered to the all Emps');
        this.dataService.getAllSAPEmployees(0, 99999).subscribe(function (res) {
            var allEmps = {
                data: res.values
            };
            return allEmps;
        }, function (error) { console.log('Something went wrong!!'); return null; });
    };
    HomeComponent.prototype.addOrUpdateCustomer = function (customerObj) {
        var _this = this;
        this.dialogService.addDialog(CustomerRegistrationComponent, { selected: customerObj })
            .subscribe(function (modal) {
            if (modal) {
                if (customerObj != null) {
                    _this.dataService.update(customerObj.id, modal).subscribe(function (res) {
                        _this.loadCustomers();
                    }, function (err) { return _this.handleError(err); });
                }
                else {
                    _this.dataService.add(modal).subscribe(function (res) {
                        _this.loadCustomers();
                    }, function (err) { return _this.handleError(err); });
                }
            }
        });
    };
    HomeComponent.prototype.popupSAPPartReg = function (selected) {
        var selectedStrArr = [selected];
        this.dialogService.addDialog(SAPPartSelectorComponent, { alreadySelectedMaterials: selectedStrArr })
            .subscribe(function (modal) {
            console.log('recieved: ' + modal);
        });
    };
    HomeComponent.prototype.deleteCustomer = function (custObj) {
        var _this = this;
        this.dataService.delete(custObj.id).subscribe(function (res) {
            _this.loadCustomers();
        }, function (error) { return _this.handleError(error); });
    };
    HomeComponent.prototype.handleError = function (error) {
        if (error['statusText'] != null) {
            alert(error.status + ': ' + error.statusText);
        }
        else {
            alert(error || 'Something went wrong!! Please contact admin.');
        }
    };
    return HomeComponent;
}());
HomeComponent = __decorate([
    Component({
        selector: 'app-home-component',
        templateUrl: './home.component.html',
        styles: [
            "\n            .main-container {\n                margin: 0 auto;\n            }\n            .grid-container {\n                height: 500px;\n            }\n        "
        ]
    }),
    __metadata("design:paramtypes", [ThingService,
        DialogService])
], HomeComponent);
export { HomeComponent };
//# sourceMappingURL=home.component.js.map