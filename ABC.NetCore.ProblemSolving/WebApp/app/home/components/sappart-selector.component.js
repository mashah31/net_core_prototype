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
import { DialogComponent, DialogService } from 'ng2-bootstrap-modal';
import { ThingService } from '../../core/services/thing-data.service';
var SAPPartSelectorComponent = (function (_super) {
    __extends(SAPPartSelectorComponent, _super);
    function SAPPartSelectorComponent(dataService, dialog) {
        var _this = _super.call(this, dialog) || this;
        _this.dataService = dataService;
        _this.dialog = dialog;
        _this.isLoading = true;
        _this.pageSize = 20;
        _this.skip = 0;
        _this.selectedMaterials = [];
        _this.gridSettings = { checkboxOnly: false, mode: 'multiple' };
        return _this;
    }
    SAPPartSelectorComponent.prototype.ngOnInit = function () {
        this.loadData();
    };
    SAPPartSelectorComponent.prototype.pageChange = function (event) {
        this.skip = event.skip;
        this.loadData();
    };
    SAPPartSelectorComponent.prototype.loadData = function () {
        var _this = this;
        this.isLoading = true;
        this.dataService.getAllSAPParts(this.skip, this.pageSize).subscribe(function (res) {
            _this.gridData = {
                data: res.values,
                total: res.size
            };
            _this.selectedMaterials = _this.alreadySelectedMaterials;
        }, function () { _this.isLoading = false; });
    };
    SAPPartSelectorComponent.prototype.submit = function () {
        this.result = (this.selectedMaterials && this.selectedMaterials.length > 0)
            ? this.selectedMaterials : this.alreadySelectedMaterials;
        this.close();
    };
    SAPPartSelectorComponent.prototype.cancel = function () {
        this.result = this.alreadySelectedMaterials;
        this.close();
    };
    return SAPPartSelectorComponent;
}(DialogComponent));
SAPPartSelectorComponent = __decorate([
    Component({
        selector: 'app-sappart-selector',
        templateUrl: './sappart-selector.component.html',
        styles: [
            "\n            .modal-dialog {\n\t            overflow-y: initial !important;\n            }\n            .modal-body{\n                height: 800px;\n                overflow-y: auto;\n            }\n            .sapparts {\n                width: 300px;\n            }\n        "
        ]
    }),
    __metadata("design:paramtypes", [ThingService, DialogService])
], SAPPartSelectorComponent);
export { SAPPartSelectorComponent };
//# sourceMappingURL=sappart-selector.component.js.map