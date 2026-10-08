import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Result } from '../_models/result';
import { UserListDto } from '../_models/userListDto';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) {}

  // backend adresi, api https profilinde 7000 portunda çalışıyor
  baseUrl = "https://localhost:7000/api/users/";

  // kullanıcıları rolleriyle birlikte getiriyor (sadece admin)
  getAll() {
    return this.http.get<Result<UserListDto[]>>(this.baseUrl);
  }

  // kullanıcının rolünü değiştiriyor
  changeRole(userId: string, role: string) {
    return this.http.put(this.baseUrl + "role", { userId, role });
  }
}
