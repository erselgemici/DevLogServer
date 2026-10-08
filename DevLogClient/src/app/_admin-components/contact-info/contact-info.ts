import { Component, OnInit } from '@angular/core';
import { t } from '../../_i18n/i18n';
import { closeOpenModals } from '../../_helpers/modal';
import { ContactInfoDto } from '../../_models/contactInfoDto';
import { ContactInfoService } from '../../_services/contact-info-service';
import { AuthService } from '../../_services/auth-service';
import { SweetalertService } from '../../_services/sweetalert-service';
declare const alertify :any;

@Component({
  selector: 'app-contact-info',
  standalone: false,
  templateUrl: './contact-info.html',
  styleUrl: './contact-info.css'
})
export class ContactInfo implements OnInit {
contactInfos:ContactInfoDto[];
newContactInfo: ContactInfoDto= new ContactInfoDto();
editContactInfo:any ={};
// backend'den gelen validation hataları, html'de alan adına göre gösteriyorum
errors:any= [];

// sayfa açılınca verileri çekiyorum
ngOnInit(): void {

  this.getcontactInfos();

}


constructor(private contactInfoService : ContactInfoService,
            private authService: AuthService,
            private swal : SweetalertService
){

}

getcontactInfos(){
 this.contactInfoService.getAll().subscribe({
    next: result => this.contactInfos= result.data,
    error: result => alertify.error(t('msg.error'))
  })
}

create(){
  this.errors= {};
  this.contactInfoService.create(this.newContactInfo).subscribe({
    next: result => this.contactInfos.push(result.data),
    error : result => {
      alertify.error(t('msg.error'));
    console.log(result.error.errors);
      this.errors= result.error.errors;


    },
    complete: () => { alertify.success(t('msg.created'));
      // işlem bitince sayfayı yenilemek yerine modal'ı kapatıp listeyi tekrar çekiyorum
          closeOpenModals();
          this.getcontactInfos();
          // formu temizle, tekrar açınca eski değerler görünmesin
          this.newContactInfo = new ContactInfoDto();
         this.errors= {};
     }
  })
}




// düzenle butonuna basınca seçilen kaydı modal'a dolduruyor
onSelected(model){
  this.errors= {};
this.editContactInfo= model;
}


update(){
  this.contactInfoService.update(this.editContactInfo).subscribe({
   error: result =>{ alertify.error(t('msg.error'));
    this.errors = result.error.errors
   },
   complete: () => {alertify.success(t('msg.updated'));
 // işlem bitince sayfayı yenilemek yerine modal'ı kapatıp listeyi tekrar çekiyorum
          closeOpenModals();
          this.getcontactInfos();

           this.errors= {};

   }
  })
}

async delete(id){

// silmeden önce sweetalert ile onay soruyorum
const isConfirmed = await this.swal.areYouSure();

if(isConfirmed){

this.contactInfoService.delete(id).subscribe({
    error: result => alertify.error(t('msg.error')),
    complete: ()=>{ alertify.success(t('msg.deleted'));
      this.getcontactInfos()
    }

  })

}
else{
  console.log("Delete Reverted")
}

}



}
