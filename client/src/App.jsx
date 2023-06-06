import React, { useState, useEffect  } from 'react';
import { BrowserRouter, Link, Route, Routes } from 'react-router-dom';
import { Home, CreatePost } from './pages';
import { Navbar } from './components';
import './App.css';

function App() {
  const [loading, setloading] = useState(false);

  return (
    <BrowserRouter>
      <div className='main-container'>
        <Navbar/>
        <div className="page">
          <Routes>
            <Route path="/" element={<Home/>}/>
            <Route path="/create-post" element={<CreatePost/>}/>
          </Routes>
        </div>
      </div>
    </BrowserRouter>
  )
}

export default App
