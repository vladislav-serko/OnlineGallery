import { Component, OnInit } from '@angular/core';
import { User } from '../shared/models/user';
import { AuthService } from '../shared/services/auth.service';
import { UsersService } from '../shared/services/users.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDeleteDialog } from './dialog/confirm-delete-dialog';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css']
})
export class SettingsComponent implements OnInit {

  user: User;
  form: FormGroup = null;

  constructor(
    private authService: AuthService,
    private userService: UsersService,
    private snackBar: MatSnackBar,
    private dialog: MatDialog
  ) { }

  ngOnInit(): void {
    const userId = this.authService.getCurrentUser().id;
    this.userService.getUser(userId).subscribe(
      (data: User) => {
        this.user = data;
        this.initForm();
      }
    )
  }

  initForm() {
    this.form = new FormGroup({
      username: new FormControl(this.user.userName, [Validators.required]),
      firstname: new FormControl(this.user.firstName),
      lastname: new FormControl(this.user.lastName),
    });
  }

  updateUser() {
    var user: User = {
      id: this.user.id,
      userName: this.form.value.username,
      firstName: this.form.value.firstname,
      lastName: this.form.value.lastname
    }

    this.userService.update(user).subscribe(
      data => {
        this.snackBar.open("Changes saved", "OK", { duration: 2000 });
        this.user = user;
      },
      err => {
        this.snackBar.open("Something goes wrong", "OK", { duration: 2000 });
      }
    );
  }

  onDelete() {
    const dialogRef = this.dialog.open(ConfirmDeleteDialog);

    dialogRef.afterClosed().subscribe(
      result =>{
        if(result){
          this.deleteUser();
        }
      }
    )
  }

  deleteUser(){
    this.userService.delete(this.user.id).subscribe(
      data => {
        this.authService.logout();
      },
      err => {
        this.snackBar.open("Something goes wrong", "OK", { duration: 2000 });
      }
    );
  }


}
