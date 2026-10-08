import { Pipe, PipeTransform } from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';

// iframe'e (google maps) veritabanından gelen url'yi verebilmek için güvenli olarak işaretliyorum
@Pipe({
  name: 'safeUrl',
  standalone: false
})
export class SafeUrlPipe implements PipeTransform {
  constructor(private sanitizer: DomSanitizer) {}

  transform(url: string): SafeResourceUrl {
    return this.sanitizer.bypassSecurityTrustResourceUrl(url);
  }
}
