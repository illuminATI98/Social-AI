import React from 'react'
import { Link } from 'react-router-dom';
import "./Navbar.css";
import Logo from "../../assets/logo.png";

const Navbar = () => {
  return (
    <div className='navbar'>
      <div className='navbar-logo'>
        <Link to="/">
          <img src={Logo} alt="Logo" />
        </Link>
      </div>
      <div className='navbar-create'>
        <Link to="/create-post">
          <button className='navbar-create-btn'>Create</button>
        </Link>
      </div>
    </div>
  )
}

export default Navbar