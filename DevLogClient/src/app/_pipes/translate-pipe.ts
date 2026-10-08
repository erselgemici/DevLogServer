import { Pipe, PipeTransform } from '@angular/core';
import { t } from '../_i18n/i18n';

// html'de {{ 'menu.home' | t }} şeklinde kullanıyorum
@Pipe({
  name: 't',
  standalone: false
})
export class TranslatePipe implements PipeTransform {
  transform(key: string): string {
    return t(key);
  }
}
