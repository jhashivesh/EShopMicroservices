export const environment = {
  production: true,
  // apiUrl: 'http://165.232.144.187:8081'
  apiUrl: window['env']['apiUrl'] || 'default',
  idpUrl: window['env']['idpUrl'] || 'default',
  clientUrl: window['env']['clientUrl'] || 'default',
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
