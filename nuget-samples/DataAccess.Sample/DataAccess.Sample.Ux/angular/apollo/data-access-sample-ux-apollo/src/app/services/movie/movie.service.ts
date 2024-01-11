import { HttpClient } from "@angular/common/http";
import { Injectable, OnInit } from "@angular/core";
import { ApolloQueryResult } from "@apollo/client/core";
import { Apollo, gql } from "apollo-angular";
import { BehaviorSubject, Observable, lastValueFrom, map } from "rxjs";
import { SortEnumType } from "../../domain/enums/sort.enum";
import { parse } from "graphql";

@Injectable({providedIn: 'root'})
export class MovieService implements OnInit {

	public movies: BehaviorSubject<any> = new BehaviorSubject<any>(null);

	public constructor(private httpClient: HttpClient, private apollo: Apollo) {	
	}

	ngOnInit(): void {
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

	public getAllMovies(): Observable<ApolloQueryResult<any>> {
		return this.apollo.watchQuery<any>({
			query: gql`
				query getAllMovies {
					movies (order: [{name:ASC}]) {
						movieId
						name
					}
				}
			`,
		}).valueChanges;
	}

	public getMovieById(movieId: string): Observable<ApolloQueryResult<any>> {
		return this.apollo.watchQuery<any>({
			query: gql`
				query getMovieById($id: UUID!) {
					movie(where: {movieId: {eq: $id}}) {
						movieId
						name,
						movieGenres { 
							genre 
						}
					}
				}
			`,
			variables: {
				id: movieId,
			},
		}).valueChanges;
	}

	public getPaginatedMovies(first: number, after: string): Observable<ApolloQueryResult<any>> {
		return this.apollo.watchQuery<any>({
			query: gql`
				query getPaginatedMovies($first: Int!, $after: String!) {
					paginatedMovies(first: $first, order: [{name: DESC}], after: $after) {
						totalCount
						pageInfo {
							startCursor
							endCursor
							hasNextPage
							hasPreviousPage
						}
						edges {
							node {
								movieId
								name
							}
						}
					}
				}
			`,
			variables: {
				first: first,
				after: after,
			},
		}).valueChanges;
	}

}