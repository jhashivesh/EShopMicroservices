import {
  HttpContextToken,
  HttpInterceptorFn,
  HttpRequest,
} from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { from, switchMap } from 'rxjs';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authSvc = inject(AuthService);
  // whenever this HttpContextToken is attached to a request
  // as we did to login request earlier
  // it means the user does not need to be authenticated
  // so we don't attach authorization header
  if (req.context.get(IS_PUBLIC)) {
    return next(req);
  }
  return from(authSvc.isAuthenticated()).pipe(
    switchMap((userAuthenticated) => {
      if (userAuthenticated) {
        const authRequest = addAuthorizationHeader(req);
        return next(authRequest);
      } else {
        return next(req);
      }
    })
  );
};
const addAuthorizationHeader = (req: HttpRequest<any>) => {
  const key = sessionStorage.key(0);
  const sessionItem = key ? sessionStorage.getItem(key) : null;
  const token = sessionItem ? JSON.parse(sessionItem).access_token : null;
  return req.clone({
    headers: req.headers.set('Authorization', `Bearer ${token}`),
  });
};
export const IS_PUBLIC = new HttpContextToken(() => false);
