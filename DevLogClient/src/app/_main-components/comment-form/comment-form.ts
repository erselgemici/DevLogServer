import { Component } from '@angular/core';
import { t } from '../../_i18n/i18n';
import { CommentService } from '../../_services/comment-service';
import { CommentDto } from '../../_models/commentDto';
import { ActivatedRoute } from '@angular/router';
declare const alertify: any;

@Component({
  selector: 'comment-form',
  standalone: false,
  templateUrl: './comment-form.html',
  styleUrl: './comment-form.css'
})
export class CommentForm {
  newComment: CommentDto = new CommentDto();

constructor(private commentService: CommentService,
            private route: ActivatedRoute
){}


createComment(){
// yorumun hangi bloga ait olduğunu url'deki id'den alıyorum
this.newComment.blogId= this.route.snapshot.params["id"];

  this.commentService.create(this.newComment).subscribe({
    error: result => alertify.error(t('msg.commentFailed')),
    complete: () => {
      alertify.success(t('msg.commentPosted'));
      // işlem bitince sayfayı yeniliyorum
      location.reload();
    }
  })

}


}
