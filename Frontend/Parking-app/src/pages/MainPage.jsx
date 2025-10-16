import React from 'react';
import { useNavigate } from 'react-router-dom';
import './MainPage.css';

export const MainPage = () => {
  const navigate = useNavigate();

  return (
    <div className="main-page">
      <h1>Welcome to the Main Page</h1>

      <div className="button-group">
        <button onClick={() => navigate('/find')}>Find</button>
        <button onClick={() => navigate('/cars')}>Cars</button>
        <button onClick={() => navigate('/tickets')}>Tickets</button>
      </div>
    </div>
  );
};

export default MainPage;
