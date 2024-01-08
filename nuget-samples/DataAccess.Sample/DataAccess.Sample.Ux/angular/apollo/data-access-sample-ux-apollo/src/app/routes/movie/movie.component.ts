import { Component, OnInit } from '@angular/core';
import { MovieService } from '../../services/movie/movie.service';
import { SharedModule } from '../../shared/shared.module';

@Component({
	selector: 'app-movie',
	standalone: true,
	imports: [],
	templateUrl: './movie.component.html',
	styleUrl: './movie.component.scss'
})
export class MovieComponent implements OnInit {

	public message: string = "";

	public constructor(private movieService: MovieService) {
	}

	public ngOnInit(): void {

		//this.movieService.test();

		this.movieService.movies.subscribe(movie => {
			console.log(movie);
		});

		this.movieService.GetMovies().then(m => {
			console.log(m);
		});

		this.movieService.getAllMovies().subscribe(({ data, loading }) =>{
			if (!loading) {
				console.log(data.movies);
			}
		});

		// this.movieService.getAllMovies().subscribe(s => {
		// 	debugger;
		// 	console.log(s);
		// });
	}

	
}
