import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Result } from '../_models/result';
import { SocialDto } from '../_models/socialDto';

@Injectable({
  providedIn: 'root'
})
export class SocialService {
  constructor(private http: HttpClient

  ){}

// backend adresi, api https profilinde 7000 portunda çalışıyor
baseUrl= "https://localhost:7000/api/socials/";


  // bütün kayıtları getiriyor
  getAll(){
    return this.http.get<Result<SocialDto[]>>(this.baseUrl);
  }

  // yeni kayıt ekleme (post)
  create(model:SocialDto){
    return this.http.post<Result<SocialDto>>(this.baseUrl,model);
  }

  // güncelleme (put), id body içinde gidiyor
  update(model:SocialDto){
    return this.http.put(this.baseUrl,model);
  }

  // id ile silme
  delete(id:string){
    return this.http.delete(this.baseUrl+id);
  }

  // tek kayıt getiriyor
  getBlogById(id:string){
    return this.http.get<Result<SocialDto>>(this.baseUrl+id);
  }
}
