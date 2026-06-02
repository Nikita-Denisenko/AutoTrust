import { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import api from '../api/axiosConfig';

const PublicProfilePage = () => {
  const { userId } = useParams();
  const [profile, setProfile] = useState(null);
  const [listings, setListings] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [activeTab, setActiveTab] = useState('listings');
  const [listingType, setListingType] = useState('sale');
  const [isFollowing, setIsFollowing] = useState(false);
  const [followersCount, setFollowersCount] = useState(0);
  const [reviews, setReviews] = useState([]);
  const [showReviewModal, setShowReviewModal] = useState(false);
  const [reviewText, setReviewText] = useState('');
  const [reviewRating, setReviewRating] = useState(5);

  const getTypeText = (type) => {
    if (type === 0 || type === 'Sale') return 'Продажа';
    if (type === 1 || type === 'Buy') return 'Покупка';
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

  useEffect(() => {
    const fetchProfile = async () => {
      try {
        const response = await api.get(`/users/${userId}`);
        setProfile(response.data);
        setFollowersCount(response.data.followersQuantity || 0);
      } catch (err) {
        setError('Не удалось загрузить профиль');
      } finally {
        setLoading(false);
      }
    };
    fetchProfile();
    fetchFollowStatus();
    fetchReviews();
  }, [userId]);

  useEffect(() => {
    if (profile?.id) {
      fetchUserListings();
    }
  }, [activeTab, listingType, profile?.id]);

  const fetchFollowStatus = async () => {
    try {
      const response = await api.get(`/follows/check/${userId}`);
      setIsFollowing(response.data.isFollowing);
    } catch (err) {
      console.error('Ошибка проверки подписки', err);
    }
  };

  const fetchReviews = async () => {
    try {
      const response = await api.get(`/reviews/user/${userId}`);
      setReviews(response.data);
    } catch (err) {
      console.error('Ошибка загрузки отзывов', err);
    }
  };

  const fetchUserListings = async () => {
    try {
      const endpoint = listingType === 'sale' 
        ? `/listings/user/${userId}/sale` 
        : `/listings/user/${userId}/buy`;
      const response = await api.get(endpoint);
      setListings(response.data);
    } catch (err) {
      console.error('Ошибка загрузки объявлений', err);
    }
  };

  const handleFollow = async () => {
    try {
      if (isFollowing) {
        await api.delete(`/follows/${userId}`);
        setIsFollowing(false);
        setFollowersCount(prev => prev - 1);
      } else {
        await api.post(`/follows/${userId}`);
        setIsFollowing(true);
        setFollowersCount(prev => prev + 1);
      }
    } catch (err) {
      console.error('Ошибка подписки', err);
      alert('Не удалось изменить подписку');
    }
  };

  const handleAddReview = async () => {
    if (!reviewText.trim()) {
      alert('Введите текст отзыва');
      return;
    }

    try {
      await api.post('/reviews', {
        receiverId: parseInt(userId),
        rating: reviewRating,
        text: reviewText
      });
      setShowReviewModal(false);
      setReviewText('');
      setReviewRating(5);
      fetchReviews();
      alert('Отзыв добавлен');
    } catch (err) {
      console.error('Ошибка добавления отзыва', err);
      alert('Не удалось добавить отзыв');
    }
  };

  if (loading) return <div className="text-center mt-5">Загрузка...</div>;
  if (error) return <div className="alert alert-danger mt-5">{error}</div>;
  if (!profile) return null;

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

  const getRatingAverage = () => {
    if (reviews.length === 0) return 0;
    const sum = reviews.reduce((acc, r) => acc + r.rating, 0);
    return (sum / reviews.length).toFixed(1);
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
                  {profile?.patronymic && (
                    <p className="text-secondary mb-3">{profile.patronymic}</p>
                  )}

                  <div className="d-flex justify-content-center gap-4 flex-wrap mb-3">
                    <div className="d-flex align-items-center gap-2">
                      <i className="bi bi-gender-ambiguous" style={{ color: '#6c757d', fontSize: '1.1rem' }}></i>
                      <span className="text-secondary">{getGenderText(profile?.gender)}</span>
                    </div>
                    {profile?.birthDate && (
                      <div className="d-flex align-items-center gap-2">
                        <i className="bi bi-calendar3" style={{ color: '#6c757d', fontSize: '1.1rem' }}></i>
                        <span className="text-secondary">{profile.birthDate}</span>
                      </div>
                    )}
                  </div>

                  <div className="d-flex justify-content-center gap-3 mb-3">
                    <button 
                      className={`btn rounded-pill px-4 py-2 ${isFollowing ? 'btn-outline-secondary' : 'btn-primary'}`}
                      onClick={handleFollow}
                    >
                      <i className={`bi ${isFollowing ? 'bi-person-dash' : 'bi-person-plus'} me-2`}></i>
                      {isFollowing ? 'Отписаться' : 'Подписаться'}
                    </button>
                    <button 
                      className="btn btn-outline-primary rounded-pill px-4 py-2"
                      onClick={() => setShowReviewModal(true)}
                    >
                      <i className="bi bi-star me-2"></i>
                      Оставить отзыв
                    </button>
                  </div>
                </div>
              </div>

              <div className="row g-3 mb-4">
                <div className="col-4">
                  <div className="card border-0 shadow-sm rounded-4 h-100 text-center profile-card" style={{ animationDelay: '0.1s' }}>
                    <div className="card-body py-3">
                      <h3 className="mb-0 fw-bold" style={{ color: '#1e3c72' }}>{reviews.length}</h3>
                      <small className="text-secondary">Отзывов</small>
                    </div>
                  </div>
                </div>
                <div className="col-4">
                  <div className="card border-0 shadow-sm rounded-4 h-100 text-center profile-card" style={{ animationDelay: '0.2s' }}>
                    <div className="card-body py-3">
                      <h3 className="mb-0 fw-bold" style={{ color: '#1e3c72' }}>{getRatingAverage()}</h3>
                      <small className="text-secondary">Рейтинг</small>
                    </div>
                  </div>
                </div>
                <div className="col-4">
                  <div className="card border-0 shadow-sm rounded-4 h-100 text-center profile-card" style={{ animationDelay: '0.3s' }}>
                    <div className="card-body py-3">
                      <h3 className="mb-0 fw-bold" style={{ color: '#1e3c72' }}>{followersCount}</h3>
                      <small className="text-secondary">Подписчиков</small>
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
                    Объявления
                  </button>
                </li>
                <li className="nav-item">
                  <button
                    className={`btn rounded-pill px-4 ${activeTab === 'reviews' ? 'btn-primary' : 'btn-outline-primary'}`}
                    onClick={() => setActiveTab('reviews')}
                  >
                    Отзывы
                  </button>
                </li>
              </ul>

              <div className="card border-0 shadow-sm rounded-4 p-4 mb-4 profile-card">
                {activeTab === 'listings' ? (
                  <>
                    <div className="d-flex justify-content-center mb-4">
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

                    {listings.length > 0 ? (
                      <div className="row">
                        {listings.map(listing => {
                          const isSale = listingType === 'sale';
                          const imageUrl = isSale 
                            ? listing.car?.imageUrl 
                            : listing.brandImageUrl;
                          const priceDisplay = isSale 
                            ? `${listing.price?.toLocaleString()} ₽`
                            : `${listing.minPrice?.toLocaleString()} - ${listing.maxPrice?.toLocaleString()} ₽`;
                          const modelDisplay = isSale 
                            ? `${listing.car?.model?.brand?.name || ''} ${listing.car?.model?.name || ''}`.trim()
                            : listing.modelName;
                          const color = isSale ? listing.car?.color : listing.carColor;

                          return (
                            <div className="col-12 mb-3" key={listing.id}>
                              <div className="border rounded-3 p-4 h-100 listing-card">
                                <div className="d-flex flex-column flex-md-row gap-4">
                                  {imageUrl ? (
                                    <img 
                                      src={imageUrl} 
                                      alt={listing.name}
                                      style={{ width: '100%', maxWidth: '200px', height: '150px', objectFit: 'contain', backgroundColor: '#f8f9fa', borderRadius: '12px', padding: '8px' }}
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
                                      <span className={`badge ${isSale ? 'bg-success' : 'bg-info'} px-3 py-2 rounded-pill`}>
                                        {isSale ? 'Продажа' : 'Покупка'}
                                      </span>
                                    </div>
                                    <p className="text-muted mb-3" style={{ fontSize: '14px', lineHeight: '1.5' }}>
                                      {listing.description?.length > 200 ? `${listing.description.substring(0, 200)}...` : listing.description}
                                    </p>
                                    <div className="row">
                                      <div className="col-md-4">
                                        <p className="text-muted mb-2">
                                          <i className="bi bi-tag me-2"></i>
                                          Цена: <span className="fw-bold text-primary">{priceDisplay}</span>
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
                                          {listing.reactionsQuantity || 0}
                                        </p>
                                      </div>
                                    </div>
                                    {(modelDisplay || color) && (
                                      <div className="row mt-2 pt-2 border-top">
                                        {modelDisplay && (
                                          <div className="col-md-6">
                                            <small className="text-muted">
                                              <i className="bi bi-car-front me-1"></i>
                                              {modelDisplay}
                                            </small>
                                          </div>
                                        )}
                                        {color && (
                                          <div className="col-md-6">
                                            <small className="text-muted">
                                              <i className="bi bi-palette me-1"></i>
                                              {getColorName(color)}
                                            </small>
                                          </div>
                                        )}
                                      </div>
                                    )}
                                  </div>
                                </div>
                              </div>
                            </div>
                          );
                        })}
                      </div>
                    ) : (
                      <div className="text-center py-5">
                        <i className="bi bi-file-text fs-1 text-muted"></i>
                        <p className="text-muted mt-2 mb-0">У пользователя пока нет объявлений о {listingType === 'sale' ? 'продаже' : 'покупке'}</p>
                      </div>
                    )}
                  </>
                ) : (
                  <>
                    <h5 className="mb-3">Отзывы</h5>
                    {reviews.length > 0 ? (
                      <div className="d-flex flex-column gap-3">
                        {reviews.map(review => (
                          <div className="border rounded-3 p-3" key={review.id}>
                            <div className="d-flex justify-content-between mb-2">
                              <div className="d-flex gap-1">
                                {[...Array(5)].map((_, i) => (
                                  <i 
                                    key={i} 
                                    className={`bi bi-star${i < review.rating ? '-fill' : ''}`}
                                    style={{ color: i < review.rating ? '#ffc107' : '#dee2e6' }}
                                  ></i>
                                ))}
                              </div>
                              <small className="text-muted">
                                {new Date(review.createdAt).toLocaleDateString()}
                              </small>
                            </div>
                            <p className="mb-0">{review.text}</p>
                          </div>
                        ))}
                      </div>
                    ) : (
                      <div className="text-center py-5">
                        <i className="bi bi-chat-dots fs-1 text-muted"></i>
                        <p className="text-muted mt-2">Нет отзывов</p>
                      </div>
                    )}
                  </>
                )}
              </div>
            </div>
          </div>
        </div>
      </div>

      {/* Модалка добавления отзыва */}
      {showReviewModal && (
        <div className="modal show d-block" tabIndex="-1" style={{ backgroundColor: 'rgba(0,0,0,0.5)' }} onClick={() => setShowReviewModal(false)}>
          <div className="modal-dialog modal-dialog-centered" onClick={(e) => e.stopPropagation()}>
            <div className="modal-content rounded-4">
              <div className="modal-header border-0" style={{ background: 'linear-gradient(135deg, #1e3c72 0%, #2a5298 100%)', color: 'white' }}>
                <h5 className="modal-title fw-bold">Оставить отзыв</h5>
                <button type="button" className="btn-close btn-close-white" onClick={() => setShowReviewModal(false)}></button>
              </div>
              <div className="modal-body p-4">
                <div className="mb-3">
                  <label className="form-label fw-semibold">Оценка</label>
                  <div className="d-flex gap-2">
                    {[1, 2, 3, 4, 5].map(rating => (
                      <button
                        key={rating}
                        className="btn btn-outline-warning rounded-circle"
                        style={{ width: '45px', height: '45px' }}
                        onClick={() => setReviewRating(rating)}
                      >
                        {rating}
                      </button>
                    ))}
                  </div>
                </div>
                <div className="mb-3">
                  <label className="form-label fw-semibold">Отзыв</label>
                  <textarea
                    className="form-control"
                    rows="4"
                    placeholder="Расскажите о вашем опыте общения с пользователем..."
                    value={reviewText}
                    onChange={(e) => setReviewText(e.target.value)}
                  />
                </div>
              </div>
              <div className="modal-footer border-0 bg-light">
                <button className="btn btn-secondary rounded-pill px-4" onClick={() => setShowReviewModal(false)}>Отмена</button>
                <button className="btn btn-primary rounded-pill px-4" onClick={handleAddReview}>Отправить</button>
              </div>
            </div>
          </div>
        </div>
      )}
    </>
  );
};

export default PublicProfilePage;