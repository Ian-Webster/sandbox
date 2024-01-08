import { Routes } from '@angular/router';
import { MovieComponent } from './routes/movie/movie.component';
import { MoviesComponent } from './routes/movies/movies/movies.component';

export const routes: Routes = [
	{
		path: 'movies', component: MoviesComponent
	},
	{
		path: 'movie', component: MovieComponent
	}
];
