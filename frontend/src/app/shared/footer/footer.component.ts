import { Component } from '@angular/core';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styles: [
    `
      #custom-footer {
        width: 100%;
        padding: 0.5vh;
        background-color: teal;
        color: white;
        position: fixed;
        bottom: 0;
        font-size: 12px;
        text-align: center;
      }
    `,
  ],
  standalone: true,
})
export class FooterComponent {}
