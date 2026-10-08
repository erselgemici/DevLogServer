import { SweetalertService } from '../../_services/sweetalert-service';
import { t } from '../../_i18n/i18n';
import { closeOpenModals } from '../../_helpers/modal';
import { Component } from '@angular/core';
import { CategoryService } from '../../_services/category-service';
import { CategoryDto } from '../../_models/category';
declare const alertify :any;

@Component({
  selector: 'admin-category',
  standalone: false,
  templateUrl: './category.html',
  styleUrl: './category.css'
})
export class Category {

constructor(private categoryService : CategoryService,
            private swal: SweetalertService
){

  this.getCategories();
}

categories: CategoryDto[];
newCategory: CategoryDto = new CategoryDto();
editCategory:any = {};
// backend'den gelen validation hataları, html'de alan adına göre gösteriyorum
errors: any=[];


getCategories(){
  this.categoryService.getCategories().subscribe({
    next: result=> this.categories= result.data,
    error: result=> console.log(result.errors)
  }
    )
  };

  createCategory(){
      this.categoryService.create(this.newCategory).subscribe({
        next: result => this.categories.push(result.data),
        error: result =>{
            alertify.error(t('msg.error'));

            if(result.status===400){
              console.log(result.error.errors)
              this.errors= result.error.errors
            };
        } ,
        complete: () => {
          alertify.success(t('msg.created'));

         // işlem bitince sayfayı yenilemek yerine modal'ı kapatıp listeyi tekrar çekiyorum
          closeOpenModals();
          this.getCategories();
          // formu temizle, tekrar açınca eski değerler görünmesin
          this.newCategory = new CategoryDto();
        }
      })
  };

async delete(id){

// silmeden önce sweetalert ile onay soruyorum
const isConfirmed = await this.swal.areYouSure();


if(isConfirmed){
this.categoryService.delete(id).subscribe({
  error: result => {console.error(result.error);
    alertify.error(t('msg.error'))
  },
  complete: () => {alertify.success(t('msg.deleted'));
    this.getCategories();
  }
})

}
else{
  console.log("Delete Reverted")
}



};

update(){
this.categoryService.update(this.editCategory).subscribe({
  error: result => {
    alertify.error(t('msg.error'));

    if(result.status===400){
              console.log(result.error)
              this.errors= result.error.errors
            };
  },
  complete: () => {
    alertify.success(t('msg.updated'));

   // işlem bitince sayfayı yenilemek yerine modal'ı kapatıp listeyi tekrar çekiyorum
          closeOpenModals();
          this.getCategories();
        }

})

};


// düzenle butonuna basınca seçilen kaydı modal'a dolduruyor
onSelected(model:CategoryDto){
  this.editCategory= model;
}



}




