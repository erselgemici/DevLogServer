import { Component, Input } from '@angular/core';
import { BlogDto } from '../../_models/blog';

// yazı kartı: kategori, etiket, arama sayfalarında ve "ilgili yazılar"da aynı kart kullanılıyor
// önce her sayfada html'i kopyalamıştım, değişiklik yapınca 3 yeri düzeltmem gerekti,
// o yüzden ayrı component yaptım (DRY: kendini tekrar etme)
@Component({
  selector: 'app-post-card',
  standalone: false,
  templateUrl: './post-card.html',
  styleUrl: './post-card.css'
})
export class PostCard {
  @Input() blog: BlogDto;
  // ilgili yazılar bölümünde kart daha küçük, özet göstermiyorum
  @Input() compact = false;
}
