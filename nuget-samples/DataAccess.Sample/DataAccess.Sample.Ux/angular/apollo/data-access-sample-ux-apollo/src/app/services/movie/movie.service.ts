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
		debugger;
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
			debugger;
			this.movies.next(result);
		});
	}

    public async GetMovies(): Promise<any> {
		try { 
			return await lastValueFrom(this.httpClient.get("https://localhost:7128/api/Movie"));
		} 
		catch (ex) {
			console.log(ex);
			return "error";
		}
	}

	// TODO - try this https://www.apollographql.com/docs/apollo-server/workflow/generate-types/ tomorrow if no anwsers on your question here https://github.com/dotansimha/graphql-code-generator/discussions/9785

	public getAllMovies(): Observable<any> | undefined {
		return undefined;
	}

	public test(): void {
		debugger;
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
			debugger;
			this.movies.next(result);
		});
	}

}