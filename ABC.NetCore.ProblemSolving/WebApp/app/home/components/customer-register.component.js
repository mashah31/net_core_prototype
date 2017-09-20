var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
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
import { Customer } from './../../models/thing';
import { DialogComponent, DialogService } from 'ng2-bootstrap-modal';
var CustomerRegistrationComponent = (function (_super) {
    __extends(CustomerRegistrationComponent, _super);
    function CustomerRegistrationComponent(dialog) {
        var _this = _super.call(this, dialog) || this;
        _this.dialog = dialog;
        _this.message = 'Register Customer';
        _this.submitBtn = 'Register';
        _this.custObj = new Customer();
        return _this;
    }
    CustomerRegistrationComponent.prototype.ngOnInit = function () {
        if (this.selected != null) {
            this.message = 'Update Customer';
            this.submitBtn = 'Update';
            this.custObj.name = this.selected.name;
            this.custObj.location = this.selected.location;
        }
    };
    CustomerRegistrationComponent.prototype.submit = function () {
        this.result = {
            name: this.custObj.name,
            location: this.custObj.location
        };
        this.close();
    };
    CustomerRegistrationComponent.prototype.cancel = function () {
        this.result = null;
        this.close();
    };
    return CustomerRegistrationComponent;
}(DialogComponent));
CustomerRegistrationComponent = __decorate([
    Component({
        selector: 'app-customer-register',
        templateUrl: './customer-register.component.html',
        styles: [
            "\n            .modal-dialog {\n\t            overflow-y: initial !important;\n            }\n            .modal-body{\n                height: 325px;\n                overflow-y: 500px;\n            }\n        "
        ]
    }),
    __metadata("design:paramtypes", [DialogService])
], CustomerRegistrationComponent);
export { CustomerRegistrationComponent };
//# sourceMappingURL=customer-register.component.js.map