import { Component, OnInit } from '@angular/core';

declare var $: any;

export interface RouteInfo {
    path: string;
    title: string;
    icon: string;
    class: string;
}

export const ROUTES: RouteInfo[] = [
    { path: 'home', title: 'Home', icon: 'ti-map', class: '' },
    { path: 'about', title: 'About', icon: 'ti-export', class: '' }
];

@Component({
    selector: 'app-sidebar',
    templateUrl: 'sidebar.component.html',
})
export class SidebarComponent implements OnInit {

    public menuItems: any[];

    ngOnInit(): void {
        this.menuItems = ROUTES.filter(item => item);
    }

    isNotMobileMenu() {
        if ($(window).width() > 991) {
            return false;
        }
        return true;
    }
}
