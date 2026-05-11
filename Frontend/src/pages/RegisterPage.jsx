import { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';

const RegisterPage = () => {
  const [formData, setFormData] = useState({
    name: '', surname: '', patronymic: '', birthDate: '', gender: 0,
    cityId: 1, email: '', phone: '', password: ''
  });
  const [error, setError] = useState('');
  const { register } = useAuth();
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await register(formData);
      navigate('/');
    } catch (err) {
      setError('Ошибка регистрации');
    }
  };

  return (
    <div className="row justify-content-center">
      <div className="col-md-6">
        <div className="card">
          <div className="card-body">
            <h2 className="text-center mb-4">Регистрация</h2>
            {error && <div className="alert alert-danger">{error}</div>}
            <form onSubmit={handleSubmit}>
              <div className="mb-3"><input type="text" className="form-control" placeholder="Имя" value={formData.name} onChange={(e) => setFormData({...formData, name: e.target.value})} required /></div>
              <div className="mb-3"><input type="text" className="form-control" placeholder="Фамилия" value={formData.surname} onChange={(e) => setFormData({...formData, surname: e.target.value})} required /></div>
              <div className="mb-3"><input type="text" className="form-control" placeholder="Отчество" value={formData.patronymic} onChange={(e) => setFormData({...formData, patronymic: e.target.value})} required /></div>
              <div className="mb-3"><input type="date" className="form-control" value={formData.birthDate} onChange={(e) => setFormData({...formData, birthDate: e.target.value})} required /></div>
              <div className="mb-3">
                <select className="form-select" value={formData.gender} onChange={(e) => setFormData({...formData, gender: parseInt(e.target.value)})}>
                  <option value="0">Мужской</option>
                  <option value="1">Женский</option>
                </select>
              </div>
              <div className="mb-3"><input type="email" className="form-control" placeholder="Email" value={formData.email} onChange={(e) => setFormData({...formData, email: e.target.value})} required /></div>
              <div className="mb-3"><input type="tel" className="form-control" placeholder="Телефон" value={formData.phone} onChange={(e) => setFormData({...formData, phone: e.target.value})} required /></div>
              <div className="mb-3"><input type="password" className="form-control" placeholder="Пароль" value={formData.password} onChange={(e) => setFormData({...formData, password: e.target.value})} required /></div>
              <button type="submit" className="btn btn-primary w-100">Зарегистрироваться</button>
              <div className="text-center mt-3">
                <Link to="/login">Уже есть аккаунт? Войти</Link>
              </div>
            </form>
          </div>
        </div>
      </div>
    </div>
  );
};

export default RegisterPage;