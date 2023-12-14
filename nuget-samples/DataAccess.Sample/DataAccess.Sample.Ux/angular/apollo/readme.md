# DataAccess.Sample.Ux - Apollo
## Introduction
This is an Angular project and it's purpose is to test interactions with the [DataAccess.Sample.Web](https://github.com/Ian-Webster/sandbox/tree/main/nuget-samples/DataAccess.Sample/DataAccess.Sample.Web) project from a frontend perspective.

This version of the project uses the [Apollo-Angular](https://the-guild.dev/graphql/apollo-angular) library.

## Routes
* /movie loads movies via GraphQL

## Basic setup

1. The Angular Apollo team provide an angular schematic to streamline set up;
    ```
    ng add apollo-angular
    ```
2. You'll be asked about certain configuration settings / options you want to apply for URL for graphql API use `https://localhost:7128/graphql/`
3. Set up a query, as an example here I'm loading movies and putting them into a behaviour subject that other code can subscribe to;
```TypeScript
import { HttpClient } from "@angular/common/http";
import { Injectable, OnInit } from "@angular/core";
import { Apollo, gql } from "apollo-angular";
import { BehaviorSubject, Observable, lastValueFrom, map } from "rxjs";

@Injectable({providedIn: 'root'})
export class MovieService implements OnInit {

	public movies: BehaviorSubject<any> = new BehaviorSubject<any>(null);

	public constructor(private httpClient: HttpClient, private apollo: Apollo) {	
	}

	ngOnInit(): void {
		this.apollo
		.watchQuery({
			query: gql`
			query getAllMovies{
				movies (order: [{name:ASC}]),
				{
				  movieId,
				  name
				}
			  }
			`,
		})
		.valueChanges.subscribe((result: any) => {
			this.movies.next(result);
		});
	}
}
```
4. You'll need to make the web API project hosting the GraphQL server has CORS enabled and allows any header;
```csharp
app.MapGraphQL();
// note that we setup CORS after mapping GraphQL
app.UseCors(c =>
{
    c.WithOrigins("http://localhost:4200"); // angular project URL
    c.AllowAnyHeader();
});
```

## Named clients
If you want to have multiple endpoints you can make sure of [named clients](https://the-guild.dev/graphql/apollo-angular/docs/get-started#named-clients), to do this you need to modify the app.config.ts to include some non-standard configuration;
```typescript
import { APOLLO_NAMED_OPTIONS, ApolloModule, NamedOptions } from 'apollo-angular';
import { HttpLink } from 'apollo-angular/http';
import { HttpClientModule } from '@angular/common/http';
import { InMemoryCache } from '@apollo/client/core';
 
@NgModule({
  imports: [BrowserModule, ApolloModule, HttpClientModule],
  providers: [
    {
      provide: APOLLO_NAMED_OPTIONS, // <-- Different from standard initialization
      useFactory(httpLink: HttpLink): NamedOptions {
        return {
          newClientName: {
            // <-- This settings will be saved by name: newClientName
            cache: new InMemoryCache(),
            link: httpLink.create({
              uri: 'https://o5x5jzoo7z.sse.codesandbox.io/graphql',
            }),
          },
        };
      },
      deps: [HttpLink],
    },
  ],
})
export class AppModule {}
```
Then you'll set the client name for Apollo in the constructor of your service before using Apollo;
```typescript
import { Apollo, ApolloBase, gql } from 'apollo-angular';
import { Injectable } from '@angular/core';
 
@Injectable()
class MovieService {
  private apollo: ApolloBase;
 
  constructor(private apolloProvider: Apollo) {
    this.apollo = this.apolloProvider.use('newClientName');
  }
 
  getData(): Observable<ApolloQueryResult> {
    return this.apollo.watchQuery({
      query: gql`
            query getAllMovies{
                movies (order: [{name:ASC}]),
                {
                  movieId,
                  name
                }
              }
      `,
    });
  }
}
```

https://the-guild.dev/graphql/apollo-angular/docs/development-and-testing/using-typescript