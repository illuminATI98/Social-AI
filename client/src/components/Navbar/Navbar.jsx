import React from 'react'
import { BrowserRouter, Link } from 'react-router-dom';
import "./Navbar.css";

const Navbar = () => {
  return (
    <div className='navbar'>
      <div className='navbar-logo'>
        <Link to="/">
          Home
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