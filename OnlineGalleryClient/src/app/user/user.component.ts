import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { AuthService } from '../shared/services/auth.service';
import { User } from '../shared/models/user';
import { UsersService } from '../shared/services/users.service';
import { identifierModuleUrl } from '@angular/compiler';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {
  user: User;
  userId: string;
  page: number;
  userFound: boolean = true;
  readonly defaultPage = 1;

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private authService: AuthService,
    private userService: UsersService,
  ) { }

  ngOnInit(): void {
    this.checkNavigation();
    this.setUser();
  }

  checkNavigation() {
    this.activatedRoute.queryParams.subscribe(params => {
      var userId = params['id'];
      var page = params['page'];
      var problem = false;
      if (!userId) {
        userId = this.authService.getCurrentUser().id;
        problem = true;
      }
      if (!page || isNaN(+page)) {
        page = this.defaultPage;
        problem = true;
      }
      if (problem)
        this.router.navigate(['user'], { queryParams: { id: userId, page: page } })
      this.page = page;
      this.userId = userId;
    });
  }

  setUser() {
    this.userService.getUser(this.userId).subscribe(
      data => {
        this.userFound = true;
        this.user = data
      },
      err => {
        if (err.status == 404) {
          this.userFound = false;
        }
      }
    );
  }

}
