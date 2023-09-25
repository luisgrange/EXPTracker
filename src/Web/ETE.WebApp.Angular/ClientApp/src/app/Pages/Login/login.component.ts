import { Component, OnInit } from '@angular/core';
import { IUser } from '../../Models/User/user.model';
import { AuthService } from '../../Services/User/auth.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})

export class Login  implements OnInit{
  isLoginPage: boolean = true;
    
  credentials: IUser = {
    Email: "",
    Password: "",
  };


  
  constructor(private auth: AuthService) { }

  ngOnInit(): void{}

  login() {
    this.auth.login(this.credentials).subscribe({
      next: (user) => {
        this.credentials = user
        console.log("rodou");
      },
      error: (response) => {console.log(response);}
    })
    
    /**
     * this.http.post('https://localhost:7144/api/identity/authentication', this.credentials, this.options)
    .subscribe({
      next: (user) => {console.log("rodou");},
      error: (response) => {console.log(response)}
    });
     */
  }

  changeView(isLogin: boolean) {
    this.isLoginPage = isLogin;
  }
}

