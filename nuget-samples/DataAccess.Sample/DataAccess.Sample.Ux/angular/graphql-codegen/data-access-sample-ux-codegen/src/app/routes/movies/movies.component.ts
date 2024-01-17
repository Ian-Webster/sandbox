import { Component, OnInit } from '@angular/core';
import { MovieService } from '../../services/movie/movie.service';
import { SharedModule } from '../../shared/shared.module';
import { RouterLink } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faEye } from '@fortawesome/free-solid-svg-icons';

@Component({
	selector: 'app-movies',
	standalone: true,
	imports: [SharedModule, RouterLink, MatTableModule, FontAwesomeModule],
	templateUrl: './movies.component.html',
	styleUrl: './movies.component.scss'
})
export class MoviesComponent implements OnInit {

	public movies: any = [];
	public loading: boolean = true;
	public displayedColumns: string[] = ['movieId', 'name', 'view'];

	faEye = faEye;

	//<i class="fa-solid fa-eye"></i>

	constructor(private movieService: MovieService) {

	}

	ngOnInit(): void {
		this.movieService.getAllMovies().subscribe(({ data, loading }) => {
			this.loading = loading;
			if (!loading) {
				this.movies = data.movies;
			}
		});
	}

}
