import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../_services/auth-service';
import { LanguageService } from '../../_services/language-service';

@Component({
  selector: 'admin-layout',
  standalone: false,
  templateUrl: './admin-layout.html',
  styleUrl: './admin-layout.css'
})
export class AdminLayout implements OnInit {

constructor(private authService: AuthService,
            public language: LanguageService){}



// NOT: template'te getUserName(), isAdmin() gibi metotlar her değişiklik algılamada (change detection)
// tekrar tekrar çalışıyor. önceden her seferinde token'ı baştan çözüyordum (menüde 7 kere isAdmin var),
// şimdi token'ı sayfa açılınca bir kere çözüp sonucu değişkenlerde tutuyorum
private userName = '';
private userImage = '';
private admin = false;

ngOnInit() {
  const decodedToken = this.authService.decodeToken();
  // kullanıcı adını token'ın içinden okuyorum
  this.userName = decodedToken?.name ?? '';
  // token içindeki profil resmi, yoksa varsayılan avatar
  this.userImage = decodedToken?.imageUrl || 'assets/img/blog/blog-author.jpg';
  // menüde admin'e özel sayfaları göstermek için
  this.admin = this.authService.isAdmin();
}

getUserName(){
  return this.userName;
}

isAdmin(){
  return this.admin;
}

getUserImage(){
  return this.userImage;
}

logout(){

  this.authService.logout();
}


}
