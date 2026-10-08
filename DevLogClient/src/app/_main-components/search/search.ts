import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject, Subscription, debounceTime, distinctUntilChanged, filter, of, switchMap, tap, catchError } from 'rxjs';
import { BlogService } from '../../_services/blog-service';
import { BlogDto } from '../../_models/blog';

// arama sayfası: /search?q=angular
// kutuya her harf yazıldığında istek atmak yerine RxJS ile akışı yönetiyorum:
//   debounceTime(400)      -> kullanıcı 400ms yazmayı bırakınca devam et
//   distinctUntilChanged() -> aynı kelime tekrar gelirse istek atma
//   filter                 -> 2 harften kısaysa arama (backend validator da 2 istiyor)
//   switchMap              -> yeni kelime gelince önceki isteği iptal et (eski cevap yenisini ezmesin)
@Component({
  selector: 'app-search',
  standalone: false,
  templateUrl: './search.html',
  styleUrl: './search.css'
})
export class Search implements OnInit, OnDestroy {
  text = '';
  results: BlogDto[] = [];
  loading = false;
  searched = false;

  // Subject: hem değer gönderebildiğim (next) hem dinleyebildiğim (subscribe) observable
  private searchText$ = new Subject<string>();
  private subscription: Subscription;

  constructor(private route: ActivatedRoute,
              private router: Router,
              private blogService: BlogService) { }

  ngOnInit() {
    this.subscription = this.searchText$.pipe(
      debounceTime(400),
      distinctUntilChanged(),
      tap(text => {
        // adres çubuğu da güncellensin, sayfa yenilenince ya da link paylaşılınca arama kaybolmasın
        this.router.navigate([], { queryParams: { q: text || null }, replaceUrl: true });
      }),
      filter(text => text.trim().length >= 2),
      tap(() => this.loading = true),
      switchMap(text => this.blogService.search(text.trim()).pipe(
        // hata olursa akış ölmesin diye boş sonuç dönüyorum, yoksa sonraki aramalar çalışmaz
        catchError(() => of({ data: [] as BlogDto[] }))
      ))
    ).subscribe(result => {
      this.results = result.data ?? [];
      this.loading = false;
      this.searched = true;
    });

    // header'daki kutudan ?q= ile gelindiyse aramayı başlat
    // snapshot yerine subscribe oldum: arama sayfasındayken header'dan yeni arama yapılırsa da çalışsın
    this.route.queryParamMap.subscribe(params => {
      const q = params.get('q') ?? '';
      if (q && q !== this.text) {
        this.text = q;
        this.searchText$.next(q);
      }
    });
  }

  onInput() {
    this.searchText$.next(this.text);
  }

  // NOT: subscribe olunan şeyi component kapanınca bırakmazsam memory leak oluyor
  ngOnDestroy() {
    this.subscription?.unsubscribe();
  }
}
