import { HttpClient } from "@angular/common/http";
import { Injectable, OnInit } from "@angular/core";
import { ApolloQueryResult } from "@apollo/client/core";
import { Apollo, gql } from "apollo-angular";
import { BehaviorSubject, Observable, lastValueFrom, map } from "rxjs";
import { SortEnumType } from "../../domain/enums/sort.enum";
import { parse } from "graphql";
import { GetMovieByIdGQL, GetOffsetPaginatedMoviesGQL, GetPaginatedMoviesAfterGQL, GetPaginatedMoviesGQL } from "../../../generated/graphql";

@Injectable({ providedIn: 'root' })
export class MovieService implements OnInit {

	public movies: BehaviorSubject<any> = new BehaviorSubject<any>(null);

	public constructor(
		private httpClient: HttpClient, 
		private apollo: Apollo, 
		private moviesByIdClient: GetMovieByIdGQL,
		private moviesPaginatedByOffsetClient: GetOffsetPaginatedMoviesGQL,
		private moviesPaginatedByCursorClient: GetPaginatedMoviesGQL,
		private moviesPaginatedByCursorWithAfterClient: GetPaginatedMoviesAfterGQL
		) {
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

	public getMovieById(movieId: string): Observable<ApolloQueryResult<any>> {
		return this.moviesByIdClient.fetch({ id: movieId });
	}

	/**
	 * Get movies paginated using cursor pagination
	 * @param first 
	 * @param after 
	 * @returns 
	 */
	public getCursorPaginatedMovies(take: number, after?: string | undefined): Observable<ApolloQueryResult<any>> {
		if (after) {
			return this.moviesPaginatedByCursorWithAfterClient.fetch({ first: take, after: after });
		}
		return this.moviesPaginatedByCursorClient.fetch({ first: take});
	}

	/**
	 * Get movies paginated using offset pagination
	 * @param take 
	 * @param skip 
	 * @returns 
	 */
	public getOffsetPaginatedMovies(take: number, skip: number): Observable<ApolloQueryResult<any>> {
		return this.moviesPaginatedByOffsetClient.fetch({ take: take, skip: skip });
	}
}