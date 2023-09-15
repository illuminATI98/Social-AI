import { React, useEffect, useState } from 'react';
import jwt_decode from "jwt-decode";
import Cookies from 'js-cookie';

function JwtTokenHandler() {
  const [user, setUser] = useState(null);
  const [jwtToken, setJwtToken] = useState(null);

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

  return {user,jwtToken};
}

export default JwtTokenHandler