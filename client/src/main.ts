import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { environment } from './environments/environment';
import { datadogRum } from '@datadog/browser-rum';

datadogRum.init({
    applicationId: '8a8bf087-c7e9-49f5-9128-f6104300a5bc',
    clientToken: 'pubd03f58f573f53cdc1b4a2bc33c5f5b5e',
    site: 'datadoghq.com',
    service: 'ExpertFinder',
//  env: 'production',
//  version: '1.0.0',
    sampleRate: 100,
    trackInteractions: true
});

if (environment.production) {
  enableProdMode();
}

platformBrowserDynamic().bootstrapModule(AppModule)
  .catch(err => console.error(err));
