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
          Create
        </Link>
      </div>
    </div>
  )
}

export default Navbar