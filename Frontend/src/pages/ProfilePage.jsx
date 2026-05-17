import { useState, useEffect } from 'react';
import { useAuth } from '../contexts/AuthContext';
import api from '../api/axiosConfig';

const ProfilePage = () => {
  const { user } = useAuth();
  const [profile, setProfile] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    const fetchProfile = async () => {
      try {
        const response = await api.get('/users/me');
        setProfile(response.data);
      } catch (err) {
        setError('Не удалось загрузить профиль');
      } finally {
        setLoading(false);
      }
    };
    fetchProfile();
  }, []);

  if (loading) return <div className="text-center mt-5">Загрузка...</div>;
  if (error) return <div className="alert alert-danger mt-5">{error}</div>;

  const getInitials = () => {
    const firstName = profile?.name?.charAt(0) || '';
    const lastName = profile?.surname?.charAt(0) || '';
    return `${firstName}${lastName}`.toUpperCase();
  };

  const getGenderText = (gender) => {
    if (gender === 'Male') return 'Мужской';
    if (gender === 'Female') return 'Женский';
    return 'Не указан';
  };

  const styles = `
    .profile-page-bg {
      background: linear-gradient(135deg, #e0f2fe 0%, #bae6fd 50%, #7dd3fc 100%);
      min-height: 100vh;
       margin-top: 0 !important;  
    }
    .profile-card {
      opacity: 0;
      transform: translateY(20px);
      animation: fadeInUp 0.6s ease-out forwards;
    }
    @keyframes fadeInUp {
      to {
        opacity: 1;
        transform: translateY(0);
      }
    }
  `;

  return (
    <>
      <style>{styles}</style>
      <div className="profile-page-bg py-5" style={{ marginTop: '76px' }}>
        <div className="container">
          <div className="row justify-content-center">
            <div className="col-lg-8">
              
              <div className="card border-0 shadow-sm rounded-4 overflow-hidden mb-4 profile-card">
                <div style={{ background: 'linear-gradient(135deg, #1e3c72 0%, #2a5298 100%)', height: '100px' }}></div>
                
                <div className="text-center" style={{ marginTop: '-50px', marginBottom: '16px' }}>
                  {profile?.avatarUrl ? (
                    <img 
                      src={profile.avatarUrl} 
                      alt="Аватар" 
                      className="rounded-circle border border-3 border-white shadow"
                      style={{ width: '100px', height: '100px', objectFit: 'cover' }} 
                    />
                  ) : (
                    <div className="rounded-circle border border-3 border-white shadow d-inline-flex align-items-center justify-content-center" 
                         style={{ 
                           background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)', 
                           width: '100px', 
                           height: '100px', 
                           fontSize: '38px', 
                           fontWeight: 500, 
                           color: 'white',
                           textTransform: 'uppercase'
                         }}>
                      {getInitials()}
                    </div>
                  )}
                </div>

                <div className="text-center px-4">
                  <h2 className="mb-1 fw-bold">{profile?.name} {profile?.surname}</h2>
                  <p className="text-secondary mb-3">{profile?.patronymic}</p>
                  
                  <div className="d-flex justify-content-center gap-4 flex-wrap mb-4">
                    <div className="d-flex align-items-center gap-2">
                      <i className="bi bi-gender-ambiguous" style={{ color: '#6c757d', fontSize: '1.1rem' }}></i>
                      <span className="text-secondary">{getGenderText(profile?.gender)}</span>
                    </div>
                    <div className="d-flex align-items-center gap-2">
                      <i className="bi bi-calendar3" style={{ color: '#6c757d', fontSize: '1.1rem' }}></i>
                      <span className="text-secondary">{profile?.birthDate}</span>
                    </div>
                  </div>
                </div>
              </div>

              <div className="row g-3 mb-4">
                <div className="col-4">
                  <div className="card border-0 shadow-sm rounded-4 h-100 text-center profile-card" style={{ animationDelay: '0.1s' }}>
                    <div className="card-body py-3">
                      <h3 className="mb-0 fw-bold" style={{ color: '#1e3c72' }}>{profile?.reviewsQuantity || 0}</h3>
                      <small className="text-secondary">Отзывов</small>
                    </div>
                  </div>
                </div>
                <div className="col-4">
                  <div className="card border-0 shadow-sm rounded-4 h-100 text-center profile-card" style={{ animationDelay: '0.2s' }}>
                    <div className="card-body py-3">
                      <h3 className="mb-0 fw-bold" style={{ color: '#1e3c72' }}>{profile?.followersQuantity || 0}</h3>
                      <small className="text-secondary">Подписчиков</small>
                    </div>
                  </div>
                </div>
                <div className="col-4">
                  <div className="card border-0 shadow-sm rounded-4 h-100 text-center profile-card" style={{ animationDelay: '0.3s' }}>
                    <div className="card-body py-3">
                      <h3 className="mb-0 fw-bold" style={{ color: '#1e3c72' }}>{profile?.followingsQuantity || 0}</h3>
                      <small className="text-secondary">Подписок</small>
                    </div>
                  </div>
                </div>
              </div>

              <div className="row g-3 mb-4">
                <div className="col-md-7">
                  <div className="card border-0 shadow-sm rounded-4 h-100 profile-card" style={{ animationDelay: '0.4s' }}>
                    <div className="card-body">
                      <h5 className="card-title fw-semibold mb-3">
                        <i className="bi bi-person-bounding-box me-2" style={{ color: '#1e3c72' }}></i>
                        О себе
                      </h5>
                      <p className="card-text text-secondary mb-0 lh-base">
                        {profile?.aboutInfo || 'Пользователь пока ничего не рассказал о себе'}
                      </p>
                    </div>
                  </div>
                </div>

                <div className="col-md-5">
                  <div className="card border-0 shadow-sm rounded-4 h-100 profile-card" style={{ animationDelay: '0.5s' }}>
                    <div className="card-body">
                      <h5 className="card-title fw-semibold mb-3">
                        <i className="bi bi-geo-alt me-2" style={{ color: '#1e3c72' }}></i>
                        Местоположение
                      </h5>
                      {profile?.location?.city ? (
                        <div className="text-secondary">
                          <p className="mb-1">
                            <span className="fw-semibold">Город:</span> {profile.location.city.name}
                          </p>
                          <p className="mb-0">
                            <span className="fw-semibold">Страна:</span> {profile.location.country?.ruName || 'Не указана'}
                          </p>
                        </div>
                      ) : (
                        <p className="text-secondary mb-0">Не указано</p>
                      )}
                    </div>
                  </div>
                </div>
              </div>

              <div className="text-center profile-card" style={{ animationDelay: '0.6s' }}>
                <button className="btn px-4 py-2 rounded-pill" style={{ backgroundColor: '#1e3c72', color: 'white' }}>
                  <i className="bi bi-pencil-square me-2"></i>
                  Редактировать профиль
                </button>
              </div>

            </div>
          </div>
        </div>
      </div>
    </>
  );
};

export default ProfilePage;