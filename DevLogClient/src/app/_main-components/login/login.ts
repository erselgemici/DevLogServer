import { Component } from '@angular/core';
import { t } from '../../_i18n/i18n';
import { AuthService } from '../../_services/auth-service';
import { LoginDto } from '../../_models/loginDto';
import { Router } from '@angular/router';
declare const alertify:any;

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {

constructor(private authService:AuthService,
  private router: Router
){}

loginDto: LoginDto = new LoginDto()
year = new Date().getFullYear();
token :any;
decodedToken: any;


login(){
this.authService.login(this.loginDto).subscribe({
  next: result => {this.token= result.data.token;
    // token'ı localStorage'a kaydediyorum, interceptor her istekte buradan okuyor
    localStorage.setItem("token",result.data.token);

          alertify.success(t('msg.loginSuccess'));
          // blog sayfası hem admin hem yazar için açık
          this.router.navigate(["/admin/blog"])


  },
  error: result => {
    console.log(result.error.errors);
    alertify.error(t('msg.loginFailed'))
  }
})

}


decodeToken(){
let decodedToken =  this.authService.decodeToken();
this.decodedToken= decodedToken;
}


}
