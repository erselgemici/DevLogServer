import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Result } from '../_models/result';
import { CategoryDto } from '../_models/category';
import { Observable, shareReplay, tap } from 'rxjs';
import { getLanguage } from '../_i18n/i18n';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
// backend adresi, api https profilinde 7000 portunda çalışıyor
baseUrl = "https://localhost:7000/api/categories/"
/**
 *
 */
constructor(private http : HttpClient) {

}




// NOT (önbellek / cache): kategoriler header/footer, ana sayfa ve kategori sayfasında ayrı ayrı
// isteniyordu, aynı veri için her sayfa geçişinde 2-3 istek gidiyordu.
// shareReplay(1): ilk istekten gelen cevabı saklıyor, sonraki subscribe'larda tekrar istek atmadan aynı cevabı veriyor.
// cevap dile göre değiştiği için (tr/en) dil başına ayrı saklıyorum
private cache: { [language: string]: Observable<Result<CategoryDto[]>> } = {};

// kategorileri bloglarıyla beraber getiriyor
getCategories(){
    // admin panelde istekler her zaman Türkçe gidiyor (interceptor'daki kuralın aynısı)
    const language = location.pathname.startsWith('/admin') ? 'tr' : getLanguage();
    if (!this.cache[language]) {
      this.cache[language] = this.http.get<Result<CategoryDto[]>>(this.baseUrl).pipe(shareReplay(1));
    }
    return this.cache[language];
  }

  // kategori eklenince/silinince/güncellenince saklanan liste eskidi, bir sonraki istekte yeniden çekilsin
  private clearCache(){
    this.cache = {};
  }

  // yeni kayıt ekleme (post)
  // tap: cevabı değiştirmeden araya bir işlem sokmak için (burada önbelleği temizliyorum)
  create(categoryDto:CategoryDto){
    return this.http.post<Result<CategoryDto>>(this.baseUrl,categoryDto).pipe(tap(() => this.clearCache()));
  }


  // id ile silme
  delete(id:string){
    return this.http.delete(this.baseUrl+id).pipe(tap(() => this.clearCache()));
  }

  // güncelleme (put), id body içinde gidiyor
  update(model:CategoryDto){
    return this.http.put(this.baseUrl,model).pipe(tap(() => this.clearCache()));
  }

  // blog eklenip silinince kategorilerin içindeki blog listesi de değişiyor, blog servisi bunu çağırıyor
  refresh(){
    this.clearCache();
  }

}
