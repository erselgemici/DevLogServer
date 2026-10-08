import { Component, OnInit } from '@angular/core';
import { t } from '../../_i18n/i18n';
import { closeOpenModals } from '../../_helpers/modal';
import { MessageDto } from '../../_models/messageDto';
import { MessageService } from '../../_services/message-service';
import { AuthService } from '../../_services/auth-service';
import { SweetalertService } from '../../_services/sweetalert-service';
declare const alertify : any;

@Component({
  selector: 'app-message',
  standalone: false,
  templateUrl: './message.html',
  styleUrl: './message.css'
})
export class Message implements OnInit {

  messages:MessageDto[];
  unreadMessages: MessageDto[];
  readMessages: MessageDto[];
  newMessage: MessageDto= new MessageDto();
  editMessage:any ={};
  // backend'den gelen validation hataları, html'de alan adına göre gösteriyorum
  errors:any= [];

  // sayfa açılınca verileri çekiyorum
  ngOnInit(): void {

    this.getUnReadMessages();
    this.getReadMessages();


  }


  constructor(private messageService : MessageService,
              private authService: AuthService,
              private swal : SweetalertService
  ){

  }

  getmessages(){
   this.messageService.getAll().subscribe({
      next: result => this.messages= result.data,
      error: result => alertify.error(t('msg.error'))
    })
  }

  getUnReadMessages(){
   this.messageService.getUnreadMessages().subscribe({
      next: result => this.unreadMessages= result.data,
      error: result => alertify.error(t('msg.error'))
    })
  }

  getReadMessages(){
   this.messageService.getReadMessages().subscribe({
      next: result => this.readMessages= result.data,
      error: result => alertify.error(t('msg.error'))
    })
  }


  create(){
    this.errors= {};
    this.messageService.create(this.newMessage).subscribe({
      next: result => this.messages.push(result.data),
      error : result => {
        alertify.error(t('msg.error'));
      console.log(result.error.errors);
        this.errors= result.error.errors;


      },
      complete: () => { alertify.success(t('msg.created'));
        // işlem bitince sayfayı yenilemek yerine modal'ı kapatıp listeyi tekrar çekiyorum
          closeOpenModals();
          this.getUnReadMessages();
          this.getReadMessages();
          // formu temizle, tekrar açınca eski değerler görünmesin
          this.newMessage = new MessageDto();
           this.errors= {};
       }
    })
  }




  // düzenle butonuna basınca seçilen kaydı modal'a dolduruyor
  onSelected(model){

  this.editMessage= model;

  // mesaj açılınca okundu olarak işaretleniyor
  this.editMessage.isRead = true;

  this.messageService.update(this.editMessage).subscribe({
    error: result => console.log(result.error),
    complete: ()=> {this.getReadMessages();
      this.getUnReadMessages();
    }
  })
  }


  update(){
    this.messageService.update(this.editMessage).subscribe({
     error: result =>{ alertify.error(t('msg.error'));
      this.errors = result.error.errors
     },
     complete: () => {alertify.success(t('msg.updated'));
   // işlem bitince sayfayı yenilemek yerine modal'ı kapatıp listeyi tekrar çekiyorum
          closeOpenModals();
          this.getUnReadMessages();
          this.getReadMessages();

             this.errors= {};

     }
    })
  }

  async delete(id){

  // silmeden önce sweetalert ile onay soruyorum
  const isConfirmed = await this.swal.areYouSure();

  if(isConfirmed){

  this.messageService.delete(id).subscribe({
      error: result => alertify.error(t('msg.error')),
      complete: ()=>{ alertify.success(t('msg.deleted'));
        this.getReadMessages();
        this.getUnReadMessages();
      }

    })

  }
  else{
    console.log("Delete Reverted")
  }

  }

}
