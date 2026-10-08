import { Pipe, PipeTransform } from '@angular/core';

// blog içeriğini html'e çeviren pipe
// içerikte basit bir markdown kullanıyorum:
//   ## Başlık      -> ara başlık
//   - madde        -> liste
//   ```dil ... ``` -> kod bloğu
//   `kod`          -> satır içi kod
//   **kalın**      -> kalın yazı
// NOT (XSS): [innerHTML] ile html basıyorum, içerikte <script> gibi bir şey olursa tehlikeli.
// o yüzden önce bütün metni escape ediyorum (< yerine &lt; vs), sonra sadece kendi etiketlerimi ekliyorum.
// Angular da innerHTML'i ayrıca sanitize ediyor ama ben yine de güvenmeyip kendim de escape ettim
// pipe = template'te değeri dönüştüren küçük sınıf, kullanım: [innerHTML]="blog.description | content"
@Pipe({
  name: 'content',
  standalone: false
})
export class ContentPipe implements PipeTransform {

  transform(text: string): string {
    if (!text) return '';

    const lines = text.replace(/\r\n/g, '\n').split('\n');
    const html: string[] = [];
    let paragraph: string[] = [];
    let list: string[] = [];
    let code: string[] | null = null;
    let codeLang = '';

    const flushParagraph = () => {
      if (paragraph.length) {
        html.push(`<p>${this.inline(paragraph.join(' '))}</p>`);
        paragraph = [];
      }
    };
    const flushList = () => {
      if (list.length) {
        html.push('<ul>' + list.map(x => `<li>${this.inline(x)}</li>`).join('') + '</ul>');
        list = [];
      }
    };

    for (const line of lines) {
      // metni satır satır geziyorum, satırın başına bakıp ne olduğuna karar veriyorum
      // code null değilse ``` açılmış demek, kapanana kadar satırları olduğu gibi topluyorum
      if (code !== null) {
        if (line.trim().startsWith('```')) {
          const label = codeLang ? `<span class="code-lang">${this.escape(codeLang)}</span>` : '';
          html.push(`<div class="code-block">${label}<pre><code>${this.escape(code.join('\n'))}</code></pre></div>`);
          code = null;
        } else {
          code.push(line);
        }
        continue;
      }

      if (line.trim().startsWith('```')) {
        flushParagraph(); flushList();
        code = [];
        codeLang = line.trim().substring(3).trim();
      } else if (line.startsWith('## ')) {
        flushParagraph(); flushList();
        html.push(`<h3>${this.inline(line.substring(3))}</h3>`);
      } else if (line.startsWith('- ')) {
        flushParagraph();
        list.push(line.substring(2));
      } else if (line.trim() === '') {
        flushParagraph(); flushList();
      } else {
        flushList();
        paragraph.push(line.trim());
      }
    }

    // kapanmamış kod bloğu kalırsa yine de gösteriyorum
    if (code !== null) {
      html.push(`<div class="code-block"><pre><code>${this.escape(code.join('\n'))}</code></pre></div>`);
    }
    flushParagraph(); flushList();

    return html.join('\n');
  }

  private inline(text: string): string {
    return this.escape(text)
      .replace(/`([^`]+)`/g, '<code>$1</code>')
      .replace(/\*\*([^*]+)\*\*/g, '<strong>$1</strong>');
  }

  private escape(text: string): string {
    return text
      .replace(/&/g, '&amp;')
      .replace(/</g, '&lt;')
      .replace(/>/g, '&gt;')
      .replace(/"/g, '&quot;');
  }
}
