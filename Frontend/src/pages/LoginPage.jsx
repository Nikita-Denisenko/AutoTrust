import { useState, useEffect } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCar, faShieldAlt, faComments } from '@fortawesome/free-solid-svg-icons';

const LoginPage = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [animate, setAnimate] = useState(false);
  const { login } = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    setAnimate(true);
  }, []);

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await login(email, password);
      navigate('/');
    } catch (err) {
      setError('Неверный email или пароль');
    }
  };

  const styles = `
    .login-page-container {
      position: fixed;
      top: 0;
      left: 0;
      width: 100%;
      height: 100vh;
      overflow: hidden;
    }
    .login-card {
      transition: box-shadow 0.2s ease;
    }
    .login-card:hover {
      transform: none !important;
      box-shadow: 0 0 12px rgba(26, 115, 232, 0.5) !important;
    }
    .slide-in {
      animation: slideInRight 0.6s ease-out forwards;
    }
    @keyframes slideInRight {
      from {
        opacity: 0;
        transform: translateX(50px);
      }
      to {
        opacity: 1;
        transform: translateX(0);
      }
    }
    .fade-in {
      animation: fadeIn 0.8s ease-out forwards;
    }
    @keyframes fadeIn {
      from {
        opacity: 0;
      }
      to {
        opacity: 1;
      }
    }
  `;

  return (
    <>
      <style>{styles}</style>
      <div className="login-page-container">
        <div className="container-fluid p-0 h-100">
          <div className="row g-0 h-100">
            
            <div className="col-lg-5 h-100 d-none d-lg-flex flex-column justify-content-center text-white fade-in" style={{ background: 'linear-gradient(135deg, #0d47a1, #1976d2)' }}>
              <div className="text-center">
                <img 
                  src="https://psv4.userapi.com/s/v1/d2/-NziyU8yq9anH40AyA2SoYUIQ6X9SoDwD43tmTN81ttusIMDp37Yz9rvvOlm96xDcdQwy9Bs6CllILJlQdfOLbak1tHVxdyi6_UQ7SFxmWqjP9pCKBdtAgplJUghB1HvtBezW2dM8imi/2Buqbhy1DN2fzRhf6iGYKC8l-LdCwlPkxT0J6Lnus6aSMbckxvQxYrc3V-hPnj2VZdRfUVU7CqrYm-73vLDdD8eb_1.png" 
                  alt="AutoTrust" 
                  width="220" 
                  height="220" 
                  className="mb-2"
                />
                <h1 className="display-1 fw-bold">AutoTrust</h1>
                <p className="lead mt-2">Социальная сеть для<br />купли-продажи автомобилей</p>
              </div>
              
              <div className="text-center mt-5">
                <div className="d-flex gap-4 justify-content-center">
                  <div className="text-center">
                    <FontAwesomeIcon icon={faCar} size="2x" className="mb-2" />
                    <p className="mb-0 small">История владения</p>
                  </div>
                  <div className="text-center">
                    <FontAwesomeIcon icon={faShieldAlt} size="2x" className="mb-2" />
                    <p className="mb-0 small">Безопасные сделки</p>
                  </div>
                  <div className="text-center">
                    <FontAwesomeIcon icon={faComments} size="2x" className="mb-2" />
                    <p className="mb-0 small">Встроенный чат</p>
                  </div>
                </div>
              </div>
            </div>

            <div className={`col-lg-7 h-100 d-flex justify-content-center align-items-center p-4 ${animate ? 'slide-in' : ''}`}>
              <div className="col-md-10 col-lg-9 col-xl-8">
                <div className="card login-card shadow-lg border-0 rounded-4">
                  <div className="card-body p-5">
                    <div className="text-center mb-4">
                      <img 
                        src="https://psv4.userapi.com/s/v1/d2/-NziyU8yq9anH40AyA2SoYUIQ6X9SoDwD43tmTN81ttusIMDp37Yz9rvvOlm96xDcdQwy9Bs6CllILJlQdfOLbak1tHVxdyi6_UQ7SFxmWqjP9pCKBdtAgplJUghB1HvtBezW2dM8imi/2Buqbhy1DN2fzRhf6iGYKC8l-LdCwlPkxT0J6Lnus6aSMbckxvQxYrc3V-hPnj2VZdRfUVU7CqrYm-73vLDdD8eb_1.png" 
                        alt="AutoTrust" 
                        width="70" 
                        height="70" 
                        className="mb-3 d-lg-none"
                      />
                      <h2 className="fw-bold">Добро пожаловать</h2>
                      <p className="text-muted fs-5">Войдите в свой аккаунт</p>
                    </div>
                    
                    {error && <div className="alert alert-danger">{error}</div>}
                    
                    <form onSubmit={handleSubmit}>
                      <div className="mb-3">
                        <label className="form-label fw-semibold">Email</label>
                        <input 
                          type="email" 
                          className="form-control form-control-lg rounded-3" 
                          placeholder="ivan@example.com"
                          value={email} 
                          onChange={(e) => setEmail(e.target.value)} 
                          required 
                        />
                      </div>
                      <div className="mb-3">
                        <label className="form-label fw-semibold">Пароль</label>
                        <input 
                          type="password" 
                          className="form-control form-control-lg rounded-3" 
                          placeholder="Введите пароль"
                          value={password} 
                          onChange={(e) => setPassword(e.target.value)} 
                          required 
                        />
                      </div>
                      <button 
                        type="submit" 
                        className="btn btn-primary btn-lg w-100 rounded-3 mb-3"
                        style={{ transition: 'background-color 0.2s ease' }}
                        onMouseEnter={(e) => e.target.style.backgroundColor = '#1557b0'}
                        onMouseLeave={(e) => e.target.style.backgroundColor = '#0d6efd'}
                      >
                        Войти
                      </button>
                      <div className="text-center">
                        <Link 
                          to="/register" 
                          className="text-decoration-none fw-semibold"
                          style={{ 
                            transition: 'color 0.2s ease',
                            color: '#0d6efd'
                          }}
                          onMouseEnter={(e) => e.target.style.color = '#1557b0'}
                          onMouseLeave={(e) => e.target.style.color = '#0d6efd'}
                        >
                          Нет аккаунта? Зарегистрироваться
                        </Link>
                      </div>
                    </form>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};

export default LoginPage;