import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from '../models/user';
import { baseUrl } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UsersService {

  readonly itemCount = 20;
  readonly url = `${baseUrl}users`

  constructor(
    private http: HttpClient,
  ) { }

  getUser(id: string){
    return this.http.get<User>(`${this.url}/${id}`)
  }

  search(query: string, page: number){
    return this.http.get(`${this.url}/search`, {
      params:{
        page: page.toString(),
        itemCount : this.itemCount.toString(),
        query: query
      }
    });
  }

  update(user: User){
    return this.http.put(`${this.url}`, user);
  }

  delete(id: string){
    return this.http.delete(`${this.url}/${id}`); 
  }

  toModerator(id: string){
    return this.http.post(`${this.url}/${id}/toModerator`, null);
  }

  toUser(id: string){
    return this.http.post(`${this.url}/${id}/toUser`, null);
  }
}
