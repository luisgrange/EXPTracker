import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IUser } from '../../Models/User/user.model';

const headers = new HttpHeaders({
  'Content-Type': 'application/json'
});

const options = { headers };

@Injectable({
  providedIn: 'root'
})

export class AuthService {
  private apiUrl = 'https://localhost:7144/api/identity';

  headers = new HttpHeaders({
    'Content-Type': 'application/json'
  });

  options = {
    headers: this.headers
  }
  constructor(private http: HttpClient) { }

  login(credentials: IUser): Observable<IUser> {
    return this.http.post<IUser>("https://localhost:7144/api/identity/authentication", credentials, this.options);
  }

  register(user: IUser): Observable<any> {
    return this.http.post(`${this.apiUrl}/create-account`, user);
  }

  logout(): Observable<any> {
    return this.http.post(`${this.apiUrl}/logout`, {});
  }

  isAuthenticated(): boolean {

    return !!localStorage.getItem('token'); // Supondo que o token esteja armazenado no localStorage
  }

}
