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
  const [selectedCar, setSelectedCar] = useState(null);
  const [showCarModal, setShowCarModal] = useState(false);
  const [loadingCarDetail, setLoadingCarDetail] = useState(false);
  const [showAddCarModal, setShowAddCarModal] = useState(false);
  const [loadingAddCar, setLoadingAddCar] = useState(false);
  const [showDeleteConfirm, setShowDeleteConfirm] = useState(false);
  const [deletingId, setDeletingId] = useState(null);
  const [addCarForm, setAddCarForm] = useState({
    modelId: '',
    releaseYear: '',
    engineMileage: '',
    color: '',
    description: '',
    hasAccident: false,
    imageUrl: '',
    stateNumber: ''
  });
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

  const getCarTitle = (car) => {
    const brandName = car.model?.brand?.name || 'Неизвестный бренд';
    const modelName = car.model?.name || 'Неизвестная модель';
    return `${brandName} ${modelName}`;
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
      const response = await api.get('/cars');
      setCars(response.data);
    } catch (err) {
      console.error('Ошибка загрузки машин', err);
    }
  };

  const fetchCarDetail = async (carId) => {
    setLoadingCarDetail(true);
    try {
      const response = await api.get(`/cars/${carId}`);
      setSelectedCar(response.data);
    } catch (err) {
      console.error('Ошибка загрузки деталей машины', err);
      alert('Не удалось загрузить детальную информацию');
    } finally {
      setLoadingCarDetail(false);
    }
  };

  const handleCarClick = async (car) => {
    setShowCarModal(true);
    await fetchCarDetail(car.id);
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
  // Принудительно получаем значения из DOM, если стейт не обновился
  const cityIdValue = createForm.cityId || document.querySelector('select[name="cityId"]')?.value;
  const carIdValue = createForm.carId || document.querySelector('select[name="carId"]')?.value;
  const priceValue = createForm.price || document.querySelector('input[name="price"]')?.value;

  if (createType === 'sale' && !carIdValue) {
    alert('Выберите автомобиль');
    return;
  }
  if (!cityIdValue) {
    alert('Выберите город');
    return;
  }
  if (!priceValue || Number(priceValue) <= 0) {
    alert('Введите корректную цену');
    return;
  }

  try {
    const endpoint = createType === 'sale' ? '/listings/sale' : '/listings/buy';
    let payload;
    
    if (createType === 'sale') {
      payload = {
        name: createForm.name,
        description: createForm.description,
        price: Number(priceValue),
        cityId: Number(cityIdValue),
        carId: Number(carIdValue)
      };
    } else {
      payload = {
        name: createForm.name,
        description: createForm.description,
        cityId: Number(cityIdValue),
        modelId: Number(createForm.modelId),
        minPrice: createForm.minPrice ? Number(createForm.minPrice) : null,
        maxPrice: createForm.maxPrice ? Number(createForm.maxPrice) : null,
        minReleaseYear: createForm.minReleaseYear ? Number(createForm.minReleaseYear) : null,
        maxReleaseYear: createForm.maxReleaseYear ? Number(createForm.maxReleaseYear) : null,
        carColor: createForm.carColor || null
      };
    }
    
    console.log('Final payload:', payload);
    
    const response = await api.post(endpoint, payload);
    console.log('Response:', response);
    
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

  const handleDeleteListing = async (id) => {
    setDeletingId(id);
    setShowDeleteConfirm(true);
  };

  const confirmDelete = async () => {
    try {
      await api.delete(`/listings/${deletingId}`);
      setShowDeleteConfirm(false);
      setDeletingId(null);
      fetchUserListings();
    } catch (err) {
      console.error('Ошибка удаления', err);
      alert('Не удалось удалить объявление');
    }
  };

  const handleAddCar = async () => {
    if (!addCarForm.modelId || !addCarForm.releaseYear) {
      alert('Заполните обязательные поля (модель, год выпуска)');
      return;
    }

    setLoadingAddCar(true);
    try {
      const payload = {
        modelId: parseInt(addCarForm.modelId),
        releaseYear: parseInt(addCarForm.releaseYear),
        engineMileage: addCarForm.engineMileage ? parseFloat(addCarForm.engineMileage) : 0,
        color: addCarForm.color || null,
        description: addCarForm.description || null,
        hasAccident: addCarForm.hasAccident,
        imageUrl: addCarForm.imageUrl || null,
        stateNumber: addCarForm.stateNumber || null
      };
      
      await api.post('/cars', payload);
      setShowAddCarModal(false);
      fetchUserCars();
      setAddCarForm({
        modelId: '',
        releaseYear: '',
        engineMileage: '',
        color: '',
        description: '',
        hasAccident: false,
        imageUrl: '',
        stateNumber: ''
      });
      alert('Автомобиль успешно зарегистрирован');
    } catch (err) {
      console.error('Ошибка регистрации авто', err);
      alert('Не удалось зарегистрировать автомобиль');
    } finally {
      setLoadingAddCar(false);
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

  const getColorName = (color) => {
    const colors = {
      Red: 'Красный',
      Blue: 'Синий',
      Black: 'Чёрный',
      White: 'Белый',
      Silver: 'Серебристый',
      Gray: 'Серый',
      Green: 'Зелёный',
      Yellow: 'Жёлтый',
      Orange: 'Оранжевый',
      Brown: 'Коричневый',
      Beige: 'Бежевый',
      Gold: 'Золотой'
    };
    return colors[color] || color || 'Не указан';
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
    .car-card {
      cursor: pointer;
      transition: transform 0.2s ease, box-shadow 0.2s ease;
    }
    .car-card:hover {
      transform: translateY(-4px);
      box-shadow: 0 8px 25px rgba(0,0,0,0.1);
    }
    .listing-card {
      transition: transform 0.2s ease, box-shadow 0.2s ease;
    }
    .listing-card:hover {
      transform: translateY(-4px);
      box-shadow: 0 12px 30px rgba(0,0,0,0.12) !important;
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
            <div className="col-lg-10">
              
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
                    <div className="d-flex justify-content-between align-items-center mb-3">
                      <h5 className="mb-0">Мои объявления</h5>
                      <button
                        className="btn btn-primary rounded-pill px-4"
                        onClick={() => {
                          setCreateType(listingType);
                          setShowCreateModal(true);
                        }}
                      >
                        <i className="bi bi-plus-lg me-2"></i>
                        Создать объявление
                      </button>
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
                      <div className="row">
                        {listings.map(listing => (
                          <div className="col-12 mb-3" key={listing.id}>
                            <div className="border rounded-3 p-4 h-100 listing-card">
                              <div className="d-flex flex-column flex-md-row gap-4">
                                {listing.type === 0 && listing.car?.imageUrl ? (
                                  <img 
                                    src={listing.car.imageUrl} 
                                    alt={listing.name}
                                    style={{ width: '100%', maxWidth: '200px', height: '150px', objectFit: 'cover', borderRadius: '12px' }}
                                    onError={(e) => {
                                      e.target.onerror = null;
                                      e.target.src = 'https://placehold.co/200x150?text=No+Image';
                                    }}
                                  />
                                ) : (
                                  <div style={{ width: '100%', maxWidth: '200px', height: '150px', backgroundColor: '#f0f0f0', borderRadius: '12px', display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
                                    <i className="bi bi-car-front" style={{ fontSize: '48px', color: '#999' }}></i>
                                  </div>
                                )}
                                <div className="flex-grow-1">
                                  <div className="d-flex justify-content-between align-items-start flex-wrap gap-2 mb-2">
                                    <h5 className="mb-0 fw-bold">{listing.name}</h5>
                                    <span className={`badge ${listing.type === 0 ? 'bg-success' : 'bg-info'} px-3 py-2 rounded-pill`}>
                                      {listing.type === 0 ? 'Продажа' : 'Покупка'}
                                    </span>
                                  </div>
                                  <p className="text-muted mb-3" style={{ fontSize: '14px', lineHeight: '1.5' }}>
                                    {listing.description?.length > 200 ? `${listing.description.substring(0, 200)}...` : listing.description}
                                  </p>
                                  <div className="row">
                                    <div className="col-md-4">
                                      <p className="text-muted mb-2">
                                        <i className="bi bi-tag me-2"></i>
                                        Цена: <span className="fw-bold text-primary">{listing.price?.toLocaleString()} ₽</span>
                                      </p>
                                    </div>
                                    <div className="col-md-4">
                                      <p className="text-muted mb-2">
                                        <i className="bi bi-geo-alt me-2"></i>
                                        {listing.location?.city?.name || 'Город не указан'}
                                      </p>
                                    </div>
                                    <div className="col-md-4">
                                      <p className="text-muted mb-2">
                                        <i className="bi bi-heart me-2"></i>
                                        {listing.reactionsQuantity || 0} реакций
                                      </p>
                                    </div>
                                  </div>
                                  {listing.type === 0 && listing.car && (
                                    <div className="row mt-2 pt-2 border-top">
                                      <div className="col-md-3">
                                        <small className="text-muted">
                                          <i className="bi bi-calendar3 me-1"></i>
                                          {listing.car.releaseYear} г.
                                        </small>
                                      </div>
                                      <div className="col-md-3">
                                        <small className="text-muted">
                                          <i className="bi bi-speedometer2 me-1"></i>
                                          {listing.car.engineMileage?.toLocaleString()} км
                                        </small>
                                      </div>
                                      <div className="col-md-3">
                                        <small className="text-muted">
                                          <i className="bi bi-palette me-1"></i>
                                          {getColorName(listing.car.color)}
                                        </small>
                                      </div>
                                      <div className="col-md-3">
                                        <small className="text-muted">
                                          <i className="bi bi-person me-1"></i>
                                          {listing.car.ownershipsQuantity || 1} влад.
                                        </small>
                                      </div>
                                    </div>
                                  )}
                                  <div className="d-flex gap-2 mt-3">
                                    <button 
                                      className="btn btn-sm btn-outline-danger rounded-pill px-3"
                                      onClick={() => handleDeleteListing(listing.id)}
                                    >
                                      <i className="bi bi-trash me-1"></i>
                                      Удалить
                                    </button>
                                  </div>
                                </div>
                              </div>
                            </div>
                          </div>
                        ))}
                      </div>
                    ) : (
                      <div className="text-center py-5">
                        <i className="bi bi-file-text fs-1 text-muted"></i>
                        <p className="text-muted mt-2 mb-0">У вас пока нет объявлений о {listingType === 'sale' ? 'продаже' : 'покупке'}</p>
                      </div>
                    )}
                  </>
                ) : (
                  <>
                    <div className="d-flex justify-content-between align-items-center mb-3 flex-wrap gap-2">
                      <h5 className="mb-0">Мои автомобили</h5>
                      <button
                        className="btn btn-primary rounded-pill px-4 py-2"
                        onClick={() => setShowAddCarModal(true)}
                      >
                        <i className="bi bi-plus-lg me-2"></i>
                        Зарегистрировать автомобиль
                      </button>
                    </div>
                    {cars.length > 0 ? (
                      <div className="row">
                        {cars.map(car => (
                          <div className="col-12 mb-3" key={car.id}>
                            <div 
                              className="border rounded-3 p-4 h-100 car-card"
                              onClick={() => handleCarClick(car)}
                            >
                              <div className="d-flex flex-column flex-md-row gap-4">
                                {car.imageUrl && car.imageUrl !== '' ? (
                                  <img 
                                    src={car.imageUrl} 
                                    alt={getCarTitle(car)}
                                    style={{ width: '100%', maxWidth: '200px', height: '150px', objectFit: 'cover', borderRadius: '12px' }}
                                    onError={(e) => {
                                      e.target.onerror = null;
                                      e.target.src = 'https://placehold.co/200x150?text=No+Image';
                                    }}
                                  />
                                ) : (
                                  <div style={{ width: '100%', maxWidth: '200px', height: '150px', backgroundColor: '#f0f0f0', borderRadius: '12px', display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
                                    <i className="bi bi-car-front" style={{ fontSize: '64px', color: '#999' }}></i>
                                  </div>
                                )}
                                <div className="flex-grow-1">
                                  <h5 className="mb-2 fw-bold">{getCarTitle(car)}</h5>
                                  <div className="row">
                                    <div className="col-md-6">
                                      <p className="text-muted mb-2">
                                        <i className="bi bi-calendar3 me-2"></i>
                                        {car.releaseYear} г.
                                      </p>
                                      <p className="text-muted mb-2">
                                        <i className="bi bi-palette me-2"></i>
                                        Цвет: {getColorName(car.color)}
                                      </p>
                                    </div>
                                    <div className="col-md-6">
                                      <p className="text-muted mb-2">
                                        <i className="bi bi-tag me-2"></i>
                                        Статус: {car.inSale ? 'В продаже' : 'Не продаётся'}
                                      </p>
                                    </div>
                                  </div>
                                </div>
                              </div>
                            </div>
                          </div>
                        ))}
                      </div>
                    ) : (
                      <div className="text-center py-5">
                        <i className="bi bi-car-front fs-1 text-muted"></i>
                        <p className="text-muted mt-2 mb-3">У вас пока нет зарегистрированных автомобилей</p>
                      </div>
                    )}
                  </>
                )}
              </div>

            </div>
          </div>
        </div>
      </div>

      {/* Модалка подтверждения удаления */}
      {showDeleteConfirm && (
        <div className="modal show d-block" tabIndex="-1" style={{ backgroundColor: 'rgba(0,0,0,0.5)' }} onClick={() => setShowDeleteConfirm(false)}>
          <div className="modal-dialog modal-dialog-centered" onClick={(e) => e.stopPropagation()}>
            <div className="modal-content rounded-4">
              <div className="modal-header border-0">
                <h5 className="modal-title">Подтверждение удаления</h5>
                <button type="button" className="btn-close" onClick={() => setShowDeleteConfirm(false)}></button>
              </div>
              <div className="modal-body">
                <p>Вы уверены, что хотите удалить это объявление?</p>
              </div>
              <div className="modal-footer border-0">
                <button className="btn btn-secondary rounded-pill px-4" onClick={() => setShowDeleteConfirm(false)}>Отмена</button>
                <button className="btn btn-danger rounded-pill px-4" onClick={confirmDelete}>Удалить</button>
              </div>
            </div>
          </div>
        </div>
      )}

      {/* Модалка деталки автомобиля */}
      {showCarModal && (
        <div className="modal show d-block" tabIndex="-1" style={{ backgroundColor: 'rgba(0,0,0,0.8)' }} onClick={() => setShowCarModal(false)}>
          <div className="modal-dialog modal-dialog-centered modal-xl" style={{ maxWidth: '1000px' }} onClick={(e) => e.stopPropagation()}>
            <div className="modal-content rounded-4 overflow-hidden">
              <div style={{ 
                background: 'linear-gradient(135deg, #1e3c72 0%, #2a5298 100%)', 
                padding: '20px 24px',
                display: 'flex',
                justifyContent: 'space-between',
                alignItems: 'center'
              }}>
                <h4 className="fw-bold mb-0" style={{ color: 'white' }}>
                  {selectedCar ? getCarTitle(selectedCar) : 'Загрузка...'}
                </h4>
                <button 
                  type="button" 
                  className="btn-close btn-close-white" 
                  onClick={() => setShowCarModal(false)}
                  style={{ opacity: 0.8 }}
                ></button>
              </div>

              <div className="modal-body p-0">
                {loadingCarDetail ? (
                  <div className="text-center py-5">
                    <div className="spinner-border text-primary" role="status"></div>
                  </div>
                ) : selectedCar ? (
                  <div className="row g-0">
                    <div className="col-md-6" style={{ background: '#f8f9fa', minHeight: '400px' }}>
                      <div className="p-4 d-flex align-items-center justify-content-center h-100">
                        {selectedCar.imageUrl && selectedCar.imageUrl !== '' ? (
                          <img 
                            src={selectedCar.imageUrl} 
                            alt={getCarTitle(selectedCar)}
                            className="img-fluid rounded-3"
                            style={{ maxHeight: '400px', width: '100%', objectFit: 'cover', borderRadius: '16px' }}
                            onError={(e) => {
                              e.target.onerror = null;
                              e.target.src = 'https://placehold.co/600x400?text=No+Image';
                            }}
                          />
                        ) : (
                          <div style={{ width: '100%', height: '300px', backgroundColor: '#e9ecef', borderRadius: '16px', display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
                            <i className="bi bi-car-front" style={{ fontSize: '80px', color: '#adb5bd' }}></i>
                          </div>
                        )}
                      </div>
                    </div>
                    <div className="col-md-6">
                      <div className="p-4">
                        <div className="mb-4">
                          <h6 className="text-muted mb-2" style={{ fontSize: '14px', letterSpacing: '0.5px' }}>ОПИСАНИЕ</h6>
                          <p className="mb-0" style={{ fontSize: '16px', lineHeight: '1.5' }}>
                            {selectedCar.description || 'Нет описания'}
                          </p>
                        </div>

                        <hr className="my-4" />

                        <div className="row">
                          <div className="col-6 mb-4">
                            <h6 className="text-muted mb-2" style={{ fontSize: '12px' }}>Год выпуска</h6>
                            <p className="mb-0 fw-bold fs-5">{selectedCar.releaseYear}</p>
                          </div>
                          <div className="col-6 mb-4">
                            <h6 className="text-muted mb-2" style={{ fontSize: '12px' }}>Пробег</h6>
                            <p className="mb-0 fw-bold fs-5">{selectedCar.engineMileage?.toLocaleString()} км</p>
                          </div>
                          <div className="col-6 mb-4">
                            <h6 className="text-muted mb-2" style={{ fontSize: '12px' }}>Цвет</h6>
                            <p className="mb-0 fw-bold fs-5">{getColorName(selectedCar.color)}</p>
                          </div>
                          <div className="col-6 mb-4">
                            <h6 className="text-muted mb-2" style={{ fontSize: '12px' }}>Госномер</h6>
                            <p className="mb-0 fw-bold fs-5">{selectedCar.stateNumber || 'Не указан'}</p>
                          </div>
                          <div className="col-6 mb-4">
                            <h6 className="text-muted mb-2" style={{ fontSize: '12px' }}>Владельцев</h6>
                            <p className="mb-0 fw-bold fs-5">{selectedCar.ownershipsQuantity || 1}</p>
                          </div>
                          <div className="col-6 mb-4">
                            <h6 className="text-muted mb-2" style={{ fontSize: '12px' }}>Статус</h6>
                            <span className={`badge ${selectedCar.inSale ? 'bg-success' : 'bg-secondary'} fs-6 px-3 py-2 rounded-pill`}>
                              {selectedCar.inSale ? 'В продаже' : 'Не продаётся'}
                            </span>
                          </div>
                          <div className="col-12 mb-3">
                            <h6 className="text-muted mb-2" style={{ fontSize: '12px' }}>Битость</h6>
                            <span className={`badge ${selectedCar.hasAccident ? 'bg-danger' : 'bg-success'} fs-6 px-3 py-2 rounded-pill`}>
                              {selectedCar.hasAccident ? 'Была в ДТП' : 'Без ДТП'}
                            </span>
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                ) : null}
              </div>
            </div>
          </div>
        </div>
      )}

      {/* Модалка регистрации авто */}
      {showAddCarModal && (
        <div className="modal show d-block" tabIndex="-1" style={{ backgroundColor: 'rgba(0,0,0,0.5)' }} onClick={() => setShowAddCarModal(false)}>
          <div className="modal-dialog modal-dialog-centered modal-lg" onClick={(e) => e.stopPropagation()}>
            <div className="modal-content rounded-4">
              <div className="modal-header border-0" style={{ background: 'linear-gradient(135deg, #1e3c72 0%, #2a5298 100%)', color: 'white' }}>
                <h5 className="modal-title fw-bold">Зарегистрировать автомобиль</h5>
                <button type="button" className="btn-close btn-close-white" onClick={() => setShowAddCarModal(false)}></button>
              </div>
              <div className="modal-body p-4">
                <div className="row g-3">
                  <div className="col-md-6">
                    <label className="form-label fw-semibold">Модель *</label>
                    <select
                      className="form-select"
                      value={addCarForm.modelId}
                      onChange={(e) => setAddCarForm({ ...addCarForm, modelId: e.target.value })}
                    >
                      <option value="">Выберите модель</option>
                      {models.map(model => (
                        <option key={model.id} value={model.id}>
                          {getModelDisplayName(model)}
                        </option>
                      ))}
                    </select>
                  </div>
                  <div className="col-md-6">
                    <label className="form-label fw-semibold">Год выпуска *</label>
                    <input
                      type="number"
                      className="form-control"
                      min="1900"
                      max={new Date().getFullYear()}
                      value={addCarForm.releaseYear}
                      onChange={(e) => setAddCarForm({ ...addCarForm, releaseYear: e.target.value })}
                    />
                  </div>
                  <div className="col-md-6">
                    <label className="form-label fw-semibold">Госномер</label>
                    <input
                      type="text"
                      className="form-control text-uppercase"
                      placeholder="A123BC"
                      value={addCarForm.stateNumber}
                      onChange={(e) => setAddCarForm({ ...addCarForm, stateNumber: e.target.value.toUpperCase() })}
                    />
                  </div>
                  <div className="col-md-6">
                    <label className="form-label fw-semibold">Пробег (км)</label>
                    <input
                      type="number"
                      className="form-control"
                      min="0"
                      value={addCarForm.engineMileage}
                      onChange={(e) => setAddCarForm({ ...addCarForm, engineMileage: e.target.value })}
                    />
                  </div>
                  <div className="col-md-6">
                    <label className="form-label fw-semibold">Цвет</label>
                    <select
                      className="form-select"
                      value={addCarForm.color}
                      onChange={(e) => setAddCarForm({ ...addCarForm, color: e.target.value })}
                    >
                      <option value="">Выберите цвет</option>
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
                    </select>
                  </div>
                  <div className="col-md-6">
                    <label className="form-label fw-semibold">Фото (URL)</label>
                    <input
                      type="text"
                      className="form-control"
                      placeholder="https://..."
                      value={addCarForm.imageUrl}
                      onChange={(e) => setAddCarForm({ ...addCarForm, imageUrl: e.target.value })}
                    />
                  </div>
                  <div className="col-12">
                    <label className="form-label fw-semibold">Описание</label>
                    <textarea
                      className="form-control"
                      rows="3"
                      placeholder="Дополнительная информация об автомобиле..."
                      value={addCarForm.description}
                      onChange={(e) => setAddCarForm({ ...addCarForm, description: e.target.value })}
                    />
                  </div>
                  <div className="col-12">
                    <div className="form-check">
                      <input
                        type="checkbox"
                        className="form-check-input"
                        id="hasAccident"
                        checked={addCarForm.hasAccident}
                        onChange={(e) => setAddCarForm({ ...addCarForm, hasAccident: e.target.checked })}
                      />
                      <label className="form-check-label" htmlFor="hasAccident">
                        Автомобиль был в ДТП
                      </label>
                    </div>
                  </div>
                </div>
              </div>
              <div className="modal-footer border-0 bg-light">
                <button className="btn btn-secondary rounded-pill px-4" onClick={() => setShowAddCarModal(false)}>Отмена</button>
                <button 
                  className="btn btn-primary rounded-pill px-4" 
                  onClick={handleAddCar}
                  disabled={loadingAddCar || !addCarForm.modelId || !addCarForm.releaseYear}
                >
                  {loadingAddCar ? 'Сохранение...' : 'Зарегистрировать'}
                </button>
              </div>
            </div>
          </div>
        </div>
      )}

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
                    step="1000"
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
                          {getCarTitle(car)} ({car.releaseYear})
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
                          max={new Date().getFullYear()}
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
                          max={new Date().getFullYear()}
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