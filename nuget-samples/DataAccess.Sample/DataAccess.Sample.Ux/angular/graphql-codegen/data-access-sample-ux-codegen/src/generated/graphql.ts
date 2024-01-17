import { gql } from 'apollo-angular';
import { Injectable } from '@angular/core';
import * as Apollo from 'apollo-angular';
export type Maybe<T> = T | null;
export type InputMaybe<T> = Maybe<T>;
export type Exact<T extends { [key: string]: unknown }> = { [K in keyof T]: T[K] };
export type MakeOptional<T, K extends keyof T> = Omit<T, K> & { [SubKey in K]?: Maybe<T[SubKey]> };
export type MakeMaybe<T, K extends keyof T> = Omit<T, K> & { [SubKey in K]: Maybe<T[SubKey]> };
export type MakeEmpty<T extends { [key: string]: unknown }, K extends keyof T> = { [_ in K]?: never };
export type Incremental<T> = T | { [P in keyof T]?: P extends ' $fragmentName' | '__typename' ? T[P] : never };
/** All built-in and custom scalars, mapped to their actual values */
export type Scalars = {
  ID: { input: string; output: string; }
  String: { input: string; output: string; }
  Boolean: { input: boolean; output: boolean; }
  Int: { input: number; output: number; }
  Float: { input: number; output: number; }
  UUID: { input: any; output: any; }
};

/** Information about the offset pagination. */
export type CollectionSegmentInfo = {
  __typename?: 'CollectionSegmentInfo';
  /** Indicates whether more items exist following the set defined by the clients arguments. */
  hasNextPage: Scalars['Boolean']['output'];
  /** Indicates whether more items exist prior the set defined by the clients arguments. */
  hasPreviousPage: Scalars['Boolean']['output'];
};

export enum Genres {
  Action = 'ACTION',
  Adventure = 'ADVENTURE',
  Animation = 'ANIMATION',
  Biography = 'BIOGRAPHY',
  Comedy = 'COMEDY',
  Crime = 'CRIME',
  Drama = 'DRAMA',
  Family = 'FAMILY',
  Fantasy = 'FANTASY',
  History = 'HISTORY',
  Horror = 'HORROR',
  Musical = 'MUSICAL',
  Romance = 'ROMANCE',
  SciFi = 'SCI_FI',
  Thriller = 'THRILLER'
}

export type GenresOperationFilterInput = {
  eq?: InputMaybe<Genres>;
  in?: InputMaybe<Array<Genres>>;
  neq?: InputMaybe<Genres>;
  nin?: InputMaybe<Array<Genres>>;
};

export type ListFilterInputTypeOfMovieGenreFilterInput = {
  all?: InputMaybe<MovieGenreFilterInput>;
  any?: InputMaybe<Scalars['Boolean']['input']>;
  none?: InputMaybe<MovieGenreFilterInput>;
  some?: InputMaybe<MovieGenreFilterInput>;
};

export type Movie = {
  __typename?: 'Movie';
  movieGenres?: Maybe<Array<MovieGenre>>;
  movieId: Scalars['UUID']['output'];
  name: Scalars['String']['output'];
};

export type MovieFilterInput = {
  and?: InputMaybe<Array<MovieFilterInput>>;
  movieGenres?: InputMaybe<ListFilterInputTypeOfMovieGenreFilterInput>;
  movieId?: InputMaybe<UuidOperationFilterInput>;
  name?: InputMaybe<StringOperationFilterInput>;
  or?: InputMaybe<Array<MovieFilterInput>>;
};

export type MovieGenre = {
  __typename?: 'MovieGenre';
  genre: Genres;
  movie: Movie;
  movieId: Scalars['UUID']['output'];
};

export type MovieGenreFilterInput = {
  and?: InputMaybe<Array<MovieGenreFilterInput>>;
  genre?: InputMaybe<GenresOperationFilterInput>;
  movie?: InputMaybe<MovieFilterInput>;
  movieId?: InputMaybe<UuidOperationFilterInput>;
  or?: InputMaybe<Array<MovieGenreFilterInput>>;
};

export type MovieSortInput = {
  movieId?: InputMaybe<SortEnumType>;
  name?: InputMaybe<SortEnumType>;
};

/** A segment of a collection. */
export type OffsetPaginatedMoviesCollectionSegment = {
  __typename?: 'OffsetPaginatedMoviesCollectionSegment';
  /** A flattened list of the items. */
  items?: Maybe<Array<Movie>>;
  /** Information to aid in pagination. */
  pageInfo: CollectionSegmentInfo;
  totalCount: Scalars['Int']['output'];
};

/** Information about pagination in a connection. */
export type PageInfo = {
  __typename?: 'PageInfo';
  /** When paginating forwards, the cursor to continue. */
  endCursor?: Maybe<Scalars['String']['output']>;
  /** Indicates whether more edges exist following the set defined by the clients arguments. */
  hasNextPage: Scalars['Boolean']['output'];
  /** Indicates whether more edges exist prior the set defined by the clients arguments. */
  hasPreviousPage: Scalars['Boolean']['output'];
  /** When paginating backwards, the cursor to continue. */
  startCursor?: Maybe<Scalars['String']['output']>;
};

/** A connection to a list of items. */
export type PaginatedMoviesConnection = {
  __typename?: 'PaginatedMoviesConnection';
  /** A list of edges. */
  edges?: Maybe<Array<PaginatedMoviesEdge>>;
  /** A flattened list of the nodes. */
  nodes?: Maybe<Array<Movie>>;
  /** Information to aid in pagination. */
  pageInfo: PageInfo;
  /** Identifies the total count of items in the connection. */
  totalCount: Scalars['Int']['output'];
};

/** An edge in a connection. */
export type PaginatedMoviesEdge = {
  __typename?: 'PaginatedMoviesEdge';
  /** A cursor for use in pagination. */
  cursor: Scalars['String']['output'];
  /** The item at the end of the edge. */
  node: Movie;
};

export type Query = {
  __typename?: 'Query';
  movie?: Maybe<Movie>;
  movies?: Maybe<Array<Movie>>;
  offsetPaginatedMovies?: Maybe<OffsetPaginatedMoviesCollectionSegment>;
  paginatedMovies?: Maybe<PaginatedMoviesConnection>;
};


export type QueryMovieArgs = {
  where?: InputMaybe<MovieFilterInput>;
};


export type QueryMoviesArgs = {
  order?: InputMaybe<Array<MovieSortInput>>;
  where?: InputMaybe<MovieFilterInput>;
};


export type QueryOffsetPaginatedMoviesArgs = {
  order?: InputMaybe<Array<MovieSortInput>>;
  skip?: InputMaybe<Scalars['Int']['input']>;
  take?: InputMaybe<Scalars['Int']['input']>;
  where?: InputMaybe<MovieFilterInput>;
};


export type QueryPaginatedMoviesArgs = {
  after?: InputMaybe<Scalars['String']['input']>;
  before?: InputMaybe<Scalars['String']['input']>;
  first?: InputMaybe<Scalars['Int']['input']>;
  last?: InputMaybe<Scalars['Int']['input']>;
  order?: InputMaybe<Array<MovieSortInput>>;
  where?: InputMaybe<MovieFilterInput>;
};

export enum SortEnumType {
  Asc = 'ASC',
  Desc = 'DESC'
}

export type StringOperationFilterInput = {
  and?: InputMaybe<Array<StringOperationFilterInput>>;
  contains?: InputMaybe<Scalars['String']['input']>;
  endsWith?: InputMaybe<Scalars['String']['input']>;
  eq?: InputMaybe<Scalars['String']['input']>;
  in?: InputMaybe<Array<InputMaybe<Scalars['String']['input']>>>;
  ncontains?: InputMaybe<Scalars['String']['input']>;
  nendsWith?: InputMaybe<Scalars['String']['input']>;
  neq?: InputMaybe<Scalars['String']['input']>;
  nin?: InputMaybe<Array<InputMaybe<Scalars['String']['input']>>>;
  nstartsWith?: InputMaybe<Scalars['String']['input']>;
  or?: InputMaybe<Array<StringOperationFilterInput>>;
  startsWith?: InputMaybe<Scalars['String']['input']>;
};

export type UuidOperationFilterInput = {
  eq?: InputMaybe<Scalars['UUID']['input']>;
  gt?: InputMaybe<Scalars['UUID']['input']>;
  gte?: InputMaybe<Scalars['UUID']['input']>;
  in?: InputMaybe<Array<InputMaybe<Scalars['UUID']['input']>>>;
  lt?: InputMaybe<Scalars['UUID']['input']>;
  lte?: InputMaybe<Scalars['UUID']['input']>;
  neq?: InputMaybe<Scalars['UUID']['input']>;
  ngt?: InputMaybe<Scalars['UUID']['input']>;
  ngte?: InputMaybe<Scalars['UUID']['input']>;
  nin?: InputMaybe<Array<InputMaybe<Scalars['UUID']['input']>>>;
  nlt?: InputMaybe<Scalars['UUID']['input']>;
  nlte?: InputMaybe<Scalars['UUID']['input']>;
};

export const GetPaginatedMoviesDocument = gql`
    query getPaginatedMovies($first: Int!, $after: String) {
  paginatedMovies(first: $first, order: [{name: ASC}], after: $after) {
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
    `;

  @Injectable({
    providedIn: 'root'
  })
  export class GetPaginatedMoviesGQL extends Apollo.Query<GetPaginatedMoviesQuery, GetPaginatedMoviesQueryVariables> {
    override document = GetPaginatedMoviesDocument;
    
    constructor(apollo: Apollo.Apollo) {
      super(apollo);
    }
  }
export const GetMovieByIdDocument = gql`
    query getMovieById($id: UUID!) {
  movie(where: {movieId: {eq: $id}}) {
    movieId
    name
    movieGenres {
      genre
    }
  }
}
    `;

  @Injectable({
    providedIn: 'root'
  })
  export class GetMovieByIdGQL extends Apollo.Query<GetMovieByIdQuery, GetMovieByIdQueryVariables> {
    override document = GetMovieByIdDocument;
    
    constructor(apollo: Apollo.Apollo) {
      super(apollo);
    }
  }
export const GetOffsetPaginatedMoviesDocument = gql`
    query getOffsetPaginatedMovies($take: Int, $skip: Int) {
  offsetPaginatedMovies(take: $take, skip: $skip, order: [{name: ASC}]) {
    totalCount
    pageInfo {
      hasNextPage
      hasPreviousPage
    }
    items {
      movieId
      name
    }
  }
}
    `;

  @Injectable({
    providedIn: 'root'
  })
  export class GetOffsetPaginatedMoviesGQL extends Apollo.Query<GetOffsetPaginatedMoviesQuery, GetOffsetPaginatedMoviesQueryVariables> {
    override document = GetOffsetPaginatedMoviesDocument;
    
    constructor(apollo: Apollo.Apollo) {
      super(apollo);
    }
  }
export type GetPaginatedMoviesQueryVariables = Exact<{
  first: Scalars['Int']['input'];
  after?: InputMaybe<Scalars['String']['input']>;
}>;


export type GetPaginatedMoviesQuery = { __typename?: 'Query', paginatedMovies?: { __typename?: 'PaginatedMoviesConnection', totalCount: number, pageInfo: { __typename?: 'PageInfo', startCursor?: string | null, endCursor?: string | null, hasNextPage: boolean, hasPreviousPage: boolean }, edges?: Array<{ __typename?: 'PaginatedMoviesEdge', node: { __typename?: 'Movie', movieId: any, name: string } }> | null } | null };

export type GetMovieByIdQueryVariables = Exact<{
  id: Scalars['UUID']['input'];
}>;


export type GetMovieByIdQuery = { __typename?: 'Query', movie?: { __typename?: 'Movie', movieId: any, name: string, movieGenres?: Array<{ __typename?: 'MovieGenre', genre: Genres }> | null } | null };

export type GetOffsetPaginatedMoviesQueryVariables = Exact<{
  take?: InputMaybe<Scalars['Int']['input']>;
  skip?: InputMaybe<Scalars['Int']['input']>;
}>;


export type GetOffsetPaginatedMoviesQuery = { __typename?: 'Query', offsetPaginatedMovies?: { __typename?: 'OffsetPaginatedMoviesCollectionSegment', totalCount: number, pageInfo: { __typename?: 'CollectionSegmentInfo', hasNextPage: boolean, hasPreviousPage: boolean }, items?: Array<{ __typename?: 'Movie', movieId: any, name: string }> | null } | null };
