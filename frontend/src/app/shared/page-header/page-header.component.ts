import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-page-header',
  template: ` <h1>{{ title }}</h1> `,
  styles: [
    `
      :host {
        text-align: center;
        margin-top: 24px;
        margin-bottom: 16px;
      }

      h1 {
        font-weight: bold;
      }
    `,
  ],
  standalone: true,
})
export class AppPageHeaderComponent {
  @Input() title: string = '';
}
