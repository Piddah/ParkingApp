import { useEffect, useState } from 'react';
import useAuthStore from '../context/AuthStore';
import { apiFetch } from '../api/client';

export const Cars = () => {
    const [cars, setCars] = useState([]);
    const [selectedCar, setSelectedCar] = useState(null);
    const userId = useAuthStore((state) => state.userId);

    useEffect(() => {
        const fetchCars = async () => {
            try {
                const response = await apiFetch(`/cars`);
                if (response.ok) {
                    const data = await response.json();
                    setCars(Array.isArray(data) ? data : []);
                } else {
                    const errorMessage = await response.json();
                    alert(`Failed to fetch cars: ${errorMessage.error || response.status}`);
                }
            } catch (error) {
                console.error('Error fetching cars:', error);
            }
        };

        if (userId) {
            fetchCars();
        }
    }, [userId]);

    const addCar = async () => {
        const numberPlate = prompt('Enter the car number plate:');
        const newCar = { numberplate: numberPlate };

        if (!userId) {
            alert('User is not logged in!');
            return;
        }

        if (!numberPlate) {
            alert('Number plate is required!');
            return;
        }

        try {
            const response = await apiFetch(`/cars`, {
                method: 'POST',
                body: JSON.stringify(newCar),
            });

            if (response.ok) {
                const car = await response.json();
                alert(`Car added Successfully: ${car.numberplate}`);
                setCars((prevCars) => Array.isArray(prevCars) ? [...prevCars, { numberplate: car.numberplate, active: false }] : [{ numberplate: car.numberplate, active: false }]);
            } else {
                const errorMessage = await response.json();
                alert(`Failed to add car: ${errorMessage.error}`);
            }
        } catch (error) {
            console.error('Error adding car:', error);
            alert('An error occurred while adding the car.');
        }
    };

    const removeCar = async () => {
        if (!selectedCar) {
            alert('Please select a car to remove.');
            return;
        }

        try {
            const response = await apiFetch(`/cars/${selectedCar}`, { method: 'DELETE' });

            if (response.ok) {
                setCars((prevCars) => Array.isArray(prevCars) ? prevCars.filter((car) => car.numberplate !== selectedCar) : []);
                alert(`Car ${selectedCar} removed successfully!`);
                setSelectedCar(null);
            } else {
                const errorMessage = await response.json();
                alert(`Failed to remove car: ${errorMessage.error}`);
            }
        } catch (error) {
            console.error('Error removing car:', error);
            alert('An error occurred while removing the car.');
        }
    };

    const startParking = async () => {
        if (!selectedCar) { alert('Select a car first.'); return; }
        try {
            const res = await apiFetch(`/parking/start/${selectedCar}`, { method: 'POST' });
            if (res.ok) {
                alert('Parking started');
                setCars(prev => prev.map(c => c.numberplate === selectedCar ? { ...c, active: true } : c));
            } else {
                const err = await res.json();
                alert(err.error || 'Failed to start');
            }
        } catch (e) { console.error(e); }
    };

    const endParking = async () => {
        if (!selectedCar) { alert('Select a car first.'); return; }
        try {
            const res = await apiFetch(`/parking/end/${selectedCar}`, { method: 'POST' });
            if (res.ok) {
                const data = await res.json();
                alert(`Parking ended. Cost: ${data.cost}`);
                setCars(prev => prev.map(c => c.numberplate === selectedCar ? { ...c, active: false } : c));
            } else {
                const err = await res.json();
                alert(err.error || 'Failed to end');
            }
        } catch (e) { console.error(e); }
    };

    return (
        <div>
            <h1>Cars</h1>
            <ul>
                {cars.length > 0 ? (
                    cars.map((car) => (
                        <li
                            key={car.numberplate}
                            onClick={() => setSelectedCar(car.numberplate)}
                            style={{
                                cursor: 'pointer',
                                fontWeight: selectedCar === car.numberplate ? 'bold' : 'normal',
                            }}
                        >
                            {car.numberplate} {car.active ? '(active)' : ''}
                        </li>
                    ))
                ) : (
                    <p>No cars available</p>
                )}
            </ul>
            <button onClick={addCar}>Add Car</button>
            <button onClick={removeCar} disabled={!selectedCar}>
                Remove Car
            </button>
            <div style={{ marginTop: 12 }}>
                <button onClick={startParking} disabled={!selectedCar}>Start Parking</button>
                <button onClick={endParking} disabled={!selectedCar}>End Parking</button>
            </div>
        </div>
    );
};
