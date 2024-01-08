import { Component, OnInit } from '@angular/core';
import { MovieService } from '../../../services/movie/movie.service';
import { SharedModule } from '../../../shared/shared.module';

@Component({
  selector: 'app-movies',
  standalone: true,
  imports: [SharedModule],
  templateUrl: './movies.component.html',
  styleUrl: './movies.component.scss'
})
export class MoviesComponent implements OnInit {

	public movies: any = [];
	public loading: boolean = true;

	constructor(private movieService: MovieService) {
	
	}

	ngOnInit(): void {
		this.movieService.getAllMovies().subscribe(({ data, loading }) =>{
			this.loading = loading;
			if (!loading) {
				this.movies = data.movies;
			}
		});
	}

}
