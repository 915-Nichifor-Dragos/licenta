import { Component } from '@angular/core';

import { RouterModule } from '@angular/router';

import { FooterComponent } from '../shared/footer/footer.component';
import { NavbarComponent } from '../shared/navbar/navbar.component';

@Component({
  selector: 'app-root',
  styleUrls: ['./app.component.scss'],
  standalone: true,
  imports: [RouterModule, FooterComponent, NavbarComponent],
  template: `
    <app-navbar></app-navbar>
    <router-outlet></router-outlet>
    <app-footer></app-footer>
  `,
})
export class AppComponent {}
