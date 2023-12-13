import { Component, OnInit } from '@angular/core';
import { MovieService } from '../../services/movie/movie.service';

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
		debugger;
		this.movieService.getAllMovies().subscribe(s => {
			debugger;
			console.log(s);
		});
	}

	
}
