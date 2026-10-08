import { HttpClient, HttpParams } from '@angular/common/http';
import { tap } from 'rxjs';
import { CategoryService } from './category-service';
import { Injectable } from '@angular/core';
import { BlogDto } from '../_models/blog';
import { Result } from '../_models/result';
import { Blog } from '../_admin-components/blog/blog';

@Injectable({
  providedIn: 'root'
})
export class BlogService {


  constructor(private http: HttpClient,
              private categoryService: CategoryService
  ){}

// backend adresi, api https profilinde 7000 portunda çalışıyor
baseUrl= "https://localhost:7000/api/blogs/";


// bütün kayıtları getiriyor
getAll(){
  return this.http.get<Result<BlogDto[]>>(this.baseUrl);
}

// ana sayfa ve sidebar'daki son 5 yazı
getLatest5Blogs(){
  return this.http.get<Result<BlogDto[]>>(this.baseUrl+"latest5blogs");
}

// yeni kayıt ekleme (post)
create(model:BlogDto){
  // kategori önbelleğinde blog listeleri de var, yeni yazı eklenince eskimesin diye temizliyorum
  return this.http.post<Result<BlogDto>>(this.baseUrl,model).pipe(tap(() => this.categoryService.refresh()));
}

// güncelleme (put), id body içinde gidiyor
update(model:BlogDto){
  return this.http.put(this.baseUrl,model).pipe(tap(() => this.categoryService.refresh()));
}

// id ile silme
delete(id:string){
  return this.http.delete(this.baseUrl+id).pipe(tap(() => this.categoryService.refresh()));
}

// bir kategoriye ait bütün yazılar (kategori sayfası için)
getBlogsByCategoryId(categoryId:string){
  return this.http.get<Result<BlogDto[]>>(this.baseUrl+"byCategoryId/"+categoryId);
}

// bir etiketteki (konudaki) bütün yazılar
getBlogsByTag(slug:string){
  return this.http.get<Result<BlogDto[]>>(this.baseUrl+"byTag/"+slug);
}

// detay sayfasının altındaki "bu konudaki diğer yazılar"
getRelatedBlogs(id:string){
  return this.http.get<Result<BlogDto[]>>(this.baseUrl+id+"/related");
}

// arama, kelimeyi query string ile gönderiyorum
// NOT: HttpParams kullanınca özel karakterler (#, & gibi) otomatik encode ediliyor,
// "?q=" + text diye elle birleştirseydim "c#" araması bozulurdu
search(text:string){
  return this.http.get<Result<BlogDto[]>>(this.baseUrl+"search", { params: new HttpParams().set("q", text) });
}

// tek kayıt getiriyor
getBlogById(id:string){
  return this.http.get<Result<BlogDto>>(this.baseUrl+id);
}



}
