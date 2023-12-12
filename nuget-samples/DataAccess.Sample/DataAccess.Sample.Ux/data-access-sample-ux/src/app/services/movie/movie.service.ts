import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Apollo } from "apollo-angular";
import { Observable, lastValueFrom, map } from "rxjs";
import { Movie, Query } from "../../../../graphql/generated";

@Injectable({providedIn: 'root'})
export class MovieService {

	public constructor(private httpClient: HttpClient, private apollo: Apollo) {	
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

	public getAllMovies(): Observable<Movie> {
		return this.apollo
		.watchQuery<Movies>(QueryMoviesArgs)
		.valueChanges.pipe(
		  map((result) => result.data)
		);
	}


}