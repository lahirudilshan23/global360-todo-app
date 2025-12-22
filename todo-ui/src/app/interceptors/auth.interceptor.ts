import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { catchError, switchMap, throwError } from 'rxjs';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const token = authService.accessToken;

  if (token) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }

  return next(req).pipe(
    catchError((err: HttpErrorResponse) => {
      if (err.status === 401 && token) {
        // Token expired then try refresh
        return authService.refreshToken()?.pipe(
          switchMap(res => {
            authService.storeTokens(res);

            // Retry failed request
            req = req.clone({
              setHeaders: {
                Authorization: `Bearer ${res.accessToken.token}`
              }
            });

            return next(req);
          }) || throwError(() => err)
        ) || throwError(() => err);
      }
      return throwError(() => err);
    })
  );
};