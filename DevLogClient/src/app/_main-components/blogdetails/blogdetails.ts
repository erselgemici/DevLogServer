import { Component, ElementRef, HostListener } from '@angular/core';
import { BlogService } from '../../_services/blog-service';
import { ActivatedRoute } from '@angular/router';
import { BlogDto } from '../../_models/blog';
import { CommentDto } from '../../_models/commentDto';
import { t } from '../../_i18n/i18n';
import hljs from 'highlight.js/lib/core';
import csharp from 'highlight.js/lib/languages/csharp';
import typescript from 'highlight.js/lib/languages/typescript';
import javascript from 'highlight.js/lib/languages/javascript';
import xml from 'highlight.js/lib/languages/xml';
import sql from 'highlight.js/lib/languages/sql';
import bash from 'highlight.js/lib/languages/bash';
import python from 'highlight.js/lib/languages/python';
import yaml from 'highlight.js/lib/languages/yaml';
import json from 'highlight.js/lib/languages/json';
import java from 'highlight.js/lib/languages/java';
import go from 'highlight.js/lib/languages/go';
import rust from 'highlight.js/lib/languages/rust';
import kotlin from 'highlight.js/lib/languages/kotlin';
import swift from 'highlight.js/lib/languages/swift';
import dart from 'highlight.js/lib/languages/dart';
import css from 'highlight.js/lib/languages/css';
import plaintext from 'highlight.js/lib/languages/plaintext';

// NOT: highlight.js'in tamamını import etseydim 190'dan fazla dil bundle'a girecekti (~1 MB).
// "core" + sadece yazılarda kullandığım dilleri kaydedince bundle çok daha küçük kalıyor
hljs.registerLanguage('csharp', csharp);
hljs.registerLanguage('typescript', typescript);
hljs.registerLanguage('javascript', javascript);
hljs.registerLanguage('xml', xml);
hljs.registerLanguage('sql', sql);
hljs.registerLanguage('bash', bash);
hljs.registerLanguage('python', python);
hljs.registerLanguage('yaml', yaml);
hljs.registerLanguage('json', json);
hljs.registerLanguage('java', java);
hljs.registerLanguage('go', go);
hljs.registerLanguage('rust', rust);
hljs.registerLanguage('kotlin', kotlin);
hljs.registerLanguage('swift', swift);
hljs.registerLanguage('dart', dart);
hljs.registerLanguage('css', css);
hljs.registerLanguage('plaintext', plaintext);

@Component({
  selector: 'app-blogdetails',
  standalone: false,
  templateUrl: './blogdetails.html',
  styleUrl: './blogdetails.css'
})
export class Blogdetails {
blog:BlogDto;
latestBlogs: BlogDto[];
// aynı etiketlere sahip diğer yazılar (farklı yazarlardan)
relatedBlogs: BlogDto[] = [];
newComment : CommentDto = new CommentDto();

// içindekiler: yazıdaki ## başlıklarından oluşuyor
toc: { id: string, text: string }[] = [];
activeTocId = '';
// sayfanın üstündeki okuma çubuğu (0-100)
progress = 0;


constructor(private blogService: BlogService,
            private route: ActivatedRoute,
            private elementRef: ElementRef
){
  // sidebar'dan başka bir yazıya tıklanınca component yeniden oluşmuyor,
  // o yüzden url'deki id değiştikçe blogu tekrar çekiyorum
  this.route.paramMap.subscribe(() => {
    this.getBlogById();
    this.getRelatedBlogs();
    window.scrollTo(0, 0);
  });
  this.getLatestBlogs();
}



getBlogById(){
    // url'deki id ile blogu getiriyorum
    this.blogService.getBlogById(this.route.snapshot.params["id"]).subscribe({
      next: result => {
        this.blog= result.data;
        // NOT: içerik [innerHTML] ile geliyor, Angular onu ekrana bu callback bittikten sonra çiziyor.
        // setTimeout(0) ile "çizim bitince çalış" diyorum, yoksa querySelector boş liste döner
        setTimeout(() => this.enhanceContent());
      }
    })
}

getRelatedBlogs(){
  this.blogService.getRelatedBlogs(this.route.snapshot.params["id"]).subscribe({
    next: result => this.relatedBlogs = result.data
  })
}

getLatestBlogs(){
  this.blogService.getLatest5Blogs().subscribe({
    next: result => this.latestBlogs= result.data
  })
}

postComment(){
  // yorumun hangi bloga ait olduğunu url'deki id'den alıyorum
  this.newComment.blogId = this.route.snapshot.params["id"];

}

// cevap yazının yazarından mı geldi? (yorumun altında "Yazar" etiketi göstermek için)
isAuthor(firstName: string, lastName: string) {
  return this.blog?.user && firstName === this.blog.user.firstName && lastName === this.blog.user.lastName;
}

// içerik ekrana çizildikten sonra: başlıklara id ver, içindekileri oluştur,
// kodları renklendir ve kopyala butonu ekle
// NOT: bunları content pipe'ta yapmadım çünkü Angular innerHTML'deki id ve button'ları
// güvenlik için (sanitize) silebiliyor. DOM'a sonradan elle eklemek daha garanti
private enhanceContent(){
  const content: HTMLElement = this.elementRef.nativeElement.querySelector('.blog-content');
  if (!content) return;

  // 1) içindekiler
  const headings = Array.from(content.querySelectorAll('h3')) as HTMLElement[];
  this.toc = headings.map((h, i) => {
    h.id = 'section-' + (i + 1);
    return { id: h.id, text: h.textContent ?? '' };
  });

  // 2) kod blokları
  content.querySelectorAll('.code-block').forEach((block: HTMLElement) => {
    const code = block.querySelector('code') as HTMLElement;
    const label = block.querySelector('.code-lang')?.textContent?.trim().toLowerCase() ?? '';
    // yazılarda html/text diye geçen dillerin highlight.js'teki adları farklı
    const language = label === 'html' ? 'xml' : label === 'text' ? 'plaintext' : label;

    if (code && hljs.getLanguage(language)) {
      code.classList.add('language-' + language);
      hljs.highlightElement(code);
    }

    // kopyala butonu (sayfa yeniden açılırsa iki tane olmasın)
    if (!block.querySelector('.copy-btn')) {
      const button = document.createElement('button');
      button.type = 'button';
      button.className = 'copy-btn';
      button.innerHTML = '<i class="bi bi-clipboard"></i> ' + t('blog.copy');
      button.addEventListener('click', () => this.copyCode(code, button));
      block.appendChild(button);
    }
  });
}

// navigator.clipboard sadece https veya localhost'ta çalışıyor (güvenli bağlam)
private copyCode(code: HTMLElement, button: HTMLButtonElement){
  navigator.clipboard.writeText(code.innerText).then(() => {
    button.innerHTML = '<i class="bi bi-check2"></i> ' + t('blog.copied');
    button.classList.add('copied');
    // 2 saniye sonra eski haline dönsün
    setTimeout(() => {
      button.innerHTML = '<i class="bi bi-clipboard"></i> ' + t('blog.copy');
      button.classList.remove('copied');
    }, 2000);
  });
}

// içindekilerden bir başlığa tıklayınca oraya kaydır
scrollTo(id: string){
  document.getElementById(id)?.scrollIntoView({ behavior: 'smooth', block: 'start' });
}

// sayfa kaydırıldıkça okuma çubuğunu ve içindekilerde aktif başlığı güncelliyorum
// @HostListener: component'e window olaylarını dinleme imkanı veriyor (addEventListener'ın Angular hali)
@HostListener('window:scroll')
onScroll(){
  const article = this.elementRef.nativeElement.querySelector('.blog-content') as HTMLElement;
  if (!article) return;

  // yazının başından sonuna kadar ne kadar okunduğu
  const start = article.getBoundingClientRect().top + window.scrollY - 120;
  const end = start + article.offsetHeight - window.innerHeight + 200;
  const ratio = (window.scrollY - start) / (end - start);
  this.progress = Math.min(100, Math.max(0, ratio * 100));

  // ekranın üst kısmını geçmiş son başlık aktif başlık
  let active = '';
  for (const item of this.toc) {
    const el = document.getElementById(item.id);
    if (el && el.getBoundingClientRect().top < 160) active = item.id;
  }
  this.activeTocId = active;
}

}
