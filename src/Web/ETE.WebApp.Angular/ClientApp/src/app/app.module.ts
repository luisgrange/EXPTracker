import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './Pages/home/home.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { Login } from './Pages/Login/login.component';
import { HideMenuGuard } from './hide-menu.guard';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    FetchDataComponent,
    Login
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full', canActivate: [HideMenuGuard] },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: 'login', component: Login },
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
