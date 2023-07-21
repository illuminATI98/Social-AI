import React, { useState, useEffect  } from 'react';
import { Route, Routes, useNavigate } from 'react-router-dom';
import { Home, CreatePost, Login, Register } from './pages';
import { Navbar } from './components';
import './App.css';
import jwt_decode from "jwt-decode";

function App() {
  const [loading, setloading] = useState(false);
  const [user, setUser] = useState(null);
  const [token, setToken] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    const token = localStorage.getItem("jwtToken");
    if (token) {
      const decodedToken = jwt_decode(token);
      setUser(decodedToken);
      setToken(token)
    } 
  }, []);

  const handleLogin = (responeToken) => {

    if (responeToken) {
      localStorage.setItem("jwtToken", responeToken);
      const decodedToken = jwt_decode(responeToken);
      const role = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
      setUser(decodedToken);
      
      if (decodedToken) { 
        navigate('/', { replace: true });
      }
    }
  };
  const handleLogout = () => {
    localStorage.removeItem("jwtToken");
    setUser(null);
    setToken(null);
    navigate('/', { replace: true });
  };

  return (
      <div className='main-container'>
        <Navbar handleLogout={handleLogout} user={user}/>
        {!user && <div className="page">
          <Routes>
            <Route path="/" element={<Home/>}/>
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
