import { LOCALE_ID, NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { registerLocaleData } from '@angular/common';
import localeTr from '@angular/common/locales/tr';
import { TranslatePipe } from './_pipes/translate-pipe';
import { ContentPipe } from './_pipes/content-pipe';
import { ExcerptPipe } from './_pipes/excerpt-pipe';
import { SafeUrlPipe } from './_pipes/safe-url-pipe';
import { getLanguage } from './_i18n/i18n';

// date pipe Türkçe tarih gösterebilsin diye Türkçe locale verisini kaydediyorum
registerLocaleData(localeTr);
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing-module';
import { App } from './app';
import { AdminLayout } from './_layouts/admin-layout/admin-layout';
import { MainLayout } from './_layouts/main-layout/main-layout';
import { Home } from './_main-components/home/home';
import { Category } from './_admin-components/category/category';
import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { Blog } from './_admin-components/blog/blog';
import { Login } from './_main-components/login/login';
import { JwtHelperService, JwtModule } from '@auth0/angular-jwt';
import { AuthGuard } from './_guards/auth-guard';
import { Blogdetails } from './_main-components/blogdetails/blogdetails';
import { CommentForm } from './_main-components/comment-form/comment-form';
import { ContactMain } from './_main-components/contact-main/contact-main';
import { Comment } from './_admin-components/comment/comment';
import { ContactInfo } from './_admin-components/contact-info/contact-info';
import { TokenInterceptor } from './_interceptors/token-interceptor';
import { Message } from './_admin-components/message/message';
import { Social } from './_admin-components/social/social';
import { SendMessage } from './_main-components/send-message/send-message';
import { Users } from './_admin-components/users/users';

import { CategoryBlogs } from './_main-components/category-blogs/category-blogs';
import { Pagination } from './_main-components/pagination/pagination';
import { PostCard } from './_main-components/post-card/post-card';
import { TagBlogs } from './_main-components/tag-blogs/tag-blogs';
import { Search } from './_main-components/search/search';
import { Dashboard } from './_admin-components/dashboard/dashboard';

@NgModule({
  declarations: [
    App,
    AdminLayout,
    MainLayout,
    Home,
    Category,
    Blog,
    Login,
    Blogdetails,
    CommentForm,
    ContactMain,
    Comment,
    ContactInfo,
    Message,
    Social,
    SendMessage,
    Users,
    TranslatePipe,
    SafeUrlPipe,
    CategoryBlogs,
    Pagination,
    ContentPipe,
    ExcerptPipe,
    PostCard,
    TagBlogs,
    Search,
    Dashboard
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule

  ],
  providers: [
    provideBrowserGlobalErrorListeners(),
    // tarih formatı seçili dile göre (ör: 6 Eki 2026 / Oct 6, 2026)
    { provide: LOCALE_ID, useFactory: () => getLanguage() === 'en' ? 'en-US' : 'tr' },
    provideHttpClient(withInterceptorsFromDi()),
    // token interceptor'ı burada kaydettim
    {provide: HTTP_INTERCEPTORS,useClass: TokenInterceptor, multi: true}

  ],
  bootstrap: [App]
})
export class AppModule { }
