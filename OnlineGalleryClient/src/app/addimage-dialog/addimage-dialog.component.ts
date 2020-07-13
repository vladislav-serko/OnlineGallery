import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators, ValidatorFn, AbstractControl } from '@angular/forms';
import { FileValidator } from 'ngx-material-file-input';
import { ImagesService } from '../shared/services/images.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-addimage-dialog',
  templateUrl: './addimage-dialog.component.html',
  styleUrls: ['./addimage-dialog.component.css']
})
export class AddimageDialogComponent implements OnInit {

  readonly maxSize = 10485760;
  readonly extensions = [".png",".jpg",".jpeg",".gif"];
  loading: boolean = false;
  addImageForm: FormGroup;

  constructor(
    private imagesService: ImagesService,
    private snackBar: MatSnackBar,
    private dialogRef: MatDialogRef<AddimageDialogComponent>
  ) { }

  ngOnInit(): void {
    this.initForm();
  }

  initForm() {
    this.addImageForm = new FormGroup({
      shortDescription: new FormControl("", Validators.maxLength(40)),
      description: new FormControl("", Validators.maxLength(300)),
      file: new FormControl("", [
        Validators.required, 
        FileValidator.maxContentSize(this.maxSize), 
        extensionValidator(this.extensions)
      ])
    });
  }

  addImage() {
    this.loading = true;
    this.imagesService.addImage(this.addImageForm.value).subscribe(
      data => {
        this.snackBar.open("Added successfully", "OK", { duration: 2000 });
        this.dialogRef.close();
        this.loading = false;
      },
      err => {
        this.snackBar.open("Something goes wrong, please try again", "OK", { duration: 2000 });
        this.loading = false;
      }
    );
  }
}

function extensionValidator(extentions: string[]): ValidatorFn {
  return (control: AbstractControl): { [key: string]: any } => {
    const re = /(?:\.([^.]+))?$/;
    const extension = re.exec(control.value._fileNames)[0];    
    const isForbidden = !extentions.includes(extension);
    return isForbidden ? { "forbiddenExtension": {extension: extension} } : null;
  }
}
