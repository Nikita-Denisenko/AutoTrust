// src/components/Layout/Header.jsx
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../../contexts/AuthContext';

const Header = () => {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <nav className="navbar navbar-expand-lg navbar-dark bg-dark fixed-top" style={{ marginBottom: 0, padding: '0.75rem 0' }}>
      <div className="container">
        <Link className="navbar-brand d-flex align-items-center" to="/" style={{ gap: '12px' }}>
          <img 
            src="https://psv4.userapi.com/s/v1/d2/-NziyU8yq9anH40AyA2SoYUIQ6X9SoDwD43tmTN81ttusIMDp37Yz9rvvOlm96xDcdQwy9Bs6CllILJlQdfOLbak1tHVxdyi6_UQ7SFxmWqjP9pCKBdtAgplJUghB1HvtBezW2dM8imi/2Buqbhy1DN2fzRhf6iGYKC8l-LdCwlPkxT0J6Lnus6aSMbckxvQxYrc3V-hPnj2VZdRfUVU7CqrYm-73vLDdD8eb_1.png" 
            alt="AutoTrust" 
            width="40" 
            height="40" 
            style={{ objectFit: 'contain' }}
          />
          <span className="fw-bold fs-3">AutoTrust</span>
        </Link>
        <button className="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav">
          <span className="navbar-toggler-icon"></span>
        </button>
        <div className="collapse navbar-collapse" id="navbarNav">
          <ul className="navbar-nav ms-auto" style={{ gap: '2rem' }}>
            <li className="nav-item"><Link className="nav-link fs-5" to="/">Главная</Link></li>
            {user ? (
              <>
                <li className="nav-item"><Link className="nav-link fs-5" to="/profile">Профиль</Link></li>
                <li className="nav-item"><button className="btn btn-link nav-link fs-5" onClick={handleLogout}>Выйти</button></li>
              </>
            ) : (
              <>
                <li className="nav-item"><Link className="nav-link fs-5" to="/login">Вход</Link></li>
                <li className="nav-item"><Link className="nav-link fs-5" to="/register">Регистрация</Link></li>
              </>
            )}
          </ul>
        </div>
      </div>
    </nav>
  );
};

export default Header;