import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Result } from '../_models/result';
import { ContactInfoDto } from '../_models/contactInfoDto';

@Injectable({
  providedIn: 'root'
})
export class ContactInfoService {


   constructor(private http: HttpClient

  ){}

// backend adresi, api https profilinde 7000 portunda çalışıyor
baseUrl= "https://localhost:7000/api/contactInfos/";


// bütün kayıtları getiriyor
getAll(){
  return this.http.get<Result<ContactInfoDto[]>>(this.baseUrl);
}

// yeni kayıt ekleme (post)
create(model:ContactInfoDto){
  return this.http.post<Result<ContactInfoDto>>(this.baseUrl,model);
}

// güncelleme (put), id body içinde gidiyor
update(model:ContactInfoDto){
  return this.http.put(this.baseUrl,model);
}

// id ile silme
delete(id:string){
  return this.http.delete(this.baseUrl+id);
}

// tek kayıt getiriyor
getBlogById(id:string){
  return this.http.get<Result<ContactInfoDto>>(this.baseUrl+id);
}

}
