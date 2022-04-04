import './App.css';
import LoginButton from "../Navbar/LoginButton"
import LogoutButton from "../Navbar/LogoutButton"
import Profile from "../Profile/Profile"

const App = () => {
  return (
    <div className='app'>
      <h1>Mechanix Admin</h1>
      <LoginButton />
      <LogoutButton />
      <Profile />
    </div>
  );
}

export default App;
