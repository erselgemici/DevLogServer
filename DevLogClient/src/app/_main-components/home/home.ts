import {  AfterViewInit, Component, OnInit } from '@angular/core';
import Swiper from 'swiper';
import { Autoplay, Navigation, Pagination } from 'swiper/modules';
import AOS from 'aos';
import { BlogService } from '../../_services/blog-service';
import { BlogDto } from '../../_models/blog';
import { CategoryService } from '../../_services/category-service';
import { CategoryDto } from '../../_models/category';
import { TagService } from '../../_services/tag-service';
import { TagDto } from '../../_models/tag';



@Component({
  selector: 'home',
  standalone: false,
  templateUrl: './home.html',
  styleUrl: './home.css'
})
export class Home implements OnInit, AfterViewInit  {
  swiper: any;
  isMobileMenuOpen = false;
  latestBlogs: BlogDto[];
  categoriesWithBlogs: CategoryDto[];
  tags: TagDto[] = [];

  constructor(private blogService: BlogService,
              private categoryService: CategoryService,
              private tagService: TagService
  ){

  }

// sayfa açılınca verileri çekiyorum
ngOnInit() {

  this.getLatest5Blogs();
  this.getCategoriesWithBlogs();
  this.tagService.getAll().subscribe({ next: result => this.tags = result.data });



    // Initialize AOS
    // scroll animasyonları
    AOS.init({
      duration: 1000,
      easing: 'ease-in-out',
      once: true,
      mirror: false
    });

    // Initialize Swiper
    // slider ayarları (template'ten geldi)
    this.swiper = new Swiper('.init-swiper', {
      modules: [Navigation, Pagination, Autoplay],
      // slaytlar api'den sonradan geldiği için swiper değişiklikleri izlesin
      observer: true,
      observeParents: true,
      loop: true,
      speed: 600,
      autoplay: {
        delay: 5000,
        disableOnInteraction: false
      },
      slidesPerView: 'auto',
      centeredSlides: true,
      pagination: {
        el: '.swiper-pagination',
        type: 'bullets',
        clickable: true
      },
      navigation: {
        nextEl: '.swiper-button-next',
        prevEl: '.swiper-button-prev'
      }
    });

    // Handle scroll top button
    window.addEventListener('scroll', () => {
      const scrollTop = document.querySelector('.scroll-top');
      if (scrollTop) {
        if (window.scrollY > 100) {
          scrollTop.classList.add('active');
        } else {
          scrollTop.classList.remove('active');
        }
      }
    });
  }

  ngAfterViewInit() {
    // Remove preloader after view is initialized

    const preloader = document.querySelector('#preloader');
    if (preloader) {
      preloader.remove();
    }


  }

  toggleMobileMenu() {
    this.isMobileMenuOpen = !this.isMobileMenuOpen;
    const navmenu = document.querySelector('#navmenu');
    if (navmenu) {
      if (this.isMobileMenuOpen) {
        navmenu.classList.add('mobile-nav-active');
      } else {
        navmenu.classList.remove('mobile-nav-active');
      }
    }
  }

  getLatest5Blogs(){
    this.blogService.getLatest5Blogs().subscribe({
      next: result => this.latestBlogs= result.data
    })
  }

  getCategoriesWithBlogs(){
    this.categoryService.getCategories().subscribe({
      next: result => {
        // her kategorinin yazılarını yeniden eskiye sıralıyorum,
        // ana sayfada sadece ilk birkaç tanesi gösteriliyor
        result.data.forEach(category => {
          category.blogs = (category.blogs ?? []).sort((a, b) =>
            new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime());
        });
        this.categoriesWithBlogs = result.data;
      }
    })
  }




}
