import { Component, OnInit } from '@angular/core';
import { SharedModule } from '../../shared/shared.module';
import { MovieService } from '../../services/movie/movie.service';
import { SortEnumType } from '../../domain/enums/sort.enum';

@Component({
  selector: 'app-movies-paginated',
  standalone: true,
  imports: [SharedModule],
  templateUrl: './movies-paginated.component.html',
  styleUrl: './movies-paginated.component.scss'
})
export class MoviesPaginatedComponent implements OnInit {

	public movies: any;
	public loading: boolean = true;

	constructor(private movieService: MovieService) {
		
	}

	ngOnInit(): void {
		this.movieService.getPaginatedMovies(10, "MQ==").subscribe(({ data, loading }) =>{
			if (!loading) {
				this.movies = data.paginatedMovies;
				this.loading = false;
			}
		});
	}

	getNextPage() {
		if (!this.movies || !this.movies.pageInfo || !this.movies.pageInfo.hasNextPage) {return;}
		this.loading = true;
		this.movieService.getPaginatedMovies(10, this.movies.pageInfo.endCursor).subscribe(({ data, loading }) =>{
			if (!loading) {
				this.movies = data.paginatedMovies;
				this.loading = false;
			}
		});
	}

	getPreviousPage() {
		if (!this.movies && !this.movies.pageInfo.hasPreviousPage) {return;}
		this.loading = true;
		this.movieService.getPaginatedMovies(10, this.movies.pageInfo.startCursor).subscribe(({ data, loading }) =>{
			if (!loading) {
				this.movies = data.paginatedMovies;
				this.loading = false;
			}
		});
	}

}
