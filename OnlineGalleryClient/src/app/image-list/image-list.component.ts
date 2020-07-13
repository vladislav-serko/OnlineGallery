import { Component, OnInit, Input, ViewChild, Output } from '@angular/core';
import { ImageInterface } from '../shared/models/image-interface';
import { NgxMasonryComponent } from 'ngx-masonry';
import { PagedData } from '../shared/models/paged-data';
import { ImagesService } from '../shared/services/images.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { EventEmitter } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';

@Component({
  selector: 'app-image-list',
  templateUrl: './image-list.component.html',
  styleUrls: ['./image-list.component.css']
})
export class ImageListComponent implements OnInit {

  @Input() pagedData: PagedData<ImageInterface>;
  @Input() showAdditionalOptions: boolean;
  @Output() onPageChange: EventEmitter<number> = new EventEmitter();
  @ViewChild(NgxMasonryComponent) masonry: NgxMasonryComponent;

  private placed: boolean = false;

  constructor(
    private imagesService: ImagesService,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {

  }

  reload() {
    this.masonry.reloadItems();
    this.masonry.layout();
  }

  onImageDelete(id: string) {
    const index = this.pagedData.data.findIndex((x) => x.id == id);
    this.pagedData.data.splice(index, 1);
    this.reload();
  }

  pageChanged(pageEvent: PageEvent) {
    this.onPageChange.emit(pageEvent.pageIndex + 1);
  }

} 
