import React, { useState, useEffect  } from 'react';
import { Route, Routes, useNavigate } from 'react-router-dom';
import { Home, CreatePost, Login, Register } from './pages';
import { Navbar } from './components';
import './App.scss';
import jwt_decode from "jwt-decode";
import Cookies from 'js-cookie';

function App() {
  const [user, setUser] = useState(null);
  const [jwtToken, setJwtToken] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    try {
      const token = Cookies.get("jwtToken");
      if (token) {
        const decodedToken = jwt_decode(token);
        setUser(decodedToken);
        setJwtToken(token);
        console.log(token)
      }
    } catch (error) {
      console.error("Error decoding token:", error);
    }
  }, []);

  const handleLogin = (responeToken) => {
    if (responeToken) {
      Cookies.set("jwtToken", responeToken);
      const decodedToken = jwt_decode(responeToken);
      const role = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
      if (decodedToken) { 
        navigate('/', { replace: true });
      }
      setUser(decodedToken);
    }
  };
  const handleLogout = () => {
    Cookies.remove("jwtToken");
    setUser(null);
    setToken(null);
    navigate('/', { replace: true });
  };

  return (
      <div className='main-container'>
        <Navbar handleLogout={handleLogout} user={user}/>
        {!user && <div className="page">
          <Routes>
            <Route path="/" element={<div>Please Log in</div>}/>
            <Route path="/login" element={<Login onLogin={handleLogin}/>}/>
            <Route path="/register" element={<Register/>}/>
          </Routes>
        </div>}

        {user && <div className="page">
          <Routes>
            <Route path="/" element={<Home/>}/>
            <Route path="/create-post" element={<CreatePost/>}/>
          </Routes>
        </div>}
        
      </div>
    
  )
}

export default App
