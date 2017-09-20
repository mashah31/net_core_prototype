var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { Injectable } from '@angular/core';
import { Http, Request, Headers, RequestMethod } from "@angular/http";
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
var JsonHttpHelper = JsonHttpHelper_1 = (function () {
    function JsonHttpHelper(_http) {
        this._http = _http;
        this.headers = new Headers();
        this.headers.append('Content-Type', 'application/json');
        this.headers.append('Accept', 'application/json');
    }
    JsonHttpHelper.prototype.get = function (url) {
        return this._http.get(url)
            .map(function (res) { return res.json(); })
            .catch(this.handleError);
    };
    JsonHttpHelper.prototype.post = function (url, data, args) {
        if (args == null)
            args = {};
        if (args.headers === undefined)
            args.headers = this.headers;
        return this._http.post(url, JSON.stringify(data), args)
            .map(function (res) { return JsonHttpHelper_1.json(res); })
            .catch(this.handleError);
    };
    JsonHttpHelper.prototype.put = function (url, data, args) {
        if (args == null)
            args = {};
        if (args.headers === undefined)
            args.headers = this.headers;
        return this._http.put(url, JSON.stringify(data), args)
            .map(function (res) { return JsonHttpHelper_1.json(res); })
            .catch(this.handleError);
    };
    JsonHttpHelper.prototype.remove = function (url, data, args) {
        if (args == null)
            args = {};
        args.url = url;
        args.method = RequestMethod.Delete;
        if (!args.headers)
            args.headers = this.headers;
        args.body = data ? JSON.stringify(data) : null;
        return this._http.request(new Request(args))
            .map(function (res) { return JsonHttpHelper_1.json(res); })
            .catch(this.handleError);
    };
    JsonHttpHelper.json = function (res) {
        return res.text() === "" ? res : res.json();
    };
    JsonHttpHelper.prototype.handleError = function (error) {
        console.error(error);
        return Observable.throw(error);
    };
    return JsonHttpHelper;
}());
JsonHttpHelper = JsonHttpHelper_1 = __decorate([
    Injectable(),
    __metadata("design:paramtypes", [Http])
], JsonHttpHelper);
export { JsonHttpHelper };
var JsonHttpHelper_1;
//# sourceMappingURL=http.service.ts.js.map