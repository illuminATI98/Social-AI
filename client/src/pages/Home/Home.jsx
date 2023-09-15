import React, { useState, useEffect } from 'react';
import { BACKEND_API_URL } from '../../url';
import jwt_decode from "jwt-decode";
import Cookies from 'js-cookie';
import { PostCard } from '../../components';

const Home = () => {
  const [allPosts, setAllPosts] = useState(null);
  const [user, setUser] = useState(null);
  const [jwtToken, setJwtToken] = useState(null);

  useEffect(() => {
    try {
      const token = Cookies.get("jwtToken");
      if (token) {
        const decodedToken = jwt_decode(token);
        setUser(decodedToken);
        setJwtToken(token);
      }
    } catch (error) {
      console.error("Error decoding token:", error);
    }
  }, []);

  useEffect(() => {
    if (jwtToken) {
      async function getAllPosts() {
        try {
          const response = await fetch(`${BACKEND_API_URL}/api/posts`, {
            method: "GET",
            headers: {
              Accept: 'application/json',
              'Content-Type': 'application/json',
              'Authorization': `Bearer ${jwtToken}`
            },
          });
          const data = await response.json();
          setAllPosts(data["$values"]);
        } catch (error) {
          alert(error);
        }
      }

      getAllPosts();
    }
  }, [jwtToken]);
  
  return (
    <div className='home-container'>
      <div className='home-posts'>
        {allPosts === null ? (
          <div>Loading...</div>
        ) : (
          <div>
            {allPosts.map(post => (
              <PostCard key={post.id} postData={post} />
            ))}
          </div>
        )}
      </div>
    </div>
  );
}

export default Home