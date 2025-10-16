import React, { useEffect, useRef } from 'react';
import 'ol/ol.css';
import { Map, View } from 'ol';
import TileLayer from 'ol/layer/Tile';
import OSM from 'ol/source/OSM';
import { fromLonLat } from 'ol/proj';

export const MapComponent = () => {
  const mapRef = useRef(null);

  useEffect(() => {
    // Initialize map
    const map = new Map({
      target: mapRef.current,
      layers: [
        new TileLayer({
          source: new OSM(), 
        }),
      ],
      view: new View({
        center: fromLonLat([13.0038, 55.6050]),
        zoom: 12, 
      }),
    });

    return () => map.setTarget(null); 
  }, []);

  return (
    <div
      ref={mapRef}
      style={{
        width: '90%', 
        height: '500px', 
        borderRadius: '0', 
        boxShadow: 'none'
      }}
    />
  );
};

export default MapComponent;
