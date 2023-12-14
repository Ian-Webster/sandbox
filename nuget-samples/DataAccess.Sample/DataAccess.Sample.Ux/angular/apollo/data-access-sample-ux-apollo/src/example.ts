import { gql } from 'apollo-angular';

const GET_POSTS = gql`query getAllMovies{
	movies (order: [{name:DESC}]),
	{
	  movieId,
	  name
	}
}`