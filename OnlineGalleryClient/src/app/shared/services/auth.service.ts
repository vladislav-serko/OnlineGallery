import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { baseUrl } from 'src/environments/environment';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router'
import { UserData } from '../models/user-data';
import * as jwt_decode from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(
    private http: HttpClient,
    private snackBar: MatSnackBar,
    private router: Router,
  ) { }

  register(data: any) {
    return this.http.post<any>(`${baseUrl}account/register`, data).subscribe(
      data => {
        this.snackBar.open("Registered successfully", "OK", { duration: 2000 });
        this.router.navigateByUrl("/login");
      },
      err => {
        if (err.status == 400) {
          if (err.error.errors instanceof Array) {
            err.error.errors.forEach(element => {
              this.snackBar.open(element.description, "OK", { duration: 4000 });
            });
          } else {
            console.log(err);
          }
        }
      });
  }

  login(data: any) {
    return this.http.post<any>(`${baseUrl}account/login`, data).subscribe(
      data => {
        const userData = this.getUserData(data['token']);
        localStorage.setItem("currentUser", JSON.stringify(userData));

        this.snackBar.open("Login successful", "OK", { duration: 2000 });
        this.router.navigateByUrl('/user');
      },
      err => {
        if (err.status == 404)
          this.snackBar.open("Username or password are incorrect", "OK", { duration: 2000 });
      });
  }

  getUserData(token: string): UserData {
    const payload = jwt_decode(token);
    const userData: UserData = {
      id: payload['sub'],
      userName: payload['unique_name'],
      roles: payload['role'],
      token: token
    }

    return userData;
  }

  getCurrentUser(): UserData {
    return JSON.parse(localStorage.getItem('currentUser'));
  }

  isLoggedIn(): boolean {
    return localStorage.getItem("currentUser") != null;
  }

  isAdmin(): boolean {
    return this.getCurrentUser().roles.includes("Admin");
  }

  isModerator(): boolean {
    return this.getCurrentUser().roles.includes("Moderator");
  }

  logout() {
    localStorage.removeItem("currentUser");
    this.router.navigateByUrl('/login');
  }
}

