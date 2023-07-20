import React, { useEffect, useState } from 'react';
import { PropagateLoader } from "react-spinners";
import "./CreatePost.css";
import preview from "../../assets/preview.jpg";

const CreatePost = () => {
  const [form, setForm] = useState({
    prompt:"",
    description:"",
    img:""
  });

  const [generatingImg, setGeneratingImg] = useState(false);

  function updateForm(value) {
    return setForm( (prev)=> {
     return {...prev, ...value}});
  };

  async function handleCreateImage(e){
    e.preventDefault();
    if(form.prompt){
      try{
        setGeneratingImg(true);
        const response = await fetch("https://localhost:44333/GenerateAIImage", {
          method: "POST",
          headers: {
            Accept: 'application/json',
            'Content-Type': 'application/json'
          },
          body: JSON.stringify(form.prompt),
        })

        const data = await response.json()

        updateForm({img:data[0]});
      } catch (error) {
        alert(error);
      }finally {
        setGeneratingImg(false);
      }
    } else {
      alert("Please enter a prompt");
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

        {form.img ? (
          <img
            src={form.img}
            alt={form.prompt}
            className='create-post-img'
          />
        ): (
          <img
            src={preview}
            alt="preview"
            className='create-post-img'
          />
        )}

        {generatingImg && (
          <div>
            <PropagateLoader
              color="#36d7b7"
              loading
            />
          </div>
        )}
        <label htmlFor="description">Description</label>
        <input 
        type="text"
        className='create-post-input'
        id='description'
        value={form.description}
        onChange={(e) => updateForm({description:e.target.value})}
        />

      </form>
    </div>
  )
}

export default CreatePost