import { Component, OnInit } from '@angular/core';
import { SharedModule } from '../../shared/shared.module';
import { MovieService } from '../../services/movie/movie.service';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { faArrowCircleLeft, faArrowCircleRight } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';

@Component({
  selector: 'app-movies-paginated',
  standalone: true,
  imports: [SharedModule, MatTableModule, MatPaginatorModule, FontAwesomeModule ],
  templateUrl: './movies-paginated.component.html',
  styleUrl: './movies-paginated.component.scss'
})
export class MoviesPaginatedComponent implements OnInit {

	displayedColumns: string[] = ['movieId', 'name', 'view'];
	faArrowCircleLeft = faArrowCircleLeft;
	faArrowCircleRight = faArrowCircleRight;

	public movies: any;
	public loading: boolean = true;

	constructor(private movieService: MovieService) {
		
	}

	ngOnInit(): void {
		this.movieService.getCursorPaginatedMovies(10, true).subscribe(({ data, loading }) =>{
			if (!loading) {
				this.movies = data.paginatedMovies;
				this.loading = false;
			}
		});
	}

	getNextPage() {
		if (!this.movies || !this.movies.pageInfo || !this.movies.pageInfo.hasNextPage) {return;}
		this.loading = true;
		this.movieService.getCursorPaginatedMovies(10, true, this.movies.pageInfo.endCursor).subscribe(({ data, loading }) =>{
			if (!loading) {
				this.movies = data.paginatedMovies;
				this.loading = false;
				console.log(this.movies);
			}
		});
	}

	getPreviousPage() {
		if (!this.movies && !this.movies.pageInfo.hasPreviousPage) {return;}
		this.loading = true;
		this.movieService.getCursorPaginatedMovies(10, false, this.movies.pageInfo.startCursor).subscribe(({ data, loading }) =>{
			if (!loading) {
				this.movies = data.paginatedMovies;
				this.loading = false;
				console.log(this.movies);
			}
		});
	}

	pageEvent($event: PageEvent) {
		console.log($event);
		if (this.movies) {
			this.loading = true;
			const cursor = $event.previousPageIndex == null ? null : $event.previousPageIndex < $event.pageIndex ? this.movies.pageInfo.endCursor : this.movies.pageInfo.startCursor;
			this.movieService.getCursorPaginatedMovies($event.pageSize, cursor).subscribe(({ data, loading }) =>{
				if (!loading) {
					this.movies = data.paginatedMovies;
					this.loading = false;
				}
			});
		}
	}

}
