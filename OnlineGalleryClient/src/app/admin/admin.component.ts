import { Component, OnInit } from '@angular/core';
import { PagedData } from '../shared/models/paged-data';
import { User } from '../shared/models/user';
import { UsersService } from '../shared/services/users.service';
import { UserWithRoles } from '../shared/models/user-with-roles';
import { PageEvent } from '@angular/material/paginator';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {

  readonly defaultPage: number = 1;
  pagedData: PagedData<UserWithRoles>;
  page: number = this.defaultPage;
  query: string;

  constructor(
    private userService: UsersService
  ) { }

  ngOnInit(): void {
  }

  getUsers(){
    this.userService.search(this.query, this.page).subscribe(
      (data: PagedData<UserWithRoles>)=>{
        this.pagedData = data;
      }
    );
  }

  onDelete(id: string){
    const user = this.pagedData.data.find(u => u.id == id);
    const index = this.pagedData.data.indexOf(user);
    this.pagedData.data.splice(index, 1);
  }

  navigateToPage(event: PageEvent){
    this.page = event.pageIndex+1;
    this.getUsers();
  }

  
}
