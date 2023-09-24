import { Component } from '@angular/core';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})

export class Login{
  currentPage: string = 'login';

  changeView(page: string) {
    this.currentPage = page;
  }
}

