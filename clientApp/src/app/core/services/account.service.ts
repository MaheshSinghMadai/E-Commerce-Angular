import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Address, User } from '../../shared/models/user';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  baseUrl = environment.apiUrl;
  http = inject(HttpClient);
  currentUser = signal<User | null>(null);

  login(values: any) {
    let params = new HttpParams();
    params = params.append('useCookies', true);
    return this.http.post<User>(this.baseUrl + 'api/login', values, { params });
  }

  register(values: any) {
    return this.http.post(this.baseUrl + 'Account/Register', values);
  }

  getUserInfo() {
    return this.http
      .get<User>(this.baseUrl + 'Account/GetUserInfo')
      .pipe(map(
        user => {
          this.currentUser.set(user);
          return user;
        }
      ));
  }

  logout() {
    return this.http.post(this.baseUrl + 'Account/logout', {});
  }

  updateAddress(address: Address) {
    return this.http.post(this.baseUrl + 'Account/address', address);
  }
}
