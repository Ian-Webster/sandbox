import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { SharedModule } from '../../shared/shared.module';
import { MovieService } from '../../services/movie/movie.service';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatInputModule } from '@angular/material/input';
import { MatSelectChange, MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { faArrowCircleLeft, faArrowCircleRight, faEye } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { CursorPage } from '../../domain/models/paging/cusor-page';

@Component({
	selector: 'app-movies-paginated',
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
	templateUrl: './movies-paginated.component.html',
	styleUrl: './movies-paginated.component.scss'
})
export class MoviesPaginatedComponent implements OnInit {

	displayedColumns: string[] = ['movieId', 'name', 'view'];
	faArrowCircleLeft = faArrowCircleLeft;
	faArrowCircleRight = faArrowCircleRight;
	faEye = faEye;

	public movies: any;
	public loading: boolean = true;
	public pageSize: number = 5;
	public pageSizes: Array<number> = [5, 10, 25];

	private pages: Array<CursorPage> = new Array<CursorPage>();
	private pageIndex: number = 0;

	constructor(private movieService: MovieService) {

	}

	ngOnInit(): void {
		this.pages = new Array<CursorPage>();
		this.pageIndex = 0;
		this.movieService.getCursorPaginatedMovies(this.pageSize).subscribe(({ data, loading }) => {
			if (!loading) {
				this.movies = data.paginatedMovies;
				this.pages.push({ index: 0, cursor: undefined }); // first page has no cursor so push undefined
				this.loading = false;
			}
		});
	}

	public onPageSizeChange($event: MatSelectChange) {
		this.pageSize = $event.value;
		this.ngOnInit();
	}

	public getNextPage() {
		if (!this.movies || !this.movies.pageInfo || !this.movies.pageInfo.hasNextPage) { return; }
		this.loading = true;
		const currentNextCursor = this.movies.pageInfo.endCursor;
		this.movieService.getCursorPaginatedMovies(this.pageSize, this.movies.pageInfo.endCursor).subscribe(({ data, loading }) => {
			if (!loading) {
				this.movies = data.paginatedMovies;
				this.pageIndex++;
				this.pages.push({ index: this.pageIndex, cursor: currentNextCursor });
				this.loading = false;
			}
		});
	}

	public getPreviousPage() {
		if (!this.movies && !this.movies.pageInfo.hasPreviousPage) { return; }
		this.loading = true;
		this.pageIndex--;
		this.movieService.getCursorPaginatedMovies(this.pageSize, this.pages.find(p => p.index === this.pageIndex)?.cursor).subscribe(({ data, loading }) => {
			if (!loading) {
				this.movies = data.paginatedMovies;
				this.loading = false;
			}
		});
	}

}
