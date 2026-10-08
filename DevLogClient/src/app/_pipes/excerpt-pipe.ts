import { Pipe, PipeTransform } from '@angular/core';

// liste sayfalarında içeriğin kısa özetini göstermek için
// markdown işaretlerini temizleyip kelimenin ortasından kesmeden kısaltıyor
// kullanım: {{ blog.summary | excerpt:150 }}  -> 150 = en fazla kaç karakter
// pipe'a iki nokta ile parametre veriliyor, transform metodunun ikinci parametresine geliyor
@Pipe({
  name: 'excerpt',
  standalone: false
})
export class ExcerptPipe implements PipeTransform {

  transform(text: string, length = 150): string {
    if (!text) return '';

    const plain = text
      .replace(/```[\s\S]*?```/g, ' ')     // kod blokları
      .replace(/^## .*$/gm, ' ')           // ara başlıklar
      .replace(/^- /gm, '')                // liste işaretleri
      .replace(/`|\*\*/g, '')              // satır içi kod ve kalın
      .replace(/\s+/g, ' ')
      .trim();

    if (plain.length <= length) return plain;

    const cut = plain.substring(0, length);
    const lastSpace = cut.lastIndexOf(' ');
    return (lastSpace > 0 ? cut.substring(0, lastSpace) : cut) + '...';
  }
}
