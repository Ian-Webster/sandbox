# DataAccess.Sample.Ux
## Introduction
This is an Angular project and it's purpose is to test interactions with the [DataAccess.Sample.Web](https://github.com/Ian-Webster/sandbox/tree/main/nuget-samples/DataAccess.Sample/DataAccess.Sample.Web) project from a front-end perspective.

## Routes
* /movie loads movies via GraphQL

## GraphQL setup
This project leverages the [GraphQL Code Generator](https://the-guild.dev/graphql/codegen) library (GitHub link [here](https://github.com/dotansimha/graphql-code-generator)) to create typings for the various GraphQL plumbing required to interact with the API.

The following are the setup steps that were taken to set up the library (instructions were mostly taken from [here](https://the-guild.dev/graphql/codegen/docs/guides/angular) but mixed in with some troubleshooting steps also);
1. Installed the various NPM packages required for the library into the project;
    ```
    npm i graphql
    npm i -D typescript
    npm i -D @graphql-codegen/cli
    npm i -D @graphql-codegen/typescript
    npm i -D @graphql-codegen/typescript-operations
    npm i -D @graphql-codegen/typescript-apollo-angular
    ```
2. Create your .graphql queries, as an example here is a query that selects all movies;
```
query getAllMovies{
  movies (order: [{name:ASC}]),
  {
    movieId,
    name
  }
}
```
Couple of things to note here;
* Save the file as {some-thing}.graphql in your project
* Make sure you give your query a name (in my case I used "getAllMovies")
* You should test the query work either by using vanilla apollo or by using a GraphQL runner (the one [built into the HotChocolate library](https://chillicream.com/products/bananacakepop) for instance)
3. Created a typescript file named "codegen.ts" in the root of the project;
    ```typescript
    import type { CodegenConfig } from '@graphql-codegen/cli'
    
    const config: CodegenConfig = {
      overwrite: true,
      schema: 'https://uri-to-your-graphql-server/',
      documents: './path-to-your-queries/*.graphql',
      generates: {
        './path-to-where-your-want-your-generated-file/generated.ts': {
          plugins: ['typescript', 'typescript-operations', 'typescript-apollo-angular']
        }
      },
      //ignoreNoDocuments: true,
    }
    export default config
    ```
4. Modified the package.json scripts block to include a new command named generate;
    ```json
      "scripts": {
        "ng": "ng",
        "start": "ng serve",
        "build": "ng build",
        "watch": "ng build --watch --configuration development",
        "test": "ng test",
        "generate": "set NODE_TLS_REJECT_UNAUTHORIZED=0&& graphql-codegen --config codegen.ts" <-- new value
      }
    ```
    Note: we have to set the `NODE_TLS_REJECT_UNAUTHORIZED` environment setting to false to fix an issue introduced in [this PR](https://github.com/dotansimha/graphql-code-generator/issues/1806) that changed the default behaviour of the library to reject self-signed SSL certs (see this [comment](https://github.com/dotansimha/graphql-code-generator/issues/1785#issuecomment-493976501) for details) and because localhost .net certs are self certifcated the library will refused to communicate without the setting being set to false
5. Run the [DataAccess.Sample.Web](https://github.com/Ian-Webster/sandbox/tree/main/nuget-samples/DataAccess.Sample/DataAccess.Sample.Web)
6. Next you'll need to set up several configuration files for Apollo, you can run `ng add apollo-angular` (see https://the-guild.dev/graphql/apollo-angular/docs/get-started) to have these automatically created. You need a graphql.provider.ts file in the root of your app folder, it should look like this;
    ```typescript
	import { Apollo, APOLLO_OPTIONS } from 'apollo-angular';
	import { HttpLink } from 'apollo-angular/http';
	import { ApplicationConfig, inject } from '@angular/core';
	import { ApolloClientOptions, InMemoryCache } from '@apollo/client/core';

	const uri = 'https://localhost:7128/graphql/'; // <-- add the URL of the GraphQL server here
	export function apolloOptionsFactory(): ApolloClientOptions<any> {
		const httpLink = inject(HttpLink);
		return {
			link: httpLink.create({ uri }),
			cache: new InMemoryCache(),
		};
	}

	export const graphqlProvider: ApplicationConfig['providers'] = [
		Apollo,
		{
			provide: APOLLO_OPTIONS,
			useFactory: apolloOptionsFactory,
		},
	];
    ```
	if you make use of the Apollo scaffolding the URI variable should be correct, if you are creating the file for yourself you'll need to make sure that matches your GraphQL back-end.
7. In addition to the new file mentioned above you'll also need to modify your app.config.ts, to look something like this (as noted previously if you use the Apollo scaffolding this should be done for you);
	```typescript
	import { ApplicationConfig } from '@angular/core';
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
	```
8. Run the new npm command inserted in step 3 with the command `npm run generate`, you should see your code file generated
9. As at time of writing this document (with graphql-codegen/cli version 5 and graphql-codegen/typescript 4.0.1) there is a bug with the generated code that leads to a build error, to resolve this;
    1. Open the generated file
    2. For each of the GraphQL queries you feed to the generator there will be a export class statement in the generated file, they will look something like this;
        ```typescript
        @Injectable({
        	providedIn: 'root'
        })
        export class GetOffsetPaginatedMoviesGQL extends Apollo.Query<GetOffsetPaginatedMoviesQuery, GetOffsetPaginatedMoviesQueryVariables> {
        	document = GetOffsetPaginatedMoviesDocument;
        
        	constructor(apollo: Apollo.Apollo) {
        		super(apollo);
        	}
        }
        ```
    3. The issue is with  the `document =  someQueryDocument` line - you need to add "override" to fix the build error, so using the above example it would look like the following after a fix;
		```typescript
		@Injectable({
			providedIn: 'root'
		})
		export class GetOffsetPaginatedMoviesGQL extends Apollo.Query<GetOffsetPaginatedMoviesQuery, GetOffsetPaginatedMoviesQueryVariables> {
			override document = GetOffsetPaginatedMoviesDocument;

			constructor(apollo: Apollo.Apollo) {
				super(apollo);
			}
		}
		```
10. You should now be able to make use of the helper classes to run queries, here is an example of loading a movie by id;
```typescript
import { GetMovieByIdGQL } from "../../../generated/graphql"; // import the get movies helper object generated by graphql-code-gen

@Injectable({ providedIn: 'root' })
export class MovieService implements OnInit {

	public constructor(private moviesByIdClient: GetMovieByIdGQL) { // inject the helper
	}

	ngOnInit(): void {
	}

	public getMovieById(movieId: string): Observable<ApolloQueryResult<any>> {
		return this.moviesByIdClient.fetch({ id: movieId }); // call fetch passing in the movie id parameter
	}
}
``` 