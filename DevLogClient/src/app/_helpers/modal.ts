// bootstrap'in js'i angular.json'da scripts olarak ekli, global "bootstrap" nesnesini kullanıyorum
declare const bootstrap: any;

// açık olan bootstrap modal'larını kapatıyor
// NOT: eskiden kaydettikten sonra location.reload() ile bütün sayfayı yeniliyordum.
// sayfa yenilenince angular uygulaması ve bütün istekler baştan yükleniyor, admin panel yavaş hissettiriyordu.
// artık sadece modal'ı kapatıp listeyi tekrar çekiyorum
export function closeOpenModals() {
  document.querySelectorAll('.modal.show').forEach(element => {
    bootstrap.Modal.getInstance(element)?.hide();
  });
}
