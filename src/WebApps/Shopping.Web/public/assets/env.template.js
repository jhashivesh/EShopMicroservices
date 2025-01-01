(function (window) {
  window.env = window.env || {};

  // Environment variables
  window["env"]["apiUrl"] = "${API_URL}";
  window["env"]["idpUrl"] = "${IDP_URL}";
  window["env"]["clientUrl"] = "${CLIENT_URL}";
})(this);
