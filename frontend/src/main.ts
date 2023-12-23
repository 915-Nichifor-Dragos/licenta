import { appConfig } from './app/core/app.config';
import { AppComponent } from './app/core/app.component';
import { bootstrapApplication } from '@angular/platform-browser';

bootstrapApplication(AppComponent, appConfig).catch((err) =>
  console.error(err)
);
