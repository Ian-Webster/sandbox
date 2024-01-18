import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { SharedModule } from '../../shared/shared.module';
import { MovieService } from '../../services/movie/movie.service';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { faArrowCircleLeft, faArrowCircleRight, faEye } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';

@Component({
	selector: 'app-movies-offset-paginated',
	standalone: true,
	imports: [
		SharedModule,
		MatTableModule,
		MatPaginatorModule,
		FontAwesomeModule,
		MatInputModule,
		MatSelectModule,
		MatFormFieldModule,
		RouterLink],
	templateUrl: './movies-offset-paginated.component.html',
	styleUrl: './movies-offset-paginated.component.scss'
})
export class MoviesOffsetPaginatedComponent implements OnInit {

	displayedColumns: string[] = ['movieId', 'name', 'view'];
	faArrowCircleLeft = faArrowCircleLeft;
	faArrowCircleRight = faArrowCircleRight;
	faEye = faEye;

	public movies: any;
	public loading: boolean = true;
	public pageSize: number = 5;
	public pageSizes: Array<number> = [5, 10, 25];
	public numberOfRecords: number = 0;
	public pageIndex: number = 0;
	
	constructor(private movieService: MovieService) {

	}

	public ngOnInit(): void {
		this.getMovies(this.pageSize, 0);
	}

	public onPage($event: PageEvent) {
		if (this.movies) {
			this.pageSize = $event.pageSize;
			this.getMovies(this.pageSize, $event.pageIndex * this.pageSize);
		}
	}

	private getMovies(pageSize: number, offset: number): void {
		this.loading = true;
		this.movieService.getOffsetPaginatedMovies(pageSize, offset).subscribe(({ data, loading }) => {
			if (!loading) {
				this.movies = data.offsetPaginatedMovies;
				this.numberOfRecords = this.movies.totalCount;
				this.pageIndex = Math.floor(offset / pageSize);  // recalculate pageIndex
				this.loading = false;
			}
		});
	}
}
