import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Result } from '../_models/result';
import { DashboardDto } from '../_models/dashboard';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  baseUrl = "https://localhost:7000/api/dashboard";

  constructor(private http: HttpClient) { }

  // token'ı interceptor ekliyor, giriş yapmamışsa 401 döner
  get() {
    return this.http.get<Result<DashboardDto>>(this.baseUrl);
  }
}
