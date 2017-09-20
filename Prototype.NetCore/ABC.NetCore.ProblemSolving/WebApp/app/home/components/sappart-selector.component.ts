import { Component, OnInit } from '@angular/core';
import { SAPPart, SAPPartGetAllResponse } from './../../models/thing';
import { DialogComponent, DialogService, DialogOptions } from 'ng2-bootstrap-modal';
import { ThingService } from '../../core/services/thing-data.service';
import { GridDataResult, PageChangeEvent } from '@progress/kendo-angular-grid';

export interface SAPPartModel {
    alreadySelectedMaterials: string[];
}

@Component({
    selector: 'app-sappart-selector',
    templateUrl: './sappart-selector.component.html',
    styles: [
        `
            .modal-dialog {
	            overflow-y: initial !important;
            }
            .modal-body{
                height: 800px;
                overflow-y: auto;
            }
            .sapparts {
                width: 300px;
            }
        `
    ]
})
export class SAPPartSelectorComponent
    extends DialogComponent<SAPPartModel, string[]>
    implements SAPPartModel, OnInit {
    alreadySelectedMaterials: string[];
    private isLoading = true;

    public pageSize = 20;
    public skip = 0;
    public selectedMaterials: string[] = [];
    public gridData: GridDataResult;
    public gridSettings = { checkboxOnly: false, mode: 'multiple' }

    constructor(private dataService: ThingService, private dialog: DialogService) {
        super(dialog);
    }

    ngOnInit() {
        this.loadData();
    }

    pageChange(event: PageChangeEvent): void {
        this.skip = event.skip;
        this.loadData();
    }

    loadData() {
        this.isLoading = true;
        this.dataService.getAllSAPParts(this.skip, this.pageSize).subscribe(res => {
            this.gridData = {
                data: res.values,
                total: res.size
            };
            this.selectedMaterials = this.alreadySelectedMaterials;
        }, () => { this.isLoading = false; });
    }

    submit() {
        this.result = (this.selectedMaterials && this.selectedMaterials.length > 0)
                ? this.selectedMaterials : this.alreadySelectedMaterials;
        this.close();
    }

    cancel() {
        this.result = this.alreadySelectedMaterials;
        this.close();
    }
}
