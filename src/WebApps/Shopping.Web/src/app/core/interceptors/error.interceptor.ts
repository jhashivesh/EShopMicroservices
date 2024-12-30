import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';
import { SnackbarService } from '../services/snackbar.service';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const snackbar = inject(SnackbarService);
  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 400) {
        if (error.error.errors) {
          const modelStateErrors = [];
          const errors = error.error.errors;
          for (const key in errors) {
            if (errors[key]) {
              modelStateErrors.push(errors[key]);
            }
          }
          throw modelStateErrors.flat();
        } else {
          snackbar.error(error.error.title || error.error || error);
        }
      }
      if (error.status === 401) {
        snackbar.error(error?.error?.title || error?.error || error?.message);
      }
      if (error.status === 404) {
        router.navigate(['/not-found']);
      }
      if (error.status === 500) {
        const navigationExtras: NavigationExtras = {
          state: { error: error.error },
        };
        router.navigate(['/server-error'], navigationExtras);
      }
      return throwError(() => error);
    })
  );
};
