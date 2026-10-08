import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BlogService } from '../../_services/blog-service';
import { TagService } from '../../_services/tag-service';
import { BlogDto } from '../../_models/blog';
import { TagDto } from '../../_models/tag';

// bir konudaki (etiketteki) bütün yazılar: /tag/dotnet
// kategori sayfasından farkı: farklı kategorilerdeki ve farklı yazarların yazıları burada bir arada
@Component({
  selector: 'app-tag-blogs',
  standalone: false,
  templateUrl: './tag-blogs.html',
  styleUrl: './tag-blogs.css'
})
export class TagBlogs implements OnInit {
  slug: string;
  tag: TagDto;
  tags: TagDto[] = [];
  blogs: BlogDto[] = [];
  pagedBlogs: BlogDto[] = [];
  // bu konuda yazan farklı yazar sayısı (başlığın altında gösteriyorum)
  authorCount = 0;

  pageSize = 6;
  currentPage = 1;
  loading = true;

  constructor(private route: ActivatedRoute,
              private router: Router,
              private blogService: BlogService,
              private tagService: TagService) { }

  ngOnInit() {
    this.tagService.getAll().subscribe({
      next: result => {
        this.tags = result.data;
        this.tag = this.tags.find(x => x.slug === this.slug);
      }
    });

    // kategori sayfasındaki gibi: aynı component'teyken başka etikete geçilince tekrar yükle
    this.route.paramMap.subscribe(params => {
      this.slug = params.get('slug');
      this.tag = this.tags.find(x => x.slug === this.slug);
      this.getBlogs();
    });

    this.route.queryParamMap.subscribe(params => {
      this.currentPage = Number(params.get('page')) || 1;
      this.setPage();
    });
  }

  getBlogs() {
    this.loading = true;
    this.blogService.getBlogsByTag(this.slug).subscribe({
      next: result => {
        this.blogs = result.data ?? [];
        // Set tekrar eden değerleri tutmuyor, userId'leri Set'e atınca farklı yazar sayısı çıkıyor
        this.authorCount = new Set(this.blogs.map(x => x.userId)).size;
        this.loading = false;
        this.setPage();
      },
      error: () => this.loading = false
    });
  }

  setPage() {
    if (!this.blogs.length) {
      this.pagedBlogs = [];
      return;
    }
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
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }
}
