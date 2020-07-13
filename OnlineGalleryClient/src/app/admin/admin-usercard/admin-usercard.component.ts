import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { UserWithRoles } from 'src/app/shared/models/user-with-roles';
import { UsersService } from 'src/app/shared/services/users.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { PageEvent } from '@angular/material/paginator';

@Component({
  selector: 'app-admin-usercard',
  templateUrl: './admin-usercard.component.html',
  styleUrls: ['./admin-usercard.component.css']
})
export class AdminUsercardComponent implements OnInit {

  @Input() user: UserWithRoles;
  @Output() delete: EventEmitter<string> = new EventEmitter;
  isModer: boolean;

  constructor(
    private userService: UsersService,
    private snackBar: MatSnackBar,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.isModer = this.user.roles.includes("Moderator");
  }

  toModer() {
    this.userService.toModerator(this.user.id).subscribe(
      data=>{
        this.isModer = true;
      },
      err => {
        this.showError();
      }
    )
  }

  toUser() {
    this.userService.toUser(this.user.id).subscribe(
      data=>{
        this.isModer = false;
      },
      err => {
        this.showError();
      }
    )
  }

  onDelete() {
    this.userService.delete(this.user.id).subscribe(
      data=>{
        this.delete.emit(this.user.id);
      },
      err => {
        console.log(err);
        this.showError();
      }
    )
  }

  showError(){
    this.snackBar.open("Something goes wrong", "Ok", {duration: 2000});
  }

  navigateToUserProfile(){
    this.router.navigate(["user"], {queryParams:{id:this.user.id, page:1}})
  }

}
