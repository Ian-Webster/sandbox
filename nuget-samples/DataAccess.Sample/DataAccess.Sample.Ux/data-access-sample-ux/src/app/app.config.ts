import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient, withFetch } from '@angular/common/http';
import { provideClientHydration } from '@angular/platform-browser';
import { APOLLO_OPTIONS } from 'apollo-angular';
import { InMemoryCache } from '@apollo/client';
import { HttpLink } from 'apollo-angular/http';

export const appConfig: ApplicationConfig = {
	providers: [
		provideHttpClient(withFetch()), 
		provideRouter(routes), 
		provideClientHydration(),
		{
			provide: APOLLO_OPTIONS,
			useFactory(httpLink: HttpLink) {
			  return {
				cache: new InMemoryCache(),
				link: httpLink.create({
				  uri: 'https://localhost:7128/graphql/',
				}),
			  };
			},
			deps: [HttpLink],
		  },
	] 
};
