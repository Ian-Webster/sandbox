import { Component, OnInit } from '@angular/core';
import { MovieService } from '../../services/movie/movie.service';
import { SharedModule } from '../../shared/shared.module';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
	selector: 'app-movie',
	standalone: true,
	imports: [SharedModule],
	templateUrl: './movie.component.html',
	styleUrl: './movie.component.scss'
})
export class MovieComponent implements OnInit {

	public loading: boolean = true;
	public movie: any = null;
	public genre: string = '';

	private movieId: string|null = '';

	public constructor(private movieService: MovieService, private route: ActivatedRoute) {
		this.movieId = this.route.snapshot.paramMap.get('id');
	}

	public ngOnInit(): void {

		if (!this.movieId) {
			this.loading = false;
			return;
		}

		this.movieService.getMovieById(this.movieId).subscribe(result => {
			this.movie = result.data.movie;
			this.genre = this.movie.movieGenres.map((x: any) => x.genre);
			this.loading = result.loading;
		});

	}

	
}
