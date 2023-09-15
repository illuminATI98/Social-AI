import React from 'react'
import { Link } from 'react-router-dom';
import "./Navbar.scss";
import Logo from "../../assets/logo.png";


const Navbar = ({ handleLogout, user }) => {
  
  return (
    <div className='navbar'>
      <div className='navbar-logo'>
        <Link to="/">
          <img src={Logo} alt="Logo" />
        </Link>
      </div>

      {user === null && 
      <>
        <div className='navbar-login'>
        <Link to="/login">
          <button className='navbar-login-btn'>Login</button>
        </Link>
        </div>

        <div className='navbar-register'>
        <Link to="/register">
          <button className='navbar-register-btn'>Register</button>
        </Link>
        </div>
      </>}

      {user !== null &&
      <>
      <div className='navbar-create'>
        <Link to="/create-post">
          <button className='navbar-create-btn'>Create</button>
        </Link>
      </div>
      <div className='navbar-logout'>
          <button onClick={handleLogout} className='navbar-logout-btn'>Logout</button>
      </div>
      </>}

    </div>
  )
}

export default Navbar