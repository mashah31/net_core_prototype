import {Injectable} from '@angular/core';
import {Http, Request, Response, Headers, RequestOptionsArgs, RequestMethod} from "@angular/http";
import {RequestArgs} from "@angular/http/src/interfaces";
import {Observable} from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';

@Injectable()
export class JsonHttpHelper {
    protected headers: Headers;

    constructor(private _http: Http) {
        this.headers = new Headers();
        this.headers.append('Content-Type', 'application/json');
        this.headers.append('Accept', 'application/json');
    }

    get(url:string) : Observable<any> {
        return this._http.get(url)
            .map((res: Response) => res.json())
            .catch(this.handleError);
    }

    post(url:string, data:any, args?: RequestOptionsArgs) : Observable<any> {
        if (args == null) args = {};
        if (args.headers === undefined) args.headers = this.headers;

        return this._http.post(url, JSON.stringify(data), args)
            .map((res: Response) => JsonHttpHelper.json(res))
            .catch(this.handleError);
    }

    put(url:string, data:any, args?: RequestOptionsArgs) : Observable<any> {
        if (args == null) args = {};
        if (args.headers === undefined) args.headers = this.headers;

        return this._http.put(url, JSON.stringify(data), args)
            .map((res: Response) => JsonHttpHelper.json(res))
            .catch(this.handleError);
    }

    remove(url: string, data?: any, args?: RequestOptionsArgs): Observable<any> {
        if (args == null) args = {};

        args.url = url;
        args.method = RequestMethod.Delete;
        if (!args.headers) args.headers = this.headers;
        args.body  = data ? JSON.stringify(data) : null;

        return this._http.request(new Request(<RequestArgs>args))
            .map((res: Response) => JsonHttpHelper.json(res))
            .catch(this.handleError);
    }

    private static json(res: Response): any {
        return res.text() === "" ? res : res.json();
    }

    private handleError(error:any) {
        console.error(error);
        return Observable.throw(error);

        // The following doesn't work.
        // There's no error status at least in case of network errors.
        // WHY?!
        //
        // if ( error === undefined) error = null;
        // let errMsg = (error && error.message)
        //     ? error.message
        //     : (error && error.status)
        //         ? `${error.status} - ${error.statusText}`
        //         : error;
        //
        // return Observable.throw(errMsg);
    }
}