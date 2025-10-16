import { Routes, Route, useNavigate, useLocation } from 'react-router-dom';
import { About } from '../pages/About';
import { Contact } from '../pages/Contact';
import { LoginScreen } from '../pages/LoginScreen';
import { MainPage } from '../pages/MainPage';
import { Find } from '../pages/Find';
import './Layout.css';
import { Tickets } from '../pages/Tickets';
import { Cars } from '../pages/Cars';
import { useEffect } from 'react';
import useAuthStore from '../context/AuthStore';

export const Layout = () => {
    const navigate = useNavigate();
    const location = useLocation();

    const isLoginScreen = location.pathname === '/' || location.pathname === '/register';
    const token = useAuthStore((s) => s.token);

    useEffect(() => {
        if (!isLoginScreen && !token) {
            navigate('/');
        }
    }, [isLoginScreen, token, navigate]);

    return (
        <div className="layout">
            <div className="content">
                <Routes>
                    <Route path="/" element={<LoginScreen />} />
                    <Route path="/register" element={<LoginScreen />} />
                    <Route path="/main" element={<MainPage />} />
                    <Route path="/about" element={<About />} />
                    <Route path="/contact" element={<Contact />} />
                    <Route path="/find" element={<Find />} />
                    <Route path="/tickets" element={<Tickets />} />
                    <Route path="/cars" element={<Cars />} />
                </Routes>
            </div>
            
            {!isLoginScreen && (
            <nav className="bottom-nav">
                <button onClick={() => navigate('/find')}>Find</button>
                <button onClick={() => navigate('/tickets')}>Tickets</button>
                <button onClick={() => navigate('/cars')}>Cars</button>
            </nav>
            )}
        </div>
    );
};

export default Layout;
