import { useAuth0 } from "@auth0/auth0-react";
import { useApi } from "../../Auth/use-api";

const Profile = () => {
  const opts = {
    audience: 'https://mechanix.com',
    scope: 'read:cars',
  };
  const { user, isAuthenticated, isLoading} = useAuth0();
  const { data: cars } = useApi(
    'https://localhost:7049/cars',
    opts
  );

  if (isLoading) {
    return <div>Loading ...</div>;
  }

  return (
    isAuthenticated && (
      <div>
        <img src={user.picture} alt={user.name} />
        <h2>{user.name}</h2>
        <p>{user.email}</p>
        <h3>Cars</h3>
        {cars ? (
          <pre>{JSON.stringify(cars, null, 2)}</pre>
        ) : (
          "No user metadata defined"
        )}
      </div>
    )
  );
};

export default Profile;