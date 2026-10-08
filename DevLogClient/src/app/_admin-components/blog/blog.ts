import { Component, OnInit, signal } from '@angular/core';
import { t } from '../../_i18n/i18n';
import { closeOpenModals } from '../../_helpers/modal';
import { BlogService } from '../../_services/blog-service';
import { SweetalertService } from '../../_services/sweetalert-service';
import { BlogDto } from '../../_models/blog';
import  {Observable} from 'rxjs';
import { CategoryService } from '../../_services/category-service';
import { CategoryDto } from '../../_models/category';
import { AuthService } from '../../_services/auth-service';
import { TagService } from '../../_services/tag-service';
import { TagDto } from '../../_models/tag';
declare const alertify:any;

@Component({
  selector: 'app-blog',
  standalone: false,
  templateUrl: './blog.html',
  styleUrl: './blog.css'
})
export class Blog implements OnInit {



constructor(private blogService:BlogService,
            private swal: SweetalertService,
            private categoryService: CategoryService,
            private authService: AuthService,
            private tagService: TagService
){}

// formdaki etiket seçenekleri
tags: TagDto[] = [];

blogs:BlogDto[];

// NOT: 87 yazı / 300+ yorum tek tabloda çizilince sayfa ağırlaşıyordu (her satırda resim, butonlar).
// listeyi bölüp sadece o sayfadaki kayıtları çiziyorum
page = 1;
pageSize = 10;
loading = true;

// getter: template'te pagedBlogs değişken gibi kullanılıyor ama her seferinde hesaplanıyor
get pagedBlogs() {
  const start = (this.page - 1) * this.pageSize;
  return (this.blogs ?? []).slice(start, start + this.pageSize);
}

categories: any= [];
newBlog: BlogDto= new BlogDto();
editBlog:any ={};
// backend'den gelen validation hataları, html'de alan adına göre gösteriyorum
errors:any= [];

// sayfa açılınca verileri çekiyorum
ngOnInit(): void {

  this.getBlogs();
  this.getCategories();
  this.tagService.getAll().subscribe({ next: result => this.tags = result.data });
}

// checkbox işaretli mi? (seçilen etiketler tagIds dizisinde tutuluyor)
hasTag(blog: any, tagId: string) {
  return blog.tagIds?.includes(tagId);
}

// checkbox'a tıklanınca listede varsa çıkar, yoksa ekle
toggleTag(blog: any, tagId: string) {
  blog.tagIds ??= [];
  blog.tagIds = this.hasTag(blog, tagId)
    ? blog.tagIds.filter(x => x !== tagId)
    : [...blog.tagIds, tagId];
}

getBlogs(){
 this.blogService.getAll().subscribe({
    // yazar sadece kendi yazılarını görüyor, admin hepsini
    next: result => {
      this.blogs = this.authService.isAdmin()
        ? result.data
        : result.data.filter(b => b.userId === this.authService.getUserId());
      this.loading = false;
    },
    error: result => {
      this.loading = false;
      alertify.error(t('msg.error'));
    }
  })
}

create(){
  this.errors= {};

  // blogu yazan kişi olarak giriş yapan kullanıcının id'sini token'dan alıyorum
  this.newBlog.userId = this.authService.getUserId();
  this.blogService.create(this.newBlog).subscribe({
    next: result => this.blogs.push(result.data),
    error : result => {
      alertify.error(t('msg.error'));
    console.log(result.error.errors);
      this.errors= result.error.errors;


    },
    complete: () => { alertify.success(t('msg.created'));
      // işlem bitince sayfayı yenilemek yerine modal'ı kapatıp listeyi tekrar çekiyorum
          closeOpenModals();
          this.getBlogs();
          // formu temizle, tekrar açınca eski değerler görünmesin
          this.newBlog = new BlogDto();
         this.errors= {};
     }
  })
}

getCategories(){
this.categoryService.getCategories().subscribe({
  next: result => this.categories= result.data
})
}


// düzenle butonuna basınca seçilen kaydı modal'a dolduruyor
// listede içeriğin tamamı gelmediği için düzenlerken blogu id ile tekrar çekiyorum
// (içerik ve İngilizce alanlar sadece detay endpoint'inde var)
// { ...blog } = spread operatörü, nesnenin kopyasını alıyor. istek gelene kadar modal boş kalmasın diye
// listedeki bilgileri geçici olarak koyuyorum, kopya olduğu için tablodaki satır değişmiyor
onSelected(blog){
  this.errors= {};
  this.editBlog = { ...blog };
  this.blogService.getBlogById(blog.id).subscribe({
    next: result => {
      this.editBlog = result.data;
      // detayda etiketler nesne olarak geliyor, formda sadece id'leri tutuyorum
      this.editBlog.tagIds = (result.data.tags ?? []).map(x => x.id);
    },
    error: () => alertify.error(t('msg.error'))
  })
}


update(){
  this.editBlog.userId = this.authService.getUserId();
  this.blogService.update(this.editBlog).subscribe({
   error: result =>{ alertify.error(t('msg.error'));
    this.errors = result.error.errors
   },
   complete: () => {alertify.success(t('msg.updated'));
 // işlem bitince sayfayı yenilemek yerine modal'ı kapatıp listeyi tekrar çekiyorum
          closeOpenModals();
          this.getBlogs();

           this.errors= {};

   }
  })
}

async delete(id){

// silmeden önce sweetalert ile onay soruyorum
const isConfirmed = await this.swal.areYouSure();

if(isConfirmed){

this.blogService.delete(id).subscribe({
    error: result => alertify.error(t('msg.error')),
    complete: ()=>{ alertify.success(t('msg.deleted'));
      this.getBlogs()
    }

  })

}
else{
  console.log("Delete Reverted")
}

}



}
