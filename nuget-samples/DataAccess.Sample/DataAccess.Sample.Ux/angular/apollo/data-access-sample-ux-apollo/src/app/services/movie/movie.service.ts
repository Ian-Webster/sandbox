import { HttpClient } from "@angular/common/http";
import { Injectable, OnInit } from "@angular/core";
import { ApolloQueryResult } from "@apollo/client/core";
import { Apollo, gql } from "apollo-angular";
import { BehaviorSubject, Observable, lastValueFrom, map } from "rxjs";
import { SortEnumType } from "../../domain/enums/sort.enum";
import { parse } from "graphql";

@Injectable({ providedIn: 'root' })
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

	/**
	 * Get movies paginated using cursor pagination
	 * @param first 
	 * @param after 
	 * @returns 
	 */
	public getCursorPaginatedMovies(take: number, after?: string | undefined): Observable<ApolloQueryResult<any>> {
		let queryString = `
		  query getPaginatedMovies($first: Int!`;

		if (after) {
			queryString += `, $after: String) {
				paginatedMovies(first: $first, order: [{name: ASC}], after: $after`;
		} else {
			queryString += `) {
				paginatedMovies(first: $first, order: [{name: ASC}]`;
		}

		queryString += `) {
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
		}`;

		if (after) {
			return this.apollo.watchQuery<any>({
				query: gql`${queryString}`,
				variables: {
					first: take,
					after: after
				}
			}).valueChanges;
		}

		return this.apollo.watchQuery<any>({
			query: gql`${queryString}`,
			variables: {
				first: take
			}
		}).valueChanges;
	}

	/**
	 * Get movies paginated using offset pagination
	 * @param take 
	 * @param skip 
	 * @returns 
	 */
	public getOffsetPaginatedMovies(take: number, skip: number): Observable<ApolloQueryResult<any>> {
		return this.apollo.watchQuery<any>({
			query: gql`
				query getOffsetPaginatedMovies($take:Int, $skip:Int) {
					offsetPaginatedMovies(take: $take, skip: $skip, order: [{name:ASC}])	{
						totalCount,
						pageInfo {
							hasNextPage,
							hasPreviousPage
						}
						items {
							movieId,
							name
						}
					}
				}
			`,
			variables: {
				take: take,
				skip: skip
			},
		}).valueChanges;
	}
}