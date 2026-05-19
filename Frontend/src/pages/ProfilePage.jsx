import { useState, useEffect } from 'react';
import { useAuth } from '../contexts/AuthContext';
import api from '../api/axiosConfig';

const ProfilePage = () => {
  const { user } = useAuth();
  const [profile, setProfile] = useState(null);
  const [listings, setListings] = useState([]);
  const [cars, setCars] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [activeTab, setActiveTab] = useState('listings');
  const [listingType, setListingType] = useState('sale');
  const [loadingListings, setLoadingListings] = useState(false);
  const [showEditModal, setShowEditModal] = useState(false);
  const [uploadingAvatar, setUploadingAvatar] = useState(false);
  const [editForm, setEditForm] = useState({ aboutInfo: '' });
  const [showCreateModal, setShowCreateModal] = useState(false);
  const [createType, setCreateType] = useState('sale');
  const [cities, setCities] = useState([]);
  const [models, setModels] = useState([]);
  const [createForm, setCreateForm] = useState({
    name: '',
    description: '',
    price: '',
    cityId: '',
    carId: '',
    modelId: '',
    minPrice: '',
    maxPrice: '',
    minReleaseYear: '',
    maxReleaseYear: '',
    carColor: ''
  });

  const getModelDisplayName = (model) => {
    return `${model.brand?.name || 'Неизвестный бренд'} ${model.name}`;
  };

  useEffect(() => {
    const fetchProfile = async () => {
      try {
        const response = await api.get('/users/me');
        setProfile(response.data);
        setEditForm({ aboutInfo: response.data.aboutInfo || '' });
      } catch (err) {
        setError('Не удалось загрузить профиль');
      } finally {
        setLoading(false);
      }
    };
    fetchProfile();
    fetchCities();
    fetchModels();
  }, []);

  useEffect(() => {
    if (activeTab === 'listings' && profile?.id) {
      fetchUserListings();
    }
  }, [activeTab, listingType, profile?.id]);

  useEffect(() => {
    if (activeTab === 'cars') {
      fetchUserCars();
    }
  }, [activeTab]);

  const fetchCities = async () => {
    try {
      const response = await api.get('/cities?Size=500');
      setCities(response.data);
    } catch (err) {
      console.error('Ошибка загрузки городов', err);
    }
  };

  const fetchModels = async () => {
    try {
      const response = await api.get('/models');
      setModels(response.data);
    } catch (err) {
      console.error('Ошибка загрузки моделей', err);
    }
  };

  const fetchUserListings = async () => {
    if (!profile?.id) return;
    setLoadingListings(true);
    try {
      const endpoint = listingType === 'sale' 
        ? `/listings/user/${profile.id}/sale` 
        : `/listings/user/${profile.id}/buy`;
      const response = await api.get(endpoint);
      setListings(response.data);
    } catch (err) {
      console.error('Ошибка загрузки объявлений', err);
    } finally {
      setLoadingListings(false);
    }
  };

  const fetchUserCars = async () => {
    try {
      const response = await api.get('/users/me/cars');
      setCars(response.data);
    } catch (err) {
      console.error('Ошибка загрузки машин', err);
    }
  };

  const handleSaveProfile = async () => {
    try {
      await api.patch('/users/me/info', { aboutInfo: editForm.aboutInfo });
      setProfile({ ...profile, aboutInfo: editForm.aboutInfo });
      setShowEditModal(false);
    } catch (err) {
      console.error('Ошибка сохранения', err);
      alert('Не удалось сохранить');
    }
  };

  const handleAvatarUpload = async (e) => {
    alert('Загрузка аватара временно недоступна');
    return;
  };

  const handleCreateListing = async () => {
    try {
      const endpoint = createType === 'sale' ? '/listings/sale' : '/listings/buy';
      let payload;
      
      if (createType === 'sale') {
        payload = {
          name: createForm.name,
          description: createForm.description,
          price: parseFloat(createForm.price),
          cityId: parseInt(createForm.cityId),
          carId: parseInt(createForm.carId)
        };
      } else {
        payload = {
          name: createForm.name,
          description: createForm.description,
          cityId: parseInt(createForm.cityId),
          modelId: parseInt(createForm.modelId),
          minPrice: createForm.minPrice ? parseFloat(createForm.minPrice) : null,
          maxPrice: createForm.maxPrice ? parseFloat(createForm.maxPrice) : null,
          minReleaseYear: createForm.minReleaseYear ? parseInt(createForm.minReleaseYear) : null,
          maxReleaseYear: createForm.maxReleaseYear ? parseInt(createForm.maxReleaseYear) : null,
          carColor: createForm.carColor || null
        };
      }
      
      await api.post(endpoint, payload);
      setShowCreateModal(false);
      fetchUserListings();
      setCreateForm({
        name: '',
        description: '',
        price: '',
        cityId: '',
        carId: '',
        modelId: '',
        minPrice: '',
        maxPrice: '',
        minReleaseYear: '',
        maxReleaseYear: '',
        carColor: ''
      });
    } catch (err) {
      console.error('Ошибка создания объявления', err);
      alert('Не удалось создать объявление');
    }
  };

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
      margin-top: 0;
      padding-top: 0;
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
      <div className="profile-page-bg py-5">
        <div className="container">
          <div className="row justify-content-center">
            <div className="col-lg-8">
              
              <div className="card border-0 shadow-sm rounded-4 overflow-hidden mb-4 profile-card">
                <div style={{ background: 'linear-gradient(135deg, #1e3c72 0%, #2a5298 100%)', height: '100px' }}></div>
                
                <div className="text-center position-relative" style={{ marginTop: '-50px', marginBottom: '16px' }}>
                  <label 
                    htmlFor="avatar-upload" 
                    className="position-relative d-inline-block"
                    style={{ cursor: 'pointer' }}
                  >
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
                    <div className="position-absolute top-0 start-0 w-100 h-100 rounded-circle d-flex align-items-center justify-content-center" 
                      style={{ background: 'rgba(0,0,0,0.5)', opacity: 0, transition: 'opacity 0.3s', cursor: 'pointer', borderRadius: '50%' }}
                      onMouseEnter={(e) => e.currentTarget.style.opacity = 1}
                      onMouseLeave={(e) => e.currentTarget.style.opacity = 0}
                    >
                      <i className="bi bi-camera-fill text-white" style={{ fontSize: '24px' }}></i>
                    </div>
                  </label>
                  <input
                    type="file"
                    id="avatar-upload"
                    accept="image/*"
                    style={{ display: 'none' }}
                    onChange={handleAvatarUpload}
                    disabled={uploadingAvatar}
                  />
                </div>

                <div className="text-center px-4">
                  <h2 className="mb-1 fw-bold">{profile?.name} {profile?.surname}</h2>
                  <p className="text-secondary mb-3">{profile?.patronymic}</p>

                  <div className="d-flex justify-content-center gap-4 flex-wrap mb-3">
                    <div className="d-flex align-items-center gap-2">
                      <i className="bi bi-gender-ambiguous" style={{ color: '#6c757d', fontSize: '1.1rem' }}></i>
                      <span className="text-secondary">{getGenderText(profile?.gender)}</span>
                    </div>
                    <div className="d-flex align-items-center gap-2">
                      <i className="bi bi-calendar3" style={{ color: '#6c757d', fontSize: '1.1rem' }}></i>
                      <span className="text-secondary">{profile?.birthDate}</span>
                    </div>
                  </div>

                  <button 
                    className="btn px-4 py-2 rounded-pill fw-semibold mb-3"
                    style={{ 
                      backgroundColor: '#1e3c72',
                      color: 'white',
                      border: 'none',
                      fontSize: '14px',
                      transition: 'all 0.2s ease'
                    }}
                    onClick={() => setShowEditModal(true)}
                    onMouseEnter={(e) => {
                      e.currentTarget.style.backgroundColor = '#2a5298';
                      e.currentTarget.style.transform = 'translateY(-2px)';
                      e.currentTarget.style.boxShadow = '0 4px 12px rgba(0,0,0,0.15)';
                    }}
                    onMouseLeave={(e) => {
                      e.currentTarget.style.backgroundColor = '#1e3c72';
                      e.currentTarget.style.transform = 'translateY(0)';
                      e.currentTarget.style.boxShadow = 'none';
                    }}
                  >
                    <i className="bi bi-pencil-square me-2"></i>
                    Редактировать профиль
                  </button>
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
                      <h5 className="card-title fw-semibold mb-3" style={{ color: '#1e3c72' }}>
                        <i className="bi bi-person-bounding-box me-2"></i>
                        О себе
                      </h5>
                      <p className="card-text mb-0 lh-base" style={{ color: '#333' }}>
                        {profile?.aboutInfo || 'Пользователь пока ничего не рассказал о себе'}
                      </p>
                    </div>
                  </div>
                </div>
                <div className="col-md-5">
                  <div className="card border-0 shadow-sm rounded-4 h-100 profile-card" style={{ animationDelay: '0.5s' }}>
                    <div className="card-body">
                      <h5 className="card-title fw-semibold mb-3" style={{ color: '#1e3c72' }}>
                        <i className="bi bi-geo-alt me-2"></i>
                        Местоположение
                      </h5>
                      {profile?.location?.city ? (
                        <div style={{ color: '#333' }}>
                          <p className="mb-1">
                            <span className="fw-semibold">Город:</span> {profile.location.city.name}
                          </p>
                          <p className="mb-0">
                            <span className="fw-semibold">Страна:</span> {profile.location.country?.ruName || 'Не указана'}
                          </p>
                        </div>
                      ) : (
                        <p style={{ color: '#666' }}>Не указано</p>
                      )}
                    </div>
                  </div>
                </div>
              </div>

              <ul className="nav nav-tabs mb-4 justify-content-center border-0 gap-2">
                <li className="nav-item">
                  <button
                    className={`btn rounded-pill px-4 ${activeTab === 'listings' ? 'btn-primary' : 'btn-outline-primary'}`}
                    onClick={() => setActiveTab('listings')}
                  >
                    Мои объявления
                  </button>
                </li>
                <li className="nav-item">
                  <button
                    className={`btn rounded-pill px-4 ${activeTab === 'cars' ? 'btn-primary' : 'btn-outline-primary'}`}
                    onClick={() => setActiveTab('cars')}
                  >
                    Мои автомобили
                  </button>
                </li>
              </ul>

              <div className="card border-0 shadow-sm rounded-4 p-4 mb-4 profile-card">
                {activeTab === 'listings' ? (
                  <>
                    <div className="d-flex justify-content-between align-items-center mb-3 flex-wrap gap-2">
                      <h5 className="mb-0">Мои объявления</h5>
                      <div className="btn-group" role="group">
                        <button
                          className={`btn btn-sm ${listingType === 'sale' ? 'btn-primary' : 'btn-outline-primary'}`}
                          onClick={() => setListingType('sale')}
                        >
                          Продажа
                        </button>
                        <button
                          className={`btn btn-sm ${listingType === 'buy' ? 'btn-primary' : 'btn-outline-primary'}`}
                          onClick={() => setListingType('buy')}
                        >
                          Покупка
                        </button>
                      </div>
                    </div>

                    {loadingListings ? (
                      <div className="text-center py-5">
                        <div className="spinner-border text-primary" role="status"></div>
                      </div>
                    ) : listings.length > 0 ? (
                      <div className="row g-3">
                        {listings.map(listing => (
                          <div className="col-md-6" key={listing.id}>
                            <div className="border rounded-3 p-3 h-100">
                              <div className="d-flex justify-content-between align-items-start mb-2">
                                <h6 className="mb-0">{listing.name}</h6>
                                <span className={`badge ${listing.type === 'sale' ? 'bg-success' : 'bg-info'}`}>
                                  {listing.type === 'sale' ? 'Продажа' : 'Покупка'}
                                </span>
                              </div>
                              <p className="text-muted mb-1">{listing.price} ₽</p>
                              <small className="text-secondary">{listing.createdAt}</small>
                            </div>
                          </div>
                        ))}
                      </div>
                    ) : (
                      <div className="text-center py-5">
                        <i className="bi bi-file-text fs-1 text-muted"></i>
                        <p className="text-muted mt-2 mb-3">У вас пока нет объявлений о {listingType === 'sale' ? 'продаже' : 'покупке'}</p>
                        <button
                          className="btn btn-primary px-4 py-2 rounded-pill"
                          onClick={() => {
                            setCreateType(listingType);
                            setShowCreateModal(true);
                          }}
                        >
                          <i className="bi bi-plus-lg me-2"></i>
                          Создать объявление
                        </button>
                      </div>
                    )}
                  </>
                ) : (
                  <>
                    <div className="d-flex justify-content-between align-items-center mb-3">
                      <h5 className="mb-0">Мои автомобили</h5>
                    </div>
                    {cars.length > 0 ? (
                      <div className="row g-3">
                        {cars.map(car => (
                          <div className="col-md-6" key={car.id}>
                            <div className="border rounded-3 p-3">
                              <h6 className="mb-1">{car.brand} {car.model}</h6>
                              <p className="text-muted mb-1">{car.year} г.</p>
                              <small className="text-secondary">Госномер: {car.stateNumber}</small>
                            </div>
                          </div>
                        ))}
                      </div>
                    ) : (
                      <div className="text-center py-5">
                        <i className="bi bi-car-front fs-1 text-muted"></i>
                        <p className="text-muted mt-2 mb-0">У вас пока нет зарегистрированных автомобилей</p>
                      </div>
                    )}
                  </>
                )}
              </div>

            </div>
          </div>
        </div>
      </div>

      {showEditModal && (
        <div className="modal show d-block" tabIndex="-1" style={{ backgroundColor: 'rgba(0,0,0,0.5)' }}>
          <div className="modal-dialog modal-dialog-centered">
            <div className="modal-content rounded-4">
              <div className="modal-header border-0">
                <h5 className="modal-title">Редактировать профиль</h5>
                <button type="button" className="btn-close" onClick={() => setShowEditModal(false)}></button>
              </div>
              <div className="modal-body">
                <div className="mb-3">
                  <label className="form-label">О себе</label>
                  <textarea
                    className="form-control"
                    rows="4"
                    placeholder="Расскажите о себе..."
                    value={editForm.aboutInfo}
                    onChange={(e) => setEditForm({ ...editForm, aboutInfo: e.target.value })}
                  />
                </div>
                <div className="alert alert-info mt-3" role="alert" style={{ fontSize: '14px' }}>
                  <i className="bi bi-info-circle-fill me-2"></i>
                  Изменение имени, фамилии, города и других персональных данных доступно через обращение в поддержку.
                </div>
              </div>
              <div className="modal-footer border-0">
                <button className="btn btn-secondary" onClick={() => setShowEditModal(false)}>Отмена</button>
                <button className="btn btn-primary" onClick={handleSaveProfile}>Сохранить</button>
              </div>
            </div>
          </div>
        </div>
      )}

      {showCreateModal && (
        <div className="modal show d-block" tabIndex="-1" style={{ backgroundColor: 'rgba(0,0,0,0.5)' }}>
          <div className="modal-dialog modal-dialog-centered modal-lg">
            <div className="modal-content rounded-4">
              <div className="modal-header border-0">
                <h5 className="modal-title">Создать объявление о {createType === 'sale' ? 'продаже' : 'покупке'}</h5>
                <button type="button" className="btn-close" onClick={() => setShowCreateModal(false)}></button>
              </div>
              <div className="modal-body">
                <div className="mb-3">
                  <label className="form-label">Название</label>
                  <input
                    type="text"
                    className="form-control"
                    maxLength={40}
                    value={createForm.name}
                    onChange={(e) => setCreateForm({ ...createForm, name: e.target.value })}
                  />
                </div>
                <div className="mb-3">
                  <label className="form-label">Описание</label>
                  <textarea
                    className="form-control"
                    rows="3"
                    maxLength={4500}
                    value={createForm.description}
                    onChange={(e) => setCreateForm({ ...createForm, description: e.target.value })}
                  />
                </div>
                <div className="mb-3">
                  <label className="form-label">Цена (₽)</label>
                  <input
                    type="number"
                    className="form-control"
                    min="0"
                    value={createForm.price}
                    onChange={(e) => setCreateForm({ ...createForm, price: e.target.value })}
                  />
                </div>
                <div className="mb-3">
                  <label className="form-label">Город</label>
                  <select
                    className="form-select"
                    value={createForm.cityId}
                    onChange={(e) => setCreateForm({ ...createForm, cityId: e.target.value })}
                  >
                    <option value="">Выберите город</option>
                    {cities.map(city => (
                      <option key={city.id} value={city.id}>
                        {city.name}
                      </option>
                    ))}
                  </select>
                </div>

                {createType === 'sale' ? (
                  <div className="mb-3">
                    <label className="form-label">Автомобиль</label>
                    <select
                      className="form-select"
                      value={createForm.carId}
                      onChange={(e) => setCreateForm({ ...createForm, carId: e.target.value })}
                    >
                      <option value="">Выберите автомобиль</option>
                      {cars.map(car => (
                        <option key={car.id} value={car.id}>
                          {car.brand} {car.model} ({car.year})
                        </option>
                      ))}
                    </select>
                  </div>
                ) : (
                  <>
                    <div className="mb-3">
                      <label className="form-label">Модель</label>
                      <select
                        className="form-select"
                        value={createForm.modelId}
                        onChange={(e) => setCreateForm({ ...createForm, modelId: e.target.value })}
                      >
                        <option value="">Выберите модель</option>
                        {models.map(model => (
                          <option key={model.id} value={model.id}>
                            {getModelDisplayName(model)}
                          </option>
                        ))}
                      </select>
                    </div>
                    <div className="row">
                      <div className="col-md-6 mb-3">
                        <label className="form-label">Мин. цена (₽)</label>
                        <input
                          type="number"
                          className="form-control"
                          min="0"
                          value={createForm.minPrice}
                          onChange={(e) => setCreateForm({ ...createForm, minPrice: e.target.value })}
                        />
                      </div>
                      <div className="col-md-6 mb-3">
                        <label className="form-label">Макс. цена (₽)</label>
                        <input
                          type="number"
                          className="form-control"
                          min="0"
                          value={createForm.maxPrice}
                          onChange={(e) => setCreateForm({ ...createForm, maxPrice: e.target.value })}
                        />
                      </div>
                    </div>
                    <div className="row">
                      <div className="col-md-6 mb-3">
                        <label className="form-label">Мин. год выпуска</label>
                        <input
                          type="number"
                          className="form-control"
                          min="1900"
                          value={createForm.minReleaseYear}
                          onChange={(e) => setCreateForm({ ...createForm, minReleaseYear: e.target.value })}
                        />
                      </div>
                      <div className="col-md-6 mb-3">
                        <label className="form-label">Макс. год выпуска</label>
                        <input
                          type="number"
                          className="form-control"
                          min="1900"
                          value={createForm.maxReleaseYear}
                          onChange={(e) => setCreateForm({ ...createForm, maxReleaseYear: e.target.value })}
                        />
                      </div>
                    </div>
                    <div className="mb-3">
                      <label className="form-label">Цвет</label>
                      <select
                        className="form-select"
                        value={createForm.carColor}
                        onChange={(e) => setCreateForm({ ...createForm, carColor: e.target.value })}
                      >
                        <option value="">Любой</option>
                        <option value="Red">Красный</option>
                        <option value="Blue">Синий</option>
                        <option value="Black">Чёрный</option>
                        <option value="White">Белый</option>
                        <option value="Silver">Серебристый</option>
                        <option value="Gray">Серый</option>
                        <option value="Green">Зелёный</option>
                        <option value="Yellow">Жёлтый</option>
                        <option value="Orange">Оранжевый</option>
                        <option value="Brown">Коричневый</option>
                        <option value="Beige">Бежевый</option>
                        <option value="Gold">Золотой</option>
                      </select>
                    </div>
                  </>
                )}
              </div>
              <div className="modal-footer border-0">
                <button className="btn btn-secondary" onClick={() => setShowCreateModal(false)}>Отмена</button>
                <button className="btn btn-primary" onClick={handleCreateListing}>Создать</button>
              </div>
            </div>
          </div>
        </div>
      )}
    </>
  );
};

export default ProfilePage;