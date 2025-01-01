export const environment = {
  production: true,
  // apiUrl: 'http://165.232.144.187:8081'
  apiUrl: window['env']['apiUrl'],
  idpUrl: window['env']['idpUrl'],
  clientUrl: window['env']['clientUrl'],
  clientId: 'angular-client',
  name: 'Production',
};

declare global {
  interface Window {
    env: {
      apiUrl: string;
      idpUrl: string;
      clientUrl: string;
    };
  }
}
