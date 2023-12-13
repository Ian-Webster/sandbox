import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, lastValueFrom, map } from "rxjs";
import { GetAllMoviesGQL } from "../../../../graphql/generated";

@Injectable({providedIn: 'root'})
export class MovieService {

	public constructor(private httpClient: HttpClient, private moviesGQL: GetAllMoviesGQL) {	
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

	public getAllMovies(): Observable<any> {
		return this.moviesGQL.watch().valueChanges.pipe(map(result => result.data.movies))
	}

	// https://angular.schule/blog/2018-06-apollo-graphql-code-generator
	// public getAllMovies(): Observable<Movie[] | undefined> {
	// 	return this.apollo.query<GetAllMoviesQuery>({
	// 		query: GetAllMoviesDocument
	// 	})
	// 	.pipe(
	// 		map(({ data }) => data.movies)
	// 	);
	// }


}