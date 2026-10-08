import { Component, OnInit } from '@angular/core';
import { DashboardService } from '../../_services/dashboard-service';
import { DashboardDto } from '../../_models/dashboard';
import { AuthService } from '../../_services/auth-service';

// admin paneli açılınca ilk görünen ekran: sayılar, kategori grafiği, son yorumlar
@Component({
  selector: 'app-dashboard',
  standalone: false,
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class Dashboard implements OnInit {
  data: DashboardDto;
  // grafikteki en uzun çubuk %100 olsun diye en büyük sayıyı tutuyorum
  maxCount = 1;

  constructor(private dashboardService: DashboardService, public authService: AuthService) { }

  ngOnInit() {
    this.dashboardService.get().subscribe({
      next: result => {
        this.data = result.data;
        this.maxCount = Math.max(1, ...this.data.blogsPerCategory.map(x => x.count));
      }
    });
  }

  // çubuğun genişliği: sayı / en büyük sayı * 100
  // NOT: grafik için kütüphane eklemedim, basit yatay çubuklar css ile yeterli oldu
  barWidth(count: number) {
    return (count / this.maxCount) * 100;
  }
}
