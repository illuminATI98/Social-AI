import React, { useState } from 'react';
import { Post } from '../../components';

const Home = () => {
  const [loading, setloading] = useState(false);
  const [allPosts, setallPosts] = useState(null);

  return (
    <div className='home-container'>
      <div className='home-posts'>
        Home
      </div>
    </div>
  )
}

export default Home