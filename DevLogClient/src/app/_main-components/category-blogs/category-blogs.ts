import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BlogService } from '../../_services/blog-service';
import { CategoryService } from '../../_services/category-service';
import { BlogDto } from '../../_models/blog';
import { CategoryDto } from '../../_models/category';

// bir kategorideki bütün yazıları sayfa sayfa gösteren sayfa
// ana sayfada her kategoriden sadece birkaç yazı çıkıyor, "Tümünü gör" buraya yönlendiriyor
@Component({
  selector: 'app-category-blogs',
  standalone: false,
  templateUrl: './category-blogs.html',
  styleUrl: './category-blogs.css'
})
export class CategoryBlogs implements OnInit {
  categoryId: string;
  category: CategoryDto;
  categories: CategoryDto[] = [];

  blogs: BlogDto[] = [];
  pagedBlogs: BlogDto[] = [];

  // her sayfada kaç yazı olacak
  pageSize = 6;
  currentPage = 1;
  loading = true;

  constructor(private route: ActivatedRoute,
              private router: Router,
              private blogService: BlogService,
              private categoryService: CategoryService) { }

  ngOnInit() {
    // üstteki kategori listesi için kategorileri bir kere çekiyorum
    this.categoryService.getCategories().subscribe({
      next: result => {
        this.categories = result.data;
        this.setCategory();
      }
    });

    // NOT: route.snapshot.params sadece ilk açılıştaki değeri veriyor.
    // aynı sayfadayken başka kategoriye geçilince Angular component'i yeniden oluşturmuyor,
    // o yüzden snapshot yerine paramMap'e subscribe oldum, id her değiştiğinde burası tekrar çalışıyor
    this.route.paramMap.subscribe(params => {
      this.categoryId = params.get('id');
      this.setCategory();
      this.getBlogs();
    });

    // sayfa numarasını url'de tutuyorum (?page=2), geri tuşu da çalışsın diye
    this.route.queryParamMap.subscribe(params => {
      this.currentPage = Number(params.get('page')) || 1;
      this.setPage();
    });
  }

  setCategory() {
    this.category = this.categories.find(x => x.id === this.categoryId);
  }

  getBlogs() {
    this.loading = true;
    this.blogService.getBlogsByCategoryId(this.categoryId).subscribe({
      next: result => {
        // en yeni yazı en başta olsun
        this.blogs = (result.data ?? []).sort((a, b) =>
          new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime());
        this.loading = false;
        this.setPage();
      },
      error: () => this.loading = false
    });
  }

  // listeyi sayfa numarasına göre kesiyorum (client tarafında sayfalama)
  // örnek: pageSize 6, sayfa 2 -> slice(6, 12) yani 7. ve 12. yazılar arası
  // NOT: yazı sayısı çok artarsa (binlerce) sayfalamayı backend'de Skip/Take ile yapmak gerekir
  setPage() {
    // yazılar henüz gelmediyse url'deki sayfa numarasını bozmuyorum
    if (!this.blogs.length) {
      this.pagedBlogs = [];
      return;
    }

    // url'ye olmayan bir sayfa yazılırsa son sayfaya düşsün
    const totalPages = Math.ceil(this.blogs.length / this.pageSize);
    if (this.currentPage > totalPages) this.currentPage = totalPages;

    const start = (this.currentPage - 1) * this.pageSize;
    this.pagedBlogs = this.blogs.slice(start, start + this.pageSize);
  }

  onPageChange(page: number) {
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: { page: page === 1 ? null : page },
      queryParamsHandling: 'merge'
    });
    // yeni sayfaya geçince en üste kaydır
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }
}
