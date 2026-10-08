import { Component, OnInit } from '@angular/core';
import { t } from '../../_i18n/i18n';
import { closeOpenModals } from '../../_helpers/modal';
import { SocialDto } from '../../_models/socialDto';
import { SocialService } from '../../_services/social-service';
import { AuthService } from '../../_services/auth-service';
import { SweetalertService } from '../../_services/sweetalert-service';
declare const alertify : any;

@Component({
  selector: 'app-social',
  standalone: false,
  templateUrl: './social.html',
  styleUrl: './social.css'
})
export class Social implements OnInit {
socials:SocialDto[];
newSocial: SocialDto= new SocialDto();
editSocial:any ={};
// backend'den gelen validation hataları, html'de alan adına göre gösteriyorum
errors:any= [];

// sayfa açılınca verileri çekiyorum
ngOnInit(): void {

  this.getSocials();

}


constructor(private socialService : SocialService,
            private authService: AuthService,
            private swal : SweetalertService
){

}

getSocials(){
 this.socialService.getAll().subscribe({
    next: result => this.socials= result.data,
    error: result => alertify.error(t('msg.error'))
  })
}

create(){
  this.errors= {};
  this.socialService.create(this.newSocial).subscribe({
    next: result => this.socials.push(result.data),
    error : result => {
      alertify.error(t('msg.error'));
    console.log(result.error.errors);
      this.errors= result.error.errors;


    },
    complete: () => { alertify.success(t('msg.created'));
      // işlem bitince sayfayı yenilemek yerine modal'ı kapatıp listeyi tekrar çekiyorum
          closeOpenModals();
          this.getSocials();
          // formu temizle, tekrar açınca eski değerler görünmesin
          this.newSocial = new SocialDto();
         this.errors= {};
     }
  })
}




// düzenle butonuna basınca seçilen kaydı modal'a dolduruyor
onSelected(model){
  this.errors= {};
this.editSocial= model;
}


update(){
  this.socialService.update(this.editSocial).subscribe({
   error: result =>{ alertify.error(t('msg.error'));
    this.errors = result.error.errors
   },
   complete: () => {alertify.success(t('msg.updated'));
 // işlem bitince sayfayı yenilemek yerine modal'ı kapatıp listeyi tekrar çekiyorum
          closeOpenModals();
          this.getSocials();

           this.errors= {};

   }
  })
}

async delete(id){

// silmeden önce sweetalert ile onay soruyorum
const isConfirmed = await this.swal.areYouSure();

if(isConfirmed){

this.socialService.delete(id).subscribe({
    error: result => alertify.error(t('msg.error')),
    complete: ()=>{ alertify.success(t('msg.deleted'));
      this.getSocials()
    }

  })

}
else{
  console.log("Delete Reverted")
}

}


}
