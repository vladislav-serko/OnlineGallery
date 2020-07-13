import { Component, OnInit, Input, Inject } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ImageInterface } from 'src/app/shared/models/image-interface';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-update-image-dialog',
  templateUrl: './update-image-dialog.component.html',
  styleUrls: ['./update-image-dialog.component.css']
})
export class UpdateImageDialogComponent implements OnInit {
  constructor(
    public dialogRef: MatDialogRef<UpdateImageDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public image: ImageInterface
  ) { }
  updateForm: FormGroup;

  ngOnInit(): void {
    this.initForm();
  }

  initForm() {
    this.updateForm = new FormGroup({
      shortDescription: new FormControl(this.image.shortDescription, Validators.maxLength(40)),
      description: new FormControl(this.image.description, Validators.maxLength(300)),
    })
  }

  closeDialog() {
    var changed = { ...this.image };
    changed.shortDescription = this.updateForm.value.shortDescription;
    changed.description = this.updateForm.value.description;
    this.dialogRef.close(changed);
  }
}
