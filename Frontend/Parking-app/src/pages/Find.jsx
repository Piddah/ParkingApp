import React from 'react';
import MapComponent from '../components/MapComponent';
import './Find.css';

export const Find = () => {
  return (
    <div className="find-page">
      <h1>Find Parking</h1>
      <div className="map-container">
        <MapComponent />
      </div>
    </div>
  );
};

export default Find;