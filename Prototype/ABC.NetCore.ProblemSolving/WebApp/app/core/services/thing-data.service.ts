import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Response } from '@angular/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map'
import { Configuration } from './../../app.constants';
import {
    CustomerGetAllResponse, Customer, CustomerCreteModel,
    SAPEmployee, SAPEmployeeGetAllResponse, SAPPart, SAPPartGetAllResponse
} from './../../models/thing';

@Injectable()
export class ThingService {

    private actionUrl: string;
    private headers: HttpHeaders;
    private serverUrl: string;

    constructor(private http: HttpClient, private configuration: Configuration) {

        this.serverUrl = configuration.Server;
        this.actionUrl = configuration.Server + 'api/customers/';

        this.headers = new HttpHeaders();
        this.headers = this.headers.set('Content-Type', 'application/json');
        this.headers = this.headers.set('Accept', 'application/json');
    }

    getAllSAPParts(offset: number, limit: number): Observable<SAPPartGetAllResponse> {
        const baseApiUrl: string = this.serverUrl + 'api/sap/parts?offset=' + offset + '&limit=' + limit;
        return this.http.get<SAPPartGetAllResponse>(baseApiUrl, { headers: this.headers });
    }

    getAllSAPEmployees(offset: number, limit: number): Observable<SAPEmployeeGetAllResponse> {
        const baseApiUrl: string = this.serverUrl + 'api/sap/employees?offset=' + offset + '&limit=' + limit;
        return this.http.get<SAPEmployeeGetAllResponse>(baseApiUrl, { headers: this.headers });
    }

    getAll(): Observable<CustomerGetAllResponse> {
        return this.http.get<CustomerGetAllResponse>(this.actionUrl, { headers: this.headers });
    }

    getSingle(id: string): Observable<Customer> {
        return this.http.get<Customer>(this.actionUrl + id, { headers: this.headers });
    }

    add(toAddObj: CustomerCreteModel): Observable<boolean> {
        const toAdd = JSON.stringify({ name: toAddObj.name, location: toAddObj.location });
        return this.http
            .post(this.actionUrl, toAdd, { headers: this.headers })
            .map((res: Response) => {
                return (res && res.status && res.status === 201) ? true : false;
            })
            .catch(err => this.handleError(err, 'Something went wrong in adding customer. Contact admin.'));
    }

    update(id: string, itemToUpdate: any): Observable<Customer> {
        return this.http
            .put<Customer>(this.actionUrl + id, JSON.stringify(itemToUpdate), { headers: this.headers });
    }

    delete(id: string): Observable<boolean> {
        return this.http
            .delete(this.actionUrl + id, { headers: this.headers })
            .map((response: Response) => { return true; })
            .catch(err => this.handleError(err, 'Something went wrong in deleting customer. Contact admin.'));
    }

    private handleError(error: Response, privateMsg: string) {
        console.log('inside error part ' + error.status + ' : ' + error.statusText);
        if (error.status === 409) {
            return Observable.throw('Already exists. Name must be uniue.');
        }
        return Observable.throw(error || (privateMsg || 'Server error'));
    }
}
