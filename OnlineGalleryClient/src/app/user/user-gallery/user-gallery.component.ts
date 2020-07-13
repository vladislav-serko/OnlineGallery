import { Component, OnInit, Input } from '@angular/core';
import { ImagesService } from 'src/app/shared/services/images.service';
import { ImageInterface } from 'src/app/shared/models/image-interface';
import { PagedData } from 'src/app/shared/models/paged-data';
import { AuthService } from 'src/app/shared/services/auth.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-user-gallery',
  templateUrl: './user-gallery.component.html',
  styleUrls: ['./user-gallery.component.css']
})
export class UserGalleryComponent implements OnInit {

  pagedData: PagedData<ImageInterface>;
  showAddtionalOptions: boolean;
  @Input() userId: string;
  @Input() page: number;

  constructor(
    public service: ImagesService,
    private authService: AuthService,
    private activatedRoute: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.getImages();
    this.showAddtionalOptions = this.authService.getCurrentUser().id == this.userId;
  }

  getImages() {
    this.service.getImages(this.userId, this.page).subscribe(
      (data: PagedData<ImageInterface>) => {
        this.pagedData = data;
        this.checkPage();
      }
    )
  }

  checkPage() {
    var page = Number(this.activatedRoute.snapshot.paramMap.get('page'));
    if (page != this.pagedData.currentPage) {
      this.router.navigate(
        [],
        {
          relativeTo: this.activatedRoute,
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
