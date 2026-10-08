import { EN } from './en';
import { TR } from './tr';

// sitenin adı, değiştirmek için sadece burayı düzenlemek yeterli
export const SITE_NAME = 'DevLog';

// yeni bir dil eklemek için aynı anahtarlarla bir dosya oluşturup buraya eklemek yeterli
export const TRANSLATIONS: { [lang: string]: { [key: string]: string } } = {
  tr: { 'site.name': SITE_NAME, ...TR },
  en: { 'site.name': SITE_NAME, ...EN }
};
