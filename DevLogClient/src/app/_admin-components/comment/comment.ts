import { Component } from '@angular/core';
import { t } from '../../_i18n/i18n';
import { CommentService } from '../../_services/comment-service';
import { SweetalertService } from '../../_services/sweetalert-service';
import { CommentDto } from '../../_models/commentDto';
declare const alertify:any;

@Component({
  selector: 'app-comment',
  standalone: false,
  templateUrl: './comment.html',
  styleUrl: './comment.css'
})
export class Comment {

constructor(private commentService: CommentService,
            private swal: SweetalertService
){
  this.getcomments();
}


comments: CommentDto[];

// NOT: 87 yazı / 300+ yorum tek tabloda çizilince sayfa ağırlaşıyordu (her satırda resim, butonlar).
// listeyi bölüp sadece o sayfadaki kayıtları çiziyorum
page = 1;
pageSize = 10;
loading = true;

// getter: template'te pagedComments değişken gibi kullanılıyor ama her seferinde hesaplanıyor
get pagedComments() {
  const start = (this.page - 1) * this.pageSize;
  return (this.comments ?? []).slice(start, start + this.pageSize);
}

newComment: CommentDto = new CommentDto();
editComment:any = {};
// backend'den gelen validation hataları, html'de alan adına göre gösteriyorum
errors: any=[];


getcomments(){
  this.commentService.getAll().subscribe({
    next: result=> {
      this.comments= result.data;
      this.loading = false;
    },
    // önceden hata sadece console'a yazılıyordu, sayfa boş kalınca neden boş anlaşılmıyordu
    error: result=> {
      this.loading = false;
      alertify.error(t('msg.error'));
    }
  }
    )
  };



async delete(id){

// silmeden önce sweetalert ile onay soruyorum
const isConfirmed = await this.swal.areYouSure();


if(isConfirmed){
this.commentService.delete(id).subscribe({
  error: result => {console.error(result.error);
    alertify.error(t('msg.error'))
  },
  complete: () => {alertify.success(t('msg.deleted'));
    this.getcomments();
  }
})

}
else{
  console.log("Delete Reverted")
}



};





}
