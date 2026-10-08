// backend'deki BaseResult'ın karşılığı, bütün cevaplar data ve errors şeklinde geliyor
export class Result<T>{
  data: T;
  errors: [];

}
