var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HomeComponent } from './components/home.component';
import { CustomerRegistrationComponent } from './components/customer-register.component';
import { SAPPartSelectorComponent } from './components/sappart-selector.component';
import { HomeRoutes } from './home.routes';
import { BootstrapModalModule } from 'ng2-bootstrap-modal';
import { GridModule, ExcelModule } from '@progress/kendo-angular-grid';
import { DropDownsModule, AutoCompleteModule, ComboBoxModule } from '@progress/kendo-angular-dropdowns';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { PDFExportModule } from '@progress/kendo-angular-pdf-export';
var HomeModule = (function () {
    function HomeModule() {
    }
    return HomeModule;
}());
HomeModule = __decorate([
    NgModule({
        imports: [
            CommonModule,
            FormsModule,
            HttpClientModule,
            HomeRoutes,
            BootstrapModalModule, BrowserAnimationsModule, PDFExportModule,
            GridModule, ExcelModule, DropDownsModule, AutoCompleteModule, ComboBoxModule
        ],
        declarations: [
            HomeComponent,
            CustomerRegistrationComponent,
            SAPPartSelectorComponent
        ],
        exports: [
            HomeComponent
        ],
        entryComponents: [
            CustomerRegistrationComponent,
            SAPPartSelectorComponent
        ]
    })
], HomeModule);
export { HomeModule };
//# sourceMappingURL=home.module.js.map