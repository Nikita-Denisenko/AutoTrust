import { useState, useEffect } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCar, faShieldAlt, faComments, faUser, faEnvelope, faLock, faPhone, faCalendar, faVenusMars, faCity } from '@fortawesome/free-solid-svg-icons';

const RegisterPage = () => {
  const [formData, setFormData] = useState({
    name: '', surname: '', patronymic: '', birthDate: '', gender: 'Male',
    cityId: '',
    email: '', phone: '', password: ''
  });
  const [cities, setCities] = useState([]);
  const [loadingCities, setLoadingCities] = useState(true);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  const [animate, setAnimate] = useState(false);
  const { register } = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    setAnimate(true);
  }, []);

  // Загрузка городов
  useEffect(() => {
    const loadCities = async () => {
      try {
        const response = await fetch('http://localhost:5230/api/cities?Size=500');
        const data = await response.json();
        setCities(Array.isArray(data) ? data : []);
      } catch (err) {
        console.error('Ошибка загрузки городов', err);
      } finally {
        setLoadingCities(false);
      }
    };
    loadCities();
  }, []);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setSuccess('');

    const cityIdNum = parseInt(formData.cityId);
    if (isNaN(cityIdNum) || cityIdNum < 1) {
      setError('Выберите город');
      return;
    }

    const submitData = {
      name: formData.name,
      surname: formData.surname,
      patronymic: formData.patronymic,
      birthDate: formData.birthDate,
      gender: formData.gender,
      cityId: cityIdNum,
      email: formData.email,
      phone: formData.phone,
      password: formData.password
    };

    try {
      await register(submitData);
      setSuccess('Регистрация успешна! Перенаправление...');
      setTimeout(() => {
        navigate('/');
      }, 1500);
    } catch (err) {
      setError(err.response?.data || 'Ошибка регистрации');
    }
  };

  const styles = `
    .register-page-container {
      position: fixed;
      top: 0;
      left: 0;
      width: 100%;
      height: 100vh;
      overflow: hidden;
    }
    .register-card {
      transition: box-shadow 0.2s ease;
    }
    .register-card:hover {
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
      <div className="register-page-container">
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
                <div className="card register-card shadow-lg border-0 rounded-4">
                  <div className="card-body p-5">
                    <div className="text-center mb-4">
                      <img 
                        src="https://psv4.userapi.com/s/v1/d2/-NziyU8yq9anH40AyA2SoYUIQ6X9SoDwD43tmTN81ttusIMDp37Yz9rvvOlm96xDcdQwy9Bs6CllILJlQdfOLbak1tHVxdyi6_UQ7SFxmWqjP9pCKBdtAgplJUghB1HvtBezW2dM8imi/2Buqbhy1DN2fzRhf6iGYKC8l-LdCwlPkxT0J6Lnus6aSMbckxvQxYrc3V-hPnj2VZdRfUVU7CqrYm-73vLDdD8eb_1.png" 
                        alt="AutoTrust" 
                        width="70" 
                        height="70" 
                        className="mb-3 d-lg-none"
                      />
                      <h2 className="fw-bold">Регистрация</h2>
                      <p className="text-muted fs-5">Создайте аккаунт</p>
                    </div>
                    
                    {success && (
                      <div className="alert alert-success" role="alert">
                        {success}
                      </div>
                    )}
                    
                    {error && (
                      <div className="alert alert-danger" role="alert">
                        {error}
                      </div>
                    )}
                    
                    <form onSubmit={handleSubmit}>
                      <div className="row g-3">
                        <div className="col-md-4">
                          <div className="input-group">
                            <span className="input-group-text bg-light border-end-0">
                              <FontAwesomeIcon icon={faUser} className="text-muted" />
                            </span>
                            <input 
                              type="text" 
                              className="form-control border-start-0 ps-0" 
                              placeholder="Имя" 
                              value={formData.name} 
                              onChange={(e) => setFormData({...formData, name: e.target.value})} 
                              required 
                            />
                          </div>
                        </div>
                        <div className="col-md-4">
                          <input 
                            type="text" 
                            className="form-control" 
                            placeholder="Фамилия" 
                            value={formData.surname} 
                            onChange={(e) => setFormData({...formData, surname: e.target.value})} 
                            required 
                          />
                        </div>
                        <div className="col-md-4">
                          <input 
                            type="text" 
                            className="form-control" 
                            placeholder="Отчество" 
                            value={formData.patronymic} 
                            onChange={(e) => setFormData({...formData, patronymic: e.target.value})} 
                            required 
                          />
                        </div>
                        <div className="col-md-6">
                          <div className="input-group">
                            <span className="input-group-text bg-light border-end-0">
                              <FontAwesomeIcon icon={faCalendar} className="text-muted" />
                            </span>
                            <input 
                              type="date" 
                              className="form-control border-start-0 ps-0" 
                              value={formData.birthDate} 
                              onChange={(e) => setFormData({...formData, birthDate: e.target.value})} 
                              required 
                            />
                          </div>
                        </div>
                        <div className="col-md-6">
                          <div className="input-group">
                            <span className="input-group-text bg-light border-end-0">
                              <FontAwesomeIcon icon={faVenusMars} className="text-muted" />
                            </span>
                            <select 
                              className="form-select border-start-0 ps-0" 
                              value={formData.gender} 
                              onChange={(e) => setFormData({...formData, gender: e.target.value})}
                            >
                              <option value="Male">Мужской</option>
                              <option value="Female">Женский</option>
                            </select>
                          </div>
                        </div>
                        <div className="col-12">
                          <div className="input-group">
                            <span className="input-group-text bg-light border-end-0">
                              <FontAwesomeIcon icon={faEnvelope} className="text-muted" />
                            </span>
                            <input 
                              type="email" 
                              className="form-control border-start-0 ps-0" 
                              placeholder="Email" 
                              value={formData.email} 
                              onChange={(e) => setFormData({...formData, email: e.target.value})} 
                              required 
                            />
                          </div>
                        </div>
                        <div className="col-12">
                          <div className="input-group">
                            <span className="input-group-text bg-light border-end-0">
                              <FontAwesomeIcon icon={faPhone} className="text-muted" />
                            </span>
                            <input 
                              type="tel" 
                              className="form-control border-start-0 ps-0" 
                              placeholder="Телефон" 
                              value={formData.phone} 
                              onChange={(e) => setFormData({...formData, phone: e.target.value})} 
                              required 
                            />
                          </div>
                        </div>
                        
                        <div className="col-12">
                          <div className="input-group">
                            <span className="input-group-text bg-light border-end-0">
                              <FontAwesomeIcon icon={faCity} className="text-muted" />
                            </span>
                            <select
                              className="form-select border-start-0 ps-0"
                              value={formData.cityId}
                              onChange={(e) => setFormData({ ...formData, cityId: e.target.value })}
                              required
                              disabled={loadingCities}
                            >
                              <option value="">{loadingCities ? 'Загрузка городов...' : 'Выберите город'}</option>
                              {cities.map(city => (
                                <option key={city.id} value={city.id}>
                                  {city.name}
                                </option>
                              ))}
                            </select>
                          </div>
                        </div>

                        <div className="col-12">
                          <div className="input-group">
                            <span className="input-group-text bg-light border-end-0">
                              <FontAwesomeIcon icon={faLock} className="text-muted" />
                            </span>
                            <input 
                              type="password" 
                              className="form-control border-start-0 ps-0" 
                              placeholder="Пароль" 
                              value={formData.password} 
                              onChange={(e) => setFormData({...formData, password: e.target.value})} 
                              required 
                            />
                          </div>
                        </div>
                      </div>
                      
                      <button type="submit" className="btn btn-primary btn-lg w-100 mt-4">
                        Зарегистрироваться
                      </button>
                      
                      <div className="text-center mt-3">
                        <Link to="/login" className="text-decoration-none">
                          Уже есть аккаунт? Войти
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

export default RegisterPage;