import React, { useEffect, useState } from 'react';
import { PropagateLoader } from "react-spinners";
import "./CreatePost.scss";
import preview from "../../assets/preview.jpg";
import { BACKEND_API_URL } from '../../url';
import jwt_decode from "jwt-decode";
import Cookies from 'js-cookie';

const CreatePost = () => {
  const [user, setUser] = useState(null);
  const [jwtToken, setJwtToken] = useState(null);
  const [form, setForm] = useState({
    prompt:"",
    image:"",
    description:""
  });
  const [generatingImg, setGeneratingImg] = useState(false);

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

  function updateForm(value) {
    return setForm( (prev)=> {
     return {...prev, ...value}});
  };

  async function handleCreateImage(e){
    e.preventDefault();
    if(form.prompt){
      try{
        setGeneratingImg(true);
        const response = await fetch(`${BACKEND_API_URL}/api/GenerateAIImage`, {
          method: "POST",
          headers: {
            Accept: 'application/json',
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${jwtToken}`
          },
          body: JSON.stringify(form.prompt)
        })

        const data = await response.json()
        console.log(data)
        updateForm({image:data['$values'][0]}); 
      } catch (error) {
        alert(error);
      }finally {
        setGeneratingImg(false);
      }
    } else {
      alert("Please enter a prompt");
    }
  };

  async function handlePost(e){
    e.preventDefault();
    if(form.image){
      try{
        const response = await fetch(`${BACKEND_API_URL}/api/posts`, {
          method: "POST",
          headers: {
            Accept: 'application/json',
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${jwtToken}`
          },
          body: JSON.stringify(form)
        })

      } catch (error) {
        alert(error);
      }
    } else {
      alert("Please generate image");
    }
  };

  function handleSubmit(e){
    e.preventDefault();
  };

  return (
    <div className='create-post'>
      <h1>Create</h1>
      <p>Create imaginative images through DALL-E AI and share them with the community</p>
      <form onSubmit={handleSubmit}>
        <label htmlFor="prompt">Prompt</label>
        <input 
        type="text"
        className='create-post-input'
        id='prompt'
        value={form.prompt}
        onChange={(e) => updateForm({prompt:e.target.value})}
        />

        <button className='create-post-button' onClick={handleCreateImage}>Create image</button>

        {form.image ? (
          <div className='create-post-img-container'>
            {generatingImg && (
              <div className='create-post-loader'>
                <PropagateLoader
                  color="#36d7b7"
                  loading
                />
              </div>
            )}
            <img
              src={form.image}
              alt={form.prompt}
              className='create-post-img'
            />
          </div>
        ): (
          <>
            {generatingImg && (
              <div className='create-post-loader'>
                <PropagateLoader
                  color="#36d7b7"
                  loading
                />
              </div>
            )}
            <img
              src={preview}
              alt="preview"
              className='create-post-img'
            />
          </>
        )}

        <label htmlFor="description">Description</label>
        <input 
        type="text"
        className='create-post-input'
        id='description'
        value={form.description}
        onChange={(e) => updateForm({description:e.target.value})}
        />

        <button className='create-post-button' onClick={handlePost}>Post image</button>

      </form>
    </div>
  )
}

export default CreatePost