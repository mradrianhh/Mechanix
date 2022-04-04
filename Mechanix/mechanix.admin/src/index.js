import React from 'react';
import ReactDOM from 'react-dom';
import './index.css';
import App from './Components/App/App';
import { Auth0Provider } from "@auth0/auth0-react"

ReactDOM.render(
    <Auth0Provider
        domain="dev-4pw2cdab.us.auth0.com"
        clientId="Egk40ZVWhODHqWCIEpVvD5U0WzkRg0mK"
        redirectUri={window.location.origin}
    >
        <App />
    </Auth0Provider>,
  document.getElementById('root')
);
