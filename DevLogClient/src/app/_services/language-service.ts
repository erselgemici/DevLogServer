import { Injectable } from '@angular/core';
import { getLanguage, setLanguage, SUPPORTED_LANGUAGES, t } from '../_i18n/i18n';

// seçili dili yöneten servis, dil seçicide kullanıyorum
@Injectable({
  providedIn: 'root'
})
export class LanguageService {

  readonly languages = SUPPORTED_LANGUAGES;
  readonly current = getLanguage();

  constructor() {
    // html etiketinin lang özelliği de seçili dile göre olsun
    document.documentElement.lang = this.current;
  }

  // dil değişince sayfayı yeniliyorum, böylece api'den içerikler de yeni dilde geliyor
  change(lang: string) {
    if (lang === this.current) return;
    setLanguage(lang);
    location.reload();
  }

  translate(key: string): string {
    return t(key);
  }
}
