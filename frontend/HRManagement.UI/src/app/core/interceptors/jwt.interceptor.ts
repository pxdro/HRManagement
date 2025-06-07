import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import { catchError } from 'rxjs/operators';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService, private router: Router) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = this.authService.getToken();

    const clonedReq = token
      ? req.clone({
          setHeaders: {
            Authorization: `Bearer ${token}`
          }
        })
      : req;

    return next.handle(clonedReq).pipe(
      catchError((error: HttpErrorResponse) => {
        // Unauthorized and not /auth Endpoint, redirect to auth
        if (
          error.status === 401 &&
          !req.url.includes('/auth')
        ) {
          this.authService.logout();
          this.router.navigate(['/auth']);
        }
        return throwError(() => error);
      })
    );
  }
}
