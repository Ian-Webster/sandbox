import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient, withFetch } from '@angular/common/http';
import { graphqlProvider } from './graphql.provider';
import { provideAnimations } from '@angular/platform-browser/animations';

export const appConfig: ApplicationConfig = {
	providers: [
		provideHttpClient(withFetch()),
		provideRouter(routes),
		provideHttpClient(),
		graphqlProvider,
		provideAnimations()
	]
};
