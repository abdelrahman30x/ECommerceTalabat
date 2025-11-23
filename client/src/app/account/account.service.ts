import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, of, ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { IAddress } from '../shared/models/address';
import { IUser } from '../shared/models/user';

// أضف interface للـ AuthResponse
interface IAuthResponse {
  success: boolean;
  message: string;
  token: string;
  user: IUser;
}

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  private currentUserSource = new ReplaySubject<IUser>(1);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient, private router: Router) { }

  loadCurrentUser(token: string) {
    if (token == null) {
      this.currentUserSource.next(null);
      return of(null);
    }

    let headers = new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${token}`);

    // تصحيح الـ endpoint
    return this.http.get(this.baseUrl + 'Auth/me', {headers}).pipe(
      map((user: IUser) => {
        if (user) {
          // لا نحتاج لحفظ الـ token هنا لأنه موجود بالفعل
          this.currentUserSource.next(user);
        }
      })
    )
  }

  login(values: any) {
    return this.http.post<IAuthResponse>(this.baseUrl + 'Auth/login', values).pipe(
      map((response) => {
        if (response && response.success) {
          localStorage.setItem('token', response.token);
          this.currentUserSource.next(response.user);
        }
      })
    )
  }

  register(values: any) {
    return this.http.post<IAuthResponse>(this.baseUrl + 'Auth/register', values).pipe(
      map((response) => {
        if (response && response.success) {
          localStorage.setItem('token', response.token);
          this.currentUserSource.next(response.user);
        }
      })
    )
  }

  logout() {
    localStorage.removeItem('token');
    this.currentUserSource.next(null);
    this.router.navigateByUrl('/');
  }

  checkEmailExists(email: string) {
    return this.http.get(this.baseUrl + 'Auth/emailexists?email=' + email);
  }

  getUserAddress() {
    return this.http.get<IAddress>(this.baseUrl + 'Auth/Address');
  }

  updateUserAddress(address: IAddress) {
    return this.http.put<IAddress>(this.baseUrl + 'Auth/Address', address);
  }
}