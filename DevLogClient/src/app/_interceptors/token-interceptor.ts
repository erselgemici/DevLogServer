import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { getLanguage } from '../_i18n/i18n';

@Injectable({
  providedIn: 'root'
})
// giden her http isteğine token'ı otomatik ekleyen interceptor, servislerde tek tek header yazmamak için
export class TokenInterceptor implements HttpInterceptor {

intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
  // api içerikleri bu dile göre dönüyor. admin panelde her zaman Türkçe (asıl) veriyle çalışıyorum,
  // İngilizce karşılıklar formda ayrı alanlarda düzenleniyor
  const language = location.pathname.startsWith('/admin') ? 'tr' : getLanguage();
  req = req.clone({ setHeaders: { 'Accept-Language': language } });

  const token = localStorage.getItem("token");

// token varsa isteği kopyalayıp Authorization header'ı ekliyorum
if(token){
  const clonedRequest = req.clone({
    setHeaders: {
      Authorization: "Bearer "+token
    }
  });
  return next.handle(clonedRequest);

}

// token yoksa istek olduğu gibi gidiyor (ör: ana sayfa)
return next.handle(req);



}



}
