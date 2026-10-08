import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CommentDto } from '../_models/commentDto';
import { Result } from '../_models/result';

@Injectable({
  providedIn: 'root'
})
export class CommentService {
  // backend adresi, api https profilinde 7000 portunda çalışıyor
  baseUrl = "https://localhost:7000/api/comments/"


  constructor(private http : HttpClient) {

}


// bütün kayıtları getiriyor
getAll(){
    return this.http.get<Result<CommentDto[]>>(this.baseUrl);
  }

// yeni kayıt ekleme (post)
create(commentDto:CommentDto){
    return this.http.post<Result<CommentDto>>(this.baseUrl,commentDto);
  }

  // güncelleme (put), id body içinde gidiyor
  update(model:CommentDto){
    return this.http.put(this.baseUrl,model);
  }

  // id ile silme
  delete(id:string){
    return this.http.delete(this.baseUrl+id);
  }

  // tek kayıt getiriyor
  getById(id:string){
    return this.http.get<Result<CommentDto>>(this.baseUrl+id);
  }


}
