import { AfterViewInit, Component, OnInit } from '@angular/core';
import { AuthService } from '../../_services/auth-service';
import { Router } from '@angular/router';
import { Autoplay, Navigation, Pagination } from 'swiper/modules';
import Swiper from 'swiper';
import AOS from 'aos';
import { LanguageService } from '../../_services/language-service';
import { SocialService } from '../../_services/social-service';
import { ContactInfoService } from '../../_services/contact-info-service';
import { CategoryService } from '../../_services/category-service';
import { SocialDto } from '../../_models/socialDto';
import { ContactInfoDto } from '../../_models/contactInfoDto';
import { CategoryDto } from '../../_models/category';

@Component({
  selector: 'main-layout',
  standalone: false,
  templateUrl: './main-layout.html',
  styleUrl: './main-layout.css'
})
export class MainLayout implements OnInit, AfterViewInit {

  constructor(private authService: AuthService,
              public language: LanguageService,
              private socialService: SocialService,
              private contactInfoService: ContactInfoService,
              private categoryService: CategoryService,
              private router: Router){}

  // header'daki arama kutusunun değeri
  searchText = '';

  search() {
    const text = this.searchText.trim();
    if (!text) return;
    this.router.navigate(['/search'], { queryParams: { q: text } });
    this.searchText = '';
  }

  // header ve footer'daki bilgiler artık veritabanından geliyor
  socials: SocialDto[] = [];
  contactInfos: ContactInfoDto[] = [];
  categories: CategoryDto[] = [];
  year = new Date().getFullYear();

  private swiper: Swiper | undefined;
  isMobileMenuOpen = false;

  // sayfa açılınca verileri çekiyorum
  ngOnInit() {
    this.socialService.getAll().subscribe({ next: result => this.socials = result.data });
    this.contactInfoService.getAll().subscribe({ next: result => this.contactInfos = result.data });
    this.categoryService.getCategories().subscribe({ next: result => this.categories = result.data });

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

  scrollToTop(event: Event) {
    event.preventDefault();
    window.scrollTo({
      top: 0,
      behavior: 'smooth'
    });
  }


  getFullName(){
    let decodedToken =  this.authService.decodeToken();

    // kullanıcı adını token'ın içinden okuyorum
    return decodedToken.fullName;
  }


  loggedIn(){
    return this.authService.loggedIn();
  }

}
