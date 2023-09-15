import React , { useEffect , useState} from 'react'
import "./PostCard.scss"
import { BACKEND_API_URL } from '../../url';
import jwt_decode from "jwt-decode";
import Cookies from 'js-cookie';

const PostCard = ({postData}) => {
  const [userData, setUserData] = useState(null);
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

  /* useEffect(() => {
    if (postData.userId) {
      async function getUser() {
        try {
          const response = await fetch(`${BACKEND_API_URL}/user/${postData.userId}`, {
            method: "GET",
            headers: {
              Accept: 'application/json',
              'Content-Type': 'application/json',
              'Authorization': `Bearer ${jwtToken}`
            },
          });
          const data = await response.json();
          setUserData(data);

        } catch (error) {
          alert(error);
        }
      }

      getUser();

      console.log(userData)
    }
  }, [userData]); */

  return (
    <div className='post-container'>
      <div className='post-name'>{postData.userId}</div>
      <div className='post-image'>
        <img
          src={postData.image}
          alt={postData.prompt}
          className='post-img'
        />
      </div>
      <div className='post-interactions'></div>
      <div className='post-likes'></div>
      <div className='post-description'>{postData.description}</div>
      <div className='post-comments'></div>
    </div>
  )
}

export default PostCard