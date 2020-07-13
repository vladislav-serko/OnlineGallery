import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ImageInterface } from 'src/app/shared/models/image-interface';
import { AuthService } from 'src/app/shared/services/auth.service';
import { MatDialog } from '@angular/material/dialog';
import { UpdateImageDialogComponent } from 'src/app/image-list/card/update-image-dialog/update-image-dialog.component';
import { ImagesService } from 'src/app/shared/services/images.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-card',
  templateUrl: './card.component.html',
  styleUrls: ['./card.component.css']
})
export class CardComponent implements OnInit {

  @Input() image: ImageInterface;
  @Input() showAdditionalOptions: boolean;

  @Output() delete: EventEmitter<string> = new EventEmitter<string>();
  @Output() update: EventEmitter<any> = new EventEmitter();

  showDelete: boolean;

  constructor(
    private authService: AuthService,
    private imagesService: ImagesService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
    this.showDelete = this.authService.isModerator();
  }

  onLike() {
    if (this.image.isLiked) {
      this.imagesService.unlikeImage(this.image.id).subscribe(
        data => {
          this.image.likeCount--;
        },
        err=>{
          console.log(err);
        }
      );
    } else {
      this.imagesService.likeImage(this.image.id).subscribe(
        data => {
          this.image.likeCount++;
        },
        err=>{
          console.log(err);
        }
      );
    }

    this.image.isLiked = !this.image.isLiked;
  }

  onDelete() {
    this.imagesService.deleteImage(this.image.id).subscribe( 
      data => {
        this.snackBar.open("Deleted successfully", "OK", { duration: 2000 });
        this.delete.emit(this.image.id);
      }, err => {
        this.snackBar.open("Something goes wrong", "OK", { duration: 2000 });
      }
    );
    
  }

  launchUpdateDialog() {
    const dialogRef = this.dialog.open(UpdateImageDialogComponent, {
      data: this.image
    });

    dialogRef.afterClosed().subscribe(result => this.updateImage(result));
  }

  updateImage(image: ImageInterface) {
    if (image)
      this.imagesService.updateImage(image).subscribe(
        data => {
          this.image = image;
          this.update.emit();
        },
        err => {
          console.log(err);
          this.snackBar.open("Unable to update image. Please try again.", "OK")
        }
      )
  }
  
}
