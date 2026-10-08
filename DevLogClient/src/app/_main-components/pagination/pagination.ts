import { Component, EventEmitter, Input, OnChanges, Output } from '@angular/core';

// tekrar kullanılabilir sayfalama component'i
// toplam kayıt sayısını ve sayfa boyutunu alıp sayfa numaralarını çiziyor,
// tıklanan sayfayı pageChange ile parent component'e bildiriyor
// @Input  = parent'tan veri alma  -> [totalItems]="blogs.length"
// @Output = parent'a olay gönderme -> (pageChange)="onPageChange($event)"
// başka sayfalarda da (ör: admin tabloları) aynen kullanabilirim
@Component({
  selector: 'app-pagination',
  standalone: false,
  templateUrl: './pagination.html',
  styleUrl: './pagination.css'
})
export class Pagination implements OnChanges {
  @Input() totalItems = 0;
  @Input() pageSize = 6;
  @Input() currentPage = 1;
  @Output() pageChange = new EventEmitter<number>();

  totalPages = 0;
  pages: (number | null)[] = [];

  // ngOnChanges: @Input değerlerinden biri değişince Angular bunu otomatik çağırıyor
  ngOnChanges() {
    this.totalPages = Math.ceil(this.totalItems / this.pageSize);
    this.pages = this.buildPages();
  }

  // çok sayfa olunca hepsini göstermek yerine 1 ... 4 5 6 ... 10 şeklinde kısaltıyorum
  // null olan yerler "..." olarak gösteriliyor
  buildPages(): (number | null)[] {
    const total = this.totalPages;
    const current = this.currentPage;

    if (total <= 7) {
      return Array.from({ length: total }, (_, i) => i + 1);
    }

    const result: (number | null)[] = [1];
    const start = Math.max(2, current - 1);
    const end = Math.min(total - 1, current + 1);

    if (start > 2) result.push(null);
    for (let i = start; i <= end; i++) result.push(i);
    if (end < total - 1) result.push(null);

    result.push(total);
    return result;
  }

  goTo(page: number | null) {
    if (page === null || page < 1 || page > this.totalPages || page === this.currentPage) return;
    this.pageChange.emit(page);
  }
}
