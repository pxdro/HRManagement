import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/core/services/auth.service';

@Component({
  selector: 'app-topnav',
  templateUrl: './topnav.component.html',
  styleUrls: ['./topnav.component.scss']
})
export class TopnavComponent {
  userEmail: string | null = null;

  constructor(private authService: AuthService, private router: Router) {
    this.userEmail = this.authService.getUserEmail();
  }

  redirectHome(){
    this.router.navigate(['/']);
  }

  redirectEmployees(){
    this.router.navigate(['/employees']);
  }

  redirectDepartments(){
    this.router.navigate(['/departments']);
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
