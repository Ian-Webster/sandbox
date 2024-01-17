# DataAccess.Sample.Ux - Apollo
## Introduction
The purpose of this project is to test interactions with the [DataAccess.Sample.Web](https://github.com/Ian-Webster/sandbox/tree/main/nuget-samples/DataAccess.Sample/DataAccess.Sample.Web) project from a front-end perspective.

This version of the project uses the [Apollo-Angular](https://the-guild.dev/graphql/apollo-angular) library.

A basic [Angular Material](https://material.angular.io/components/categories) UX is in place for this project, a simple menu can be found at the top to navigate to the various components testing different aspects of the example GraphQL implementation.

## Basic setup

1. The Angular Apollo team provide an angular schematic to streamline set up;
    ```
    ng add apollo-angular
    ```
2. You'll be asked about certain configuration settings / options you want to apply for URL for Graphql API use `https://localhost:7128/graphql/`
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
## Paging
This example project has demonstrations of [cursor based](https://chillicream.com/docs/hotchocolate/v13/fetching-data/pagination) and [offset based paging](https://chillicream.com/docs/hotchocolate/v13/fetching-data/pagination/#offset-pagination).
### Cursor paging
Cursor paging uses a string cursor to identify each record in a set, you can then use these to either request records before or after any point in the set, consider the following example;
```json
{
  "data": {
    "paginatedMovies": {
      "totalCount": 50,
      "pageInfo": {
        "startCursor": "MA==",
        "endCursor": "Mg=="
      },
      "edges": [
        {
          "node": {
            "movieId": "3acb0b46-e464-4ae5-b006-513e65436d49",
            "name": "Allied"
          },
          "cursor": "MA=="
        },
        {
          "node": {
            "movieId": "e4c97b67-6168-4fe9-84f0-f8cb7b11d674",
            "name": "American Pie"
          },
          "cursor": "MQ=="
        },
        {
          "node": {
            "movieId": "24d818ef-48ea-4a8e-ac52-75de7dae3529",
            "name": "American Pie 2"
          },
          "cursor": "Mg=="
        }
      ]
    }
  }
}
```
* The first movie in the list is "Allied" with a cursor of "MA==", if we request movies before that cursor we get the previous page
* The last movie in the list is "American Pie 2" with a cursor of "Mg==", if we request movies after that cursor we get the next page

Cursor pagination is the recommend method by the team behind the HotChocolate GraphQL library however it does present some complications when implementing backwards paging, consider the following code which is taken from the movies-paginated.component component;

```typescript
export class MoviesPaginatedComponent implements OnInit {
	public movies: any;
	public loading: boolean = true;
	public pageSize: number = 5;
	public pageSizes: Array<number> = [5, 10, 25];

	private pages: Array<CursorPage> = new Array<CursorPage>();
	private pageIndex: number = 0;

	constructor(private movieService: MovieService) {

	}

	ngOnInit(): void {
		this.pages = new Array<CursorPage>();
		this.pageIndex = 0;
		this.movieService.getCursorPaginatedMovies(this.pageSize).subscribe(({ data, loading }) => {
			if (!loading) {
				this.movies = data.paginatedMovies;
				this.pages.push({ index: 0, cursor: undefined }); // first page has no cursor so push undefined
				this.loading = false;
			}
		});
	}

	public onPageSizeChange($event: MatSelectChange) {
		this.pageSize = $event.value;
		this.ngOnInit();
	}

	public getNextPage() {
		if (!this.movies || !this.movies.pageInfo || !this.movies.pageInfo.hasNextPage) { return; }
		this.loading = true;
		const currentNextCursor = this.movies.pageInfo.endCursor;
		this.movieService.getCursorPaginatedMovies(this.pageSize, this.movies.pageInfo.endCursor).subscribe(({ data, loading }) => {
			if (!loading) {
				this.movies = data.paginatedMovies;
				this.pageIndex++;
				this.pages.push({ index: this.pageIndex, cursor: currentNextCursor });
				this.loading = false;
			}
		});
	}

	public getPreviousPage() {
		if (!this.movies && !this.movies.pageInfo.hasPreviousPage) { return; }
		this.loading = true;
		this.pageIndex--;
		this.movieService.getCursorPaginatedMovies(this.pageSize, this.pages.find(p => p.index === this.pageIndex)?.cursor).subscribe(({ data, loading }) => {
			if (!loading) {
				this.movies = data.paginatedMovies;
				this.loading = false;
			}
		});
	}

}

```
Paging forward is simple the `movies` object has as `endCursor` property on it, we simply have to request data after that cursor, however if we want to go backwards we run into problems from the second page onwards, consider the following data;
```json
[
	{
	  "node": {
		"name": "Allied"
	  },
	  "cursor": "MA=="
	},
	{
	  "node": {
		"name": "American Pie"
	  },
	  "cursor": "MQ=="
	},
	{
	  "node": {
		"name": "American Pie 2"
	  },
	  "cursor": "Mg=="
	},
	{
	  "node": {
		"name": "Avatar 1"
	  },
	  "cursor": "Mw=="
	},
	{
	  "node": {
		"name": "Avatar 2: Avatar harder"
	  },
	  "cursor": "NA=="
	},
	{
	  "node": {
		"name": "Avatar 3: A good day to Avatar"
	  },
	  "cursor": "NQ=="
	},
	{
	  "node": {
		"name": "Avatar 4: Avatar forever"
	  },
	  "cursor": "Ng=="
	},
	{
	  "node": {
		"name": "Avengers 1"
	  },
	  "cursor": "Nw=="
	},
	{
	  "node": {
		"name": "Avengers 2"
	  },
	  "cursor": "OA=="
	}
]
```
imagine we are paging 3 movies at a time and we are on the final page (staring with "Avatar 4: Avatar forever"), we want to return to the previous 3 movies (starting with "Avatar 1")
* if we use "before:Ng==" we will actually get the first three movies (starting with "Allied"), this is because "before:y" combined with "first:x" will find the **first** x records **before** cursor y
* what we actually want to do is fetch the first the records after:"Mw=="
* unfortunately the data we get back from GraphQL only gives us the first and last cursors for the block of records we retrieved we cannot make use of those to page backwards
* what we end up having to do is store the "first" cursor for each page, that way for any given page index we can figure out what the "first" cursor was and use that in conjunction with a  "after:x" command

### Offset paging
While the HotChocolate team do not recommend offset paging it does making wiring up backwards paging much easier, consider the following example taken from the movies-offset-paginated.component;

```typescript
export class MoviesOffsetPaginatedComponent implements OnInit {
	public movies: any;
	public loading: boolean = true;
	public pageSize: number = 5;
	public pageSizes: Array<number> = [5, 10, 25];
	public numberOfRecords: number = 0;
	public pageIndex: number = 0;
	
	constructor(private movieService: MovieService) {

	}

	public ngOnInit(): void {
		this.getMovies(this.pageSize, 0);
	}

	public onPage($event: PageEvent) {
		if (this.movies) {
			this.pageSize = $event.pageSize;
			this.getMovies(this.pageSize, $event.pageIndex * this.pageSize);
		}
	}

	private getMovies(pageSize: number, offset: number): void {
		this.loading = true;
		this.movieService.getOffsetPaginatedMovies(pageSize, offset).subscribe(({ data, loading }) => {
			if (!loading) {
				this.movies = data.offsetPaginatedMovies;
				this.numberOfRecords = this.movies.totalCount;
				this.pageIndex = Math.floor(offset / pageSize);  // recalculate pageIndex
				this.loading = false;
			}
		});
	}
}

```
You can see there is substantially less code required, we don't need to track the details of previous pages we just need to keep track of the page index.