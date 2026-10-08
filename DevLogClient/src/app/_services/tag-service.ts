import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Result } from '../_models/result';
import { TagDto } from '../_models/tag';
import { Observable, shareReplay } from 'rxjs';
import { getLanguage } from '../_i18n/i18n';

@Injectable({
  providedIn: 'root'
})
export class TagService {
  // backend adresi, api https profilinde 7000 portunda çalışıyor
  baseUrl = "https://localhost:7000/api/tags/";

  constructor(private http: HttpClient) { }

  // etiketler ana sayfada, etiket sayfasında ve admin formunda kullanılıyor, sık değişmiyor
  // kategori servisindeki gibi shareReplay ile önbelleğe alıyorum (dil başına ayrı)
  private cache: { [language: string]: Observable<Result<TagDto[]>> } = {};

  // bütün etiketler, yazı sayısına göre çoktan aza sıralı geliyor
  getAll() {
    const language = location.pathname.startsWith('/admin') ? 'tr' : getLanguage();
    if (!this.cache[language]) {
      this.cache[language] = this.http.get<Result<TagDto[]>>(this.baseUrl).pipe(shareReplay(1));
    }
    return this.cache[language];
  }
}
