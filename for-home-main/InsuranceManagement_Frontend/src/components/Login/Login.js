import React, { useContext, useState } from 'react';
import { Link, Navigate, useNavigate } from 'react-router-dom';
import { useForm } from 'react-hook-form';
import { LoginContext } from '../../context/LoginContext';
import axios from 'axios';
import { baseURL } from '../../Server';
import { jwtDecode } from 'jwt-decode';
import { UserInformationContext } from '../../context/UserInformationContext';
import loginImage from '../../assets/Images/loginImage.png'

const Login = () => {
  const navigate = useNavigate();
  const { setToken } = useContext(LoginContext);
  const token = localStorage.getItem("login");
  const { setCurrentUserId } = useContext(UserInformationContext);
  const [errorMessage, setErrorMessage] = useState(null);

  const { register, handleSubmit, formState: { errors } } = useForm();

  const onSubmit = async (data) => {
    try {
      const response = await axios.post(`${baseURL}/auth/login`, {
        email: data.email,
        password: data.password
      });
      const token = response.data.token;
      setToken(token);
      const decodedToken = jwtDecode(token);
      setCurrentUserId(decodedToken.nameid);
      localStorage.setItem("login", token);
      if (token) {
        navigate("/home");
      }
    } catch (error) {
      if (error.response.status === 401) {
 
        setErrorMessage('Invalid email or password');
      } else {
      
        console.error('Error:', error.message);
      }
    }
  };

  return (
    <>
      {token ? <Link to='/' /> : (
        <div className='login-main-container'>
          <div className='image-container'>
            <img src={loginImage}/>
          </div>

          <form onSubmit={handleSubmit(onSubmit)} className='login-form'>
          {errorMessage && <div style={{ color: 'red' }}>{errorMessage}</div>}
          <h2>Login</h2>
            <div>
              <input type="email" {...register('email', { required: true, pattern: /^\S+@\S+$/i })} placeholder='Email' autoFocus/>
              {errors.email && <span style={{ color: 'red' }}>Email is required and must be valid</span>}

            </div>
            <div>
              
              <input type="password" {...register('password', { required: true })} placeholder='Password'/>
              {errors.password && <span style={{ color: 'red' }}>Password is required</span>}
            </div>
            <button type="submit">Login</button>
          </form>
        </div>
      )}
    </>
  );
};

export default Login;
