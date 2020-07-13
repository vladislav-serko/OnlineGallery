import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ImagesService } from '../shared/services/images.service';
import { Location } from '@angular/common'
import { PagedData } from '../shared/models/paged-data';
import { ImageInterface } from '../shared/models/image-interface';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit {
  query: string;
  page: number;
  pagedData: PagedData<ImageInterface>;
  showError: boolean = true;
  readonly defaultPage = 1;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private location: Location,
    private imagesService: ImagesService
  ) { }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      const query = params["q"];
      const page = params["page"];
      this.checkNavigation(query, page);
      this.getImages();
    })
  }

  checkNavigation(query: string, page: number) {
    if (!query)
      this.location.back();

    if (!page || isNaN(+page)) {
      page = this.defaultPage;
      this.router.navigate(['search'], {
        queryParams: {
          q: query,
          page: page
        }
      })
    }

    this.query = query;
    this.page = page;
  }

  getImages() {
    this.imagesService.searchImage(this.query, this.page).subscribe(
      (data: PagedData<ImageInterface>) =>{
        this.pagedData = data;
        this.checkPage();
        this.showError = this.pagedData.data.length<1;
      }
    )
  }

  checkPage() {
    var page = Number(this.route.snapshot.paramMap.get('page'));
    if (page != this.pagedData.currentPage) {
      this.router.navigate(
        [],
        {
          relativeTo: this.route,
          queryParams: { page: this.pagedData.currentPage },
          queryParamsHandling: 'merge',
        });
    }
  }

  navigateToPage(page: number) {
    this.page = page;
    this.getImages();
  }


}
