import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpRequest } from '@angular/common/http';
import {Http, ResponseContentType} from '@angular/http';
import { Router } from '@angular/router';
import { ImageInterface } from '../models/image-interface';
import { MatSnackBar } from '@angular/material/snack-bar';
import { baseUrl } from 'src/environments/environment';
import { PagedData } from '../models/paged-data';
import { FormGroup } from '@angular/forms';
import { AuthService } from './auth.service';
import { ElementSchemaRegistry } from '@angular/compiler';

@Injectable({
  providedIn: 'root'
})
export class ImagesService {
  itemCount: number = 15;

  constructor(
    private http: HttpClient,
    private authService: AuthService
  ) { }

  getImages(userId: string, page: number) {

    return this.http.get<PagedData<ImageInterface>>(`${baseUrl}images`, {
      params: {
        userId: userId,
        page: page.toString(),
        itemCount: this.itemCount.toString()
      }
    });
  }

  addImage(data: any) {
    var formData = new FormData();
    formData.append("userId", this.authService.getCurrentUser().id);
    formData.append("shortDescription", data.shortDescription);
    formData.append("description", data.description);
    formData.append("file", data.file._files[0]);

    return this.http.post<ImageInterface>(`${baseUrl}images`, formData);
  }

  deleteImage(id: string) {
    return this.http.delete(`${baseUrl}images/${id}`);
  }

  updateImage(image: ImageInterface) {
    return this.http.put(`${baseUrl}images`, image);
  }

  likeImage(id: string) {
    var formData = new FormData();
    formData.append("id", id);
    return this.http.post(`${baseUrl}images/like`, formData);
  }

  unlikeImage(id: string) {
    var formData = new FormData();
    formData.append("id", id);
    return this.http.post(`${baseUrl}images/unlike`, formData);
  }
  
  searchImage(query: string, page: number){
    return this.http.get(`${baseUrl}images/search`, {
      params:{
        page: page.toString(),
        itemCount : this.itemCount.toString(),
        query: query
      }
    });
  }

  downloadFullImage(id: string){
    return this.http.get(`${baseUrl}imageFiles/${id}/full`, {observe: "response", responseType: "blob"});
  }

} 
