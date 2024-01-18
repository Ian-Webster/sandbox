import { Routes } from '@angular/router';
import { MovieComponent } from './routes/movie/movie.component';
import { MoviesComponent } from './routes/movies/movies.component';
import { MoviesPaginatedComponent } from './routes/movies-paginated/movies-paginated.component';
import { MoviesOffsetPaginatedComponent } from './routes/movies-offset-paginated/movies-offset-paginated.component';

export const routes: Routes = [
	{
		path: 'movies', component: MoviesComponent
	},
	{
		path: 'movies-paged', component: MoviesPaginatedComponent
	},
	{
		path: 'movies-offset-paged', component: MoviesOffsetPaginatedComponent
	},
	{
		path: 'movie/:id', component: MovieComponent
	}
];
