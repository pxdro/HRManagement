import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { ApiResponse, Tokens } from 'src/app/shared/models/apiModels.model';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private apiUrl = 'api/auth';

  constructor(private http: HttpClient) {}

  login(email: string, password: string): Observable<ApiResponse<Tokens>> {
    return this.http.post<ApiResponse<Tokens>>(`${this.apiUrl}/login`, { email, password }).pipe(
      tap(response => {
        localStorage.setItem('authToken', response.data.authToken);
      })
    );
  }

  logout(): void {
    localStorage.removeItem('authToken');
  }

  getToken(): string | null {
    return localStorage.getItem('authToken');
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }
}