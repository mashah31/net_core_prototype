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

@NgModule({
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

export class HomeModule { }
