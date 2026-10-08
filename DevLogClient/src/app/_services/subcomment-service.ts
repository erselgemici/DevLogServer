import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Result } from '../_models/result';
import { SubCommentDto } from '../_models/subCommentDto';

@Injectable({
  providedIn: 'root'
})
export class SubCommentService {
  constructor(private http: HttpClient

  ){}

// backend adresi, api https profilinde 7000 portunda çalışıyor
baseUrl= "https://localhost:7000/api/subComments/";


  // bütün kayıtları getiriyor
  getAll(){
    return this.http.get<Result<SubCommentDto[]>>(this.baseUrl);
  }

  // yeni kayıt ekleme (post)
  create(model:SubCommentDto){
    return this.http.post<Result<SubCommentDto>>(this.baseUrl,model);
  }

  // güncelleme (put), id body içinde gidiyor
  update(model:SubCommentDto){
    return this.http.put(this.baseUrl,model);
  }

  // id ile silme
  delete(id:string){
    return this.http.delete(this.baseUrl+id);
  }

  // tek kayıt getiriyor
  getBlogById(id:string){
    return this.http.get<Result<SubCommentDto>>(this.baseUrl+id);
  }
}
