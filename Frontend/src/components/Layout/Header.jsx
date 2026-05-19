import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../../contexts/AuthContext';

const Header = () => {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  const getInitials = () => {
    const firstName = user?.name?.charAt(0) || '';
    const lastName = user?.surname?.charAt(0) || '';
    return `${firstName}${lastName}`.toUpperCase();
  };

  const avatarUrl = user?.avatarUrl || user?.avatar;

  const styles = `
    .nav-link-custom {
      position: relative;
      transition: all 0.3s ease;
    }
    .nav-link-custom::after {
      content: '';
      position: absolute;
      bottom: -4px;
      left: 50%;
      transform: translateX(-50%) scaleX(0);
      width: 80%;
      height: 2px;
      background: white;
      border-radius: 2px;
      transition: transform 0.3s ease;
    }
    .nav-link-custom:hover::after {
      transform: translateX(-50%) scaleX(1);
    }
    .nav-link-custom:hover {
      transform: translateY(-2px);
      text-shadow: 0 2px 4px rgba(0,0,0,0.2);
    }
    .btn-profile {
      transition: all 0.3s ease;
    }
    .btn-profile:hover {
      transform: scale(1.05);
      background: rgba(255,255,255,0.25) !important;
    }
    .dropdown-item-custom {
      transition: all 0.2s ease;
    }
    .dropdown-item-custom:hover {
      transform: translateX(4px);
      background-color: #f8f9fa;
    }
  `;

  return (
    <>
      <style>{styles}</style>
      <nav className="navbar navbar-expand-lg fixed-top shadow" style={{ background: 'linear-gradient(135deg, #1a1a64 0%, #192461 100%)', minHeight: '77px', }}>
        <div className="container">
          <Link className="navbar-brand d-flex align-items-center" to="/" style={{ gap: '10px' }}>
            <img 
              src="https://psv4.userapi.com/s/v1/d2/-NziyU8yq9anH40AyA2SoYUIQ6X9SoDwD43tmTN81ttusIMDp37Yz9rvvOlm96xDcdQwy9Bs6CllILJlQdfOLbak1tHVxdyi6_UQ7SFxmWqjP9pCKBdtAgplJUghB1HvtBezW2dM8imi/2Buqbhy1DN2fzRhf6iGYKC8l-LdCwlPkxT0J6Lnus6aSMbckxvQxYrc3V-hPnj2VZdRfUVU7CqrYm-73vLDdD8eb_1.png" 
              alt="AutoTrust" 
              width="35" 
              height="35" 
              style={{ objectFit: 'contain' }}
            />
            <span className="fw-bold fs-3 text-white">AutoTrust</span>
          </Link>
          
          <button className="navbar-toggler border-0" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav">
            <span className="navbar-toggler-icon"></span>
          </button>
          
          <div className="collapse navbar-collapse" id="navbarNav">
            <ul className="navbar-nav ms-auto" style={{ gap: '1rem' }}>
              <li className="nav-item">
                <Link className="nav-link fs-5 px-2 py-1 rounded-3 text-white nav-link-custom" to="/">Главная</Link>
              </li>
              
              {user ? (
                <>
                  <li className="nav-item">
                    <Link className="nav-link fs-5 px-2 py-1 rounded-3 text-white nav-link-custom" to="/feed">Лента</Link>
                  </li>
                  <li className="nav-item">
                    <Link className="nav-link fs-5 px-2 py-1 rounded-3 text-white nav-link-custom" to="/chats">Мессенджер</Link>
                  </li>
                  <li className="nav-item">
                    <Link className="nav-link fs-5 px-2 py-1 rounded-3 text-white nav-link-custom" to="/listings/sale">Продажи</Link>
                  </li>
                  <li className="nav-item">
                    <Link className="nav-link fs-5 px-2 py-1 rounded-3 text-white nav-link-custom" to="/listings/buy">Покупки</Link>
                  </li>
                  <li className="nav-item">
                    <Link className="nav-link fs-5 px-2 py-1 rounded-3 text-white nav-link-custom" to="/search">
                      🔍 Поиск
                    </Link>
                  </li>
                  
                  <li className="nav-item dropdown ms-2">
                    <button 
                      className="btn btn-link nav-link dropdown-toggle d-flex align-items-center gap-2 px-2 py-1 rounded-4 btn-profile" 
                      style={{ 
                        color: 'white', 
                        textDecoration: 'none', 
                        background: 'rgba(255,255,255,0.1)',
                        borderRadius: '40px'
                      }}
                      type="button" 
                      data-bs-toggle="dropdown" 
                      aria-expanded="false"
                    >
                      {avatarUrl ? (
                        <img 
                          src={avatarUrl} 
                          width="32" 
                          height="32" 
                          className="rounded-circle border border-2 border-white" 
                          alt="avatar" 
                          style={{ objectFit: 'cover' }}
                        />
                      ) : (
                        <div className="rounded-circle d-flex align-items-center justify-content-center border border-2 border-white" 
                             style={{ 
                               background: 'rgba(255,255,255,0.2)', 
                               width: '32px', 
                               height: '32px', 
                               color: 'white', 
                               fontSize: '14px', 
                               fontWeight: 'bold'
                             }}>
                          {getInitials()}
                        </div>
                      )}
                      <span className="fw-medium" style={{ fontSize: '14px' }}>{user?.name || 'Профиль'}</span>
                    </button>
                    <ul className="dropdown-menu dropdown-menu-end shadow border-0 mt-1" style={{ borderRadius: '12px', backgroundColor: 'white' }}>
                      <li><Link to="/profile" className="dropdown-item py-2 px-3 dropdown-item-custom" style={{ borderRadius: '8px' }}>👤 Мой профиль</Link></li>
                      <li><hr className="dropdown-divider my-1" /></li>
                      <li><button className="dropdown-item text-danger py-2 px-3 dropdown-item-custom" style={{ borderRadius: '8px' }} onClick={handleLogout}>🚪 Выйти</button></li>
                    </ul>
                  </li>
                </>
              ) : (
                <>
                  <li className="nav-item">
                    <Link className="nav-link fs-5 px-2 py-1 rounded-3 text-white nav-link-custom" to="/login">Вход</Link>
                  </li>
                  <li className="nav-item">
                    <Link className="btn btn-outline-light px-3 py-1 rounded-3 ms-2" to="/register" style={{ fontSize: '1rem', transition: 'all 0.3s ease' }} onMouseEnter={(e) => e.target.style.transform = 'scale(1.05)'} onMouseLeave={(e) => e.target.style.transform = 'scale(1)'}>Регистрация</Link>
                  </li>
                </>
              )}
            </ul>
          </div>
        </div>
      </nav>
    </>
  );
};

export default Header;