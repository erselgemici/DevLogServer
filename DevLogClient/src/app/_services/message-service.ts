import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Result } from '../_models/result';
import { MessageDto } from '../_models/messageDto';

@Injectable({
  providedIn: 'root'
})
export class MessageService {

constructor(private http: HttpClient

  ){}

// backend adresi, api https profilinde 7000 portunda çalışıyor
baseUrl= "https://localhost:7000/api/messages/";


  // bütün kayıtları getiriyor
  getAll(){
    return this.http.get<Result<MessageDto[]>>(this.baseUrl);
  }

  // okunmuş mesajlar
  getReadMessages(){
    return this.http.get<Result<MessageDto[]>>(this.baseUrl+"read");
  }

  // okunmamış mesajlar
  getUnreadMessages(){
    return this.http.get<Result<MessageDto[]>>(this.baseUrl+"unread");
  }


  // yeni kayıt ekleme (post)
  create(model:MessageDto){
    return this.http.post<Result<MessageDto>>(this.baseUrl,model);
  }

  // güncelleme (put), id body içinde gidiyor
  update(model:MessageDto){
    return this.http.put(this.baseUrl,model);
  }

  // id ile silme
  delete(id:string){
    return this.http.delete(this.baseUrl+id);
  }

  // tek kayıt getiriyor
  getBlogById(id:string){
    return this.http.get<Result<MessageDto>>(this.baseUrl+id);
  }
}
