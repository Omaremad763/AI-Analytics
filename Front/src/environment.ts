const isLocal = window.location.hostname === 'localhost';
export const environment = {
  production: !isLocal,
  apiUrl: isLocal ? 'https://localhost:44309/api' : 'server url',
};
