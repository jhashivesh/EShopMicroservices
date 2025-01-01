import { Injectable } from '@angular/core';
import { UserManager, User, UserManagerSettings } from 'oidc-client';
import { Subject } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private _userManager: UserManager;
  private _user: User | undefined | null;
  private _loginChangedSubject = new Subject<boolean>();
  public loginChanged = this._loginChangedSubject.asObservable();

  private get idpSettings(): UserManagerSettings {
    console.log('clientUrl::' + environment.clientUrl);
    console.log('idpUrl::' + environment.idpUrl);
    return {
      authority: environment.idpUrl,
      client_id: environment.clientId,
      redirect_uri: `${environment.clientUrl}/signin-callback`,
      scope: 'openid profile ecommerceApp',
      response_type: 'code',
      post_logout_redirect_uri: `${environment.clientUrl}/signout-callback`,
      loadUserInfo: true,
    };
  }

  constructor() {
    this._userManager = new UserManager(this.idpSettings);
  }

  public login = () => {
    return this._userManager.signinRedirect();
  };

  public isAuthenticated = (): Promise<boolean> => {
    return this._userManager.getUser().then((user) => {
      if (user) {
        if (this._user !== user) {
          this._loginChangedSubject.next(this.checkUser(user));
        }

        this._user = user;
        return this.checkUser(user);
      } else {
        return false;
      }
    });
  };

  private checkUser = (user: User): boolean => {
    return !!user && !user.expired;
  };

  public finishLogin = (): Promise<User> => {
    return this._userManager.signinRedirectCallback().then((user) => {
      this._user = user;
      this._loginChangedSubject.next(this.checkUser(user));

      return user;
    });
  };

  public logout = () => {
    sessionStorage.removeItem('token');
    this._userManager.signoutRedirect();
  };

  public finishLogout = () => {
    this._user = null;
    return this._userManager.signoutRedirectCallback();
  };

  public getAccessToken = (): Promise<string | null> => {
    return this._userManager.getUser().then((user) => {
      return !!user && !user.expired ? user.access_token : null;
    });
  };
}
