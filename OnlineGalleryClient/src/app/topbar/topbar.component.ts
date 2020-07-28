import { Component, OnInit } from '@angular/core';
import { AuthService } from '../shared/services/auth.service';
import { MatDialog } from '@angular/material/dialog';
import { AddimageDialogComponent } from '../addimage-dialog/addimage-dialog.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-topbar',
  templateUrl: './topbar.component.html',
  styleUrls: ['./topbar.component.css']
})
export class TopbarComponent implements OnInit {
  isLogedIn: boolean = true;
  query: string;

  constructor(
    public authService: AuthService,
    public dialog: MatDialog,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.isLogedIn = this.authService.isLoggedIn();
  }

  logout() {
    this.authService.logout();
  }

  launchDialog() {
    this.dialog.open(AddimageDialogComponent, {autoFocus:false})
  }

  goToResults(){
    this.router.navigate(["search"], {queryParams:{q:this.query, page: 1}});
  }

  navigateToCurrentProfile(){
    this.router.navigateByUrl("user", {queryParams:{id:this.authService.getCurrentUser().id, page:1}});  
  }
}
