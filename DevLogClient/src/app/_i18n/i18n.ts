import { TRANSLATIONS } from './translations';

// dil tercihini localStorage'da tutuyorum, varsayılan dil Türkçe
export const SUPPORTED_LANGUAGES = ['tr', 'en'];
export const DEFAULT_LANGUAGE = 'tr';
const STORAGE_KEY = 'lang';

export function getLanguage(): string {
  try {
    const lang = localStorage.getItem(STORAGE_KEY);
    return lang && SUPPORTED_LANGUAGES.includes(lang) ? lang : DEFAULT_LANGUAGE;
  } catch {
    return DEFAULT_LANGUAGE;
  }
}

export function setLanguage(lang: string) {
  if (!SUPPORTED_LANGUAGES.includes(lang)) return;
  localStorage.setItem(STORAGE_KEY, lang);
}

// component'lerin ts tarafında (alertify mesajları vb.) kullanmak için
export function t(key: string): string {
  const lang = getLanguage();
  return TRANSLATIONS[lang]?.[key] ?? TRANSLATIONS[DEFAULT_LANGUAGE]?.[key] ?? key;
}
