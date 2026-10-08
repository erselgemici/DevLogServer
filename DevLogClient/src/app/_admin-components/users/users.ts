import { Component, OnInit } from '@angular/core';
import { UserService } from '../../_services/user-service';
import { UserListDto } from '../../_models/userListDto';
import { AuthService } from '../../_services/auth-service';
import { t } from '../../_i18n/i18n';
declare const alertify: any;

@Component({
  selector: 'app-users',
  standalone: false,
  templateUrl: './users.html',
  styleUrl: './users.css'
})
// kullanıcıları ve rollerini yönettiğim sayfa (sadece admin)
export class Users implements OnInit {

  users: UserListDto[] = [];
  roles = ['Admin', 'Writer'];
  currentUserId = '';

  constructor(private userService: UserService, private authService: AuthService) {}

  // sayfa açılınca verileri çekiyorum
  ngOnInit(): void {
    this.currentUserId = this.authService.getUserId();
    this.getUsers();
  }

  getUsers() {
    this.userService.getAll().subscribe({
      next: result => this.users = result.data,
      error: () => alertify.error(t('msg.error'))
    });
  }

  // açılır listeden rol seçilince hemen kaydediyorum
  changeRole(user: UserListDto, role: string) {
    this.userService.changeRole(user.id, role).subscribe({
      next: () => {
        user.role = role;
        alertify.success(t('msg.roleUpdated'));
      },
      error: result => {
        alertify.error(result.error?.errors?.[0]?.errorMessage ?? t('msg.error'));
        this.getUsers();
      }
    });
  }
}
