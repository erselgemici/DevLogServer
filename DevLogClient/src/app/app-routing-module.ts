import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainLayout } from './_layouts/main-layout/main-layout';
import { Home } from './_main-components/home/home';
import { AdminLayout } from './_layouts/admin-layout/admin-layout';
import { Category } from './_admin-components/category/category';
import { Blog } from './_admin-components/blog/blog';
import { Login } from './_main-components/login/login';
import { AuthGuard } from './_guards/auth-guard';
import { Blogdetails } from './_main-components/blogdetails/blogdetails';
import { ContactMain } from './_main-components/contact-main/contact-main';
import { Comment } from './_admin-components/comment/comment';
import { ContactInfo } from './_admin-components/contact-info/contact-info';
import { Message } from './_admin-components/message/message';
import { Social } from './_admin-components/social/social';
import { Users } from './_admin-components/users/users';
import { AdminGuard } from './_guards/admin-guard';
import { CategoryBlogs } from './_main-components/category-blogs/category-blogs';
import { TagBlogs } from './_main-components/tag-blogs/tag-blogs';
import { Search } from './_main-components/search/search';
import { Dashboard } from './_admin-components/dashboard/dashboard';


const routes: Routes = [

//Main Routes

// site tarafı main layout içinde açılıyor
{ path:'', component: MainLayout,

  children: [
    {path:'', component:Home},
    {path:'login',component:Login},
    {path:'blogdetails/:id',component:Blogdetails},
    // kategori sayfası, yazılar sayfa sayfa listeleniyor
    {path:'category/:id',component:CategoryBlogs},
    // bir konudaki (etiketteki) yazılar
    {path:'tag/:slug',component:TagBlogs},
    {path:'search',component:Search},
    {path:'contact',component:ContactMain}
  ]
},



//Admin Routes    http://localhost:4200/admin/category

{path:'admin',
  // admin tarafı ayrı layout, guard ile korunuyor
  component:AdminLayout,
  canActivate:[AuthGuard],
  children:[
    // panel açılınca genel bakış (dashboard) ekranı, admin ve yazar görebiliyor
    {path:'', component:Dashboard},
    // kullanıcı ve rol yönetimi sadece admin
    {path:'users', component:Users, canActivate:[AuthGuard, AdminGuard]},
    {path:'category',
      component:Category,
    canActivate:[AuthGuard, AdminGuard]},
    {path:'blog',
       component:Blog,
      canActivate:[AuthGuard]},
       {path:'comment',
       component:Comment,
      canActivate:[AuthGuard, AdminGuard]},
      {path:'contactinfo',
       component:ContactInfo,
      canActivate:[AuthGuard, AdminGuard]},
       {path:'message',
       component:Message,
      canActivate:[AuthGuard, AdminGuard]},
       {path:'social',
       component:Social,
      canActivate:[AuthGuard, AdminGuard]}

  ]
}


];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
