export class CustomerGetAllResponse {
    public href: string;
    public rel: string[];
    public offset: number;
    public limit: number;
    public size: number;
    public first: Link;
    public values: Customer[];
}

export class Link {
    public href: string;
    public rel: string[];
}

export class Customer {
    public href: string;
    public id: string;
    public name: string;
    public location: string;
}

export class CustomerCreteModel {
    public name: string;
    public location: string;
}

export class SAPPart {
    public sapMaterialNum: string;
    public companyNumber: string;
    public partDesc: string;
}

export class SAPPartGetAllResponse {
    public href: string;
    public rel: string[];
    public offset: number;
    public limit: number;
    public size: number;
    public first: Link;
    public values: SAPPart[];
}

export class SAPEmployee {
    public userName: string;
    public lastName: string;
    public firstName: string;
    public costCenter: string;
    public orgUnitNumber: string;
    public department: string;
}

export class SAPEmployeeGetAllResponse {
    public href: string;
    public rel: string[];
    public offset: number;
    public limit: number;
    public size: number;
    public first: Link;
    public values: SAPEmployee[];
}
