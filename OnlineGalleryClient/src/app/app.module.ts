import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {MatButtonModule} from '@angular/material/button';
import { TopbarComponent } from './topbar/topbar.component'; 
import {MatToolbarModule} from '@angular/material/toolbar'; 
import {MatIconModule} from '@angular/material/icon'; 
import {TextFieldModule} from '@angular/cdk/text-field'; 
import {MatFormFieldModule} from '@angular/material/form-field';
import { NgMatSearchBarModule } from 'ng-mat-search-bar';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatCardModule} from '@angular/material/card'
import { MatInputModule } from '@angular/material/input';
import { UserComponent } from './user/user.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from "@angular/common/http";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {MatSnackBarModule} from '@angular/material/snack-bar';
import { UserInfoComponent } from './user/user-info/user-info.component';
import { ImageListComponent } from './image-list/image-list.component';
import { CardComponent } from './image-list/card/card.component'; 
import { UserGalleryComponent } from './user/user-gallery/user-gallery.component';
import {MatDividerModule} from '@angular/material/divider'; 
import { NgxMasonryModule } from 'ngx-masonry';
import {MatMenuModule} from '@angular/material/menu';
import { TokenInterceptor } from './interceptors/token-interceptor';
import {MatDialogModule} from '@angular/material/dialog';
import { AddimageDialogComponent } from './addimage-dialog/addimage-dialog.component';
import { MaterialFileInputModule } from 'ngx-material-file-input';
import {MatPaginatorModule} from '@angular/material/paginator';
import { UpdateImageDialogComponent } from './image-list/card/update-image-dialog/update-image-dialog.component';
import { SearchComponent } from './search/search.component';
import { AdminComponent } from './admin/admin.component';
import { AdminUsercardComponent } from './admin/admin-usercard/admin-usercard.component';
import { SettingsComponent } from './settings/settings.component'; 
import { ConfirmDeleteDialog } from './settings/dialog/confirm-delete-dialog';

@NgModule({
  declarations: [
    AppComponent,
    TopbarComponent,
    LoginComponent,
    RegisterComponent,
    UserComponent,
    UserInfoComponent,
    ImageListComponent,
    CardComponent,
    UserGalleryComponent,
    AddimageDialogComponent,
    UpdateImageDialogComponent,
    SearchComponent,
    AdminComponent,
    AdminUsercardComponent,
    SettingsComponent,
    ConfirmDeleteDialog
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatButtonModule,
    MatToolbarModule,
    MatIconModule,
    TextFieldModule,
    MatFormFieldModule,
    NgMatSearchBarModule,
    FlexLayoutModule,
    MatCardModule,
    MatInputModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    MatSnackBarModule,
    MatDividerModule,
    NgxMasonryModule,
    MatMenuModule,
    MatDialogModule,
    MaterialFileInputModule,
    MatPaginatorModule
  ],
  providers: [{
    provide: HTTP_INTERCEPTORS,
    useClass: TokenInterceptor,
    multi: true
}],
  bootstrap: [AppComponent]
})
export class AppModule { }
