import { Injectable } from '@angular/core';
import Swal, { SweetAlertResult } from 'sweetalert2'
import { t } from '../_i18n/i18n';

@Injectable({
  providedIn: 'root'
})
export class SweetalertService {




areYouSure() :Promise<boolean>  {
  return Swal.fire({
  title: t('msg.areYouSure'),
  text: t('msg.cannotRevert'),
  icon: "warning",
  showCancelButton: true,
  confirmButtonColor: "#3085d6",
  cancelButtonColor: "#d33",
  confirmButtonText: t('msg.yesDelete'),
  cancelButtonText: t('msg.cancel')
}).then((result: SweetAlertResult) => {

  return result.isConfirmed;



})}

}
