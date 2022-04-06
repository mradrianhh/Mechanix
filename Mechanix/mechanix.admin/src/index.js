import { Auth0Provider } from '@auth0/auth0-react';
import { render } from 'react-dom';
import App from "./Components/App/App"

function AppWithAuth0Provider(){
    return(
        <Auth0Provider
        domain="dev-4pw2cdab.us.auth0.com"
        clientId="Egk40ZVWhODHqWCIEpVvD5U0WzkRg0mK"
        redirectUri={window.location.origin}
        audience="https://mechanix.com"
        scope="read:cars"
        >
            <App />
        </Auth0Provider>
    );
}

const container = document.getElementById('root');
render(<AppWithAuth0Provider />,container);