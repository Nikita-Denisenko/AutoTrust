import { useState, useEffect, useCallback, useRef } from 'react';
import api from '../api/axiosConfig';

const FeedPage = () => {
  const [listings, setListings] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [cityId, setCityId] = useState('');
  const [cities, setCities] = useState([]);
  const [page, setPage] = useState(1);
  const [hasMore, setHasMore] = useState(true);
  const [loadingMore, setLoadingMore] = useState(false);
  const observerRef = useRef();

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

  const formatMileage = (mileage) => {
    if (!mileage && mileage !== 0) return '0';
    return mileage.toLocaleString();
  };

  const formatDate = (dateString) => {
    const date = new Date(dateString);
    const now = new Date();
    const diffMs = now - date;
    const diffDays = Math.floor(diffMs / (1000 * 60 * 60 * 24));
    
    if (diffDays === 0) return 'сегодня';
    if (diffDays === 1) return 'вчера';
    if (diffDays < 7) return `${diffDays} дня назад`;
    if (diffDays < 30) return `${Math.floor(diffDays / 7)} недель назад`;
    return date.toLocaleDateString('ru-RU');
  };

  const fetchCities = async () => {
    try {
      const response = await api.get('/cities?Size=500');
      setCities(response.data);
    } catch (err) {
      console.error('Ошибка загрузки городов', err);
    }
  };

  const fetchFeed = async (reset = true) => {
    if (reset) {
      setLoading(true);
      setPage(1);
    } else {
      setLoadingMore(true);
    }

    try {
      const params = {
        Page: reset ? 1 : page,
        Size: 10,
        SortByAsc: false
      };
      
      if (cityId) {
        params.CityId = parseInt(cityId);
      }

      const response = await api.get('/listings/feed', { params });
      
      if (reset) {
        setListings(response.data);
        setHasMore(response.data.length === 10);
        setPage(2);
      } else {
        setListings(prev => [...prev, ...response.data]);
        setHasMore(response.data.length === 10);
        setPage(prev => prev + 1);
      }
    } catch (err) {
      console.error('Ошибка загрузки ленты', err);
      setError('Не удалось загрузить объявления');
    } finally {
      setLoading(false);
      setLoadingMore(false);
    }
  };

  useEffect(() => {
    fetchCities();
  }, []);

  useEffect(() => {
    setPage(1);
    setListings([]);
    fetchFeed(true);
  }, [cityId]);

  const lastListingRef = useCallback((node) => {
    if (loadingMore) return;
    if (observerRef.current) observerRef.current.disconnect();
    observerRef.current = new IntersectionObserver(entries => {
      if (entries[0].isIntersecting && hasMore) {
        fetchFeed(false);
      }
    });
    if (node) observerRef.current.observe(node);
  }, [loadingMore, hasMore]);

  if (loading) {
    return (
      <div className="d-flex justify-content-center align-items-center" style={{ minHeight: '100vh' }}>
        <div className="spinner-border text-primary" role="status"></div>
      </div>
    );
  }

  return (
    <>
      <style>{`
        .feed-bg {
          background: linear-gradient(135deg, #e0f2fe 0%, #bae6fd 50%, #7dd3fc 100%);
          min-height: 100vh;
        }
        .feed-header {
          background: linear-gradient(135deg, #1e3c72 0%, #2a5298 100%);
          padding: 20px 0;
          box-shadow: 0 4px 20px rgba(0,0,0,0.1);
          margin-bottom: 40px;
          position: sticky;
          top: 76px;
          z-index: 1000;
        }
        .city-select {
          background: rgba(255,255,255,0.95);
          border: none;
          border-radius: 40px;
          padding: 10px 24px;
          font-weight: 500;
          color: #1e3c72;
          cursor: pointer;
          font-size: 14px;
        }
        .listing-card {
          background: white;
          border-radius: 28px;
          overflow: hidden;
          transition: all 0.3s ease;
          box-shadow: 0 8px 24px rgba(0,0,0,0.06);
        }
        .listing-card:hover {
          transform: translateY(-6px);
          box-shadow: 0 20px 40px rgba(0,0,0,0.12);
        }
        .listing-image {
          width: 100%;
          height: 320px;
          object-fit: cover;
        }
        .badge-sale {
          background: linear-gradient(135deg, #10b981, #059669);
          border-radius: 40px;
          padding: 6px 16px;
          font-size: 13px;
          font-weight: 600;
          color: white;
          display: inline-block;
        }
        .badge-buy {
          background: linear-gradient(135deg, #06b6d4, #0891b2);
          border-radius: 40px;
          padding: 6px 16px;
          font-size: 13px;
          font-weight: 600;
          color: white;
          display: inline-block;
        }
        .badge-accident {
          background: linear-gradient(135deg, #ef4444, #dc2626);
          border-radius: 40px;
          padding: 6px 16px;
          font-size: 13px;
          font-weight: 600;
          color: white;
          display: inline-block;
        }
        .badge-clean {
          background: linear-gradient(135deg, #10b981, #059669);
          border-radius: 40px;
          padding: 6px 16px;
          font-size: 13px;
          font-weight: 600;
          color: white;
          display: inline-block;
        }
        .listing-price {
          font-size: 32px;
          font-weight: 800;
          color: #1e3c72;
        }
        .spec-item {
          display: flex;
          align-items: center;
          gap: 6px;
          font-size: 14px;
          color: #4b5563;
          background: #f3f4f6;
          padding: 6px 12px;
          border-radius: 40px;
        }
        .spec-icon {
          font-size: 14px;
          color: #6b7280;
        }
        .listing-description {
          font-size: 15px;
          line-height: 1.6;
          color: #4b5563;
        }
      `}</style>

      <div className="feed-bg">
        <div className="feed-header">
          <div className="container">
            <div className="d-flex justify-content-between align-items-center">
              <h2 className="h3 fw-bold mb-0" style={{ color: 'white' }}>Лента объявлений</h2>
              <select className="city-select" value={cityId} onChange={(e) => setCityId(e.target.value)}>
                <option value="">Все города</option>
                {cities.map(city => (
                  <option key={city.id} value={city.id}>{city.name}</option>
                ))}
              </select>
            </div>
          </div>
        </div>

        <div className="container pb-5">
          <div className="row justify-content-center">
            <div className="col-lg-8">
              {error && <div className="alert alert-danger rounded-4">{error}</div>}

              {listings.length === 0 && !error && (
                <div className="text-center py-5">
                  <i className="bi bi-newspaper fs-1 text-muted"></i>
                  <p className="text-muted mt-2">Нет объявлений</p>
                </div>
              )}

              <div className="d-flex flex-column gap-4">
                {listings.map((listing, index) => {
                  const isSale = listing.type === 0 || listing.type === 'Sale';
                  const saleInfo = listing.saleInfoDto;
                  const buyInfo = listing.buyInfoDto;
                  
                  // Для продажи: картинка из saleInfo.carImageUrl, для покупки: из buyInfo.brandImageUrl
                  const imageUrl = isSale 
                    ? saleInfo?.carImageUrl 
                    : buyInfo?.brandImageUrl;
                  
                  // Для продажи: цена из saleInfo.price, для покупки: диапазон из buyInfo
                  const priceDisplay = isSale 
                    ? `${saleInfo?.price?.toLocaleString()} ₽`
                    : buyInfo ? `${buyInfo.minPrice?.toLocaleString()} - ${buyInfo.maxPrice?.toLocaleString()} ₽` : '';
                  
                  // Для продажи: модель из saleInfo, для покупки: из buyInfo
                  const modelName = isSale ? saleInfo?.modelName : buyInfo?.modelName;
                  
                  // Цвет
                  const color = isSale ? saleInfo?.carColor : buyInfo?.carColor;
                  
                  // Пробег и год только для продажи
                  const mileage = isSale ? saleInfo?.mileage : null;
                  const releaseYear = isSale ? saleInfo?.releaseYear : null;
                  const hasAccident = isSale ? saleInfo?.hasAccident : null;
                  const ownershipsQuantity = isSale ? saleInfo?.ownershipsQuantity : null;

                  return (
                    <div 
                      key={listing.id} 
                      className="listing-card"
                      ref={index === listings.length - 1 ? lastListingRef : null}
                    >
                      {imageUrl && (
                        <img 
                          src={imageUrl} 
                          alt={listing.name}
                          className="listing-image"
                          onError={(e) => { e.target.style.display = 'none'; }}
                        />
                      )}
                      <div className="p-5">
                        <div className="d-flex justify-content-between align-items-start mb-3">
                          <div className="d-flex gap-2">
                            <span className={isSale ? 'badge-sale' : 'badge-buy'}>
                              {getTypeText(listing.type)}
                            </span>
                            {isSale && hasAccident !== null && (
                              hasAccident ? (
                                <span className="badge-accident">Была в ДТП</span>
                              ) : (
                                <span className="badge-clean">Без ДТП</span>
                              )
                            )}
                          </div>
                          <small className="text-muted">
                            <i className="bi bi-clock me-1"></i>
                            {formatDate(listing.createdAt)}
                          </small>
                        </div>

                        <h3 className="fw-bold mb-3">{listing.name}</h3>

                        <div className="listing-price mb-3">
                          {priceDisplay}
                        </div>
                        
                        <div className="d-flex flex-wrap gap-2 mb-3">
                          {releaseYear && releaseYear > 0 && (
                            <div className="spec-item">
                              <i className="bi bi-calendar spec-icon"></i>
                              <span>{releaseYear} г.</span>
                            </div>
                          )}
                          {mileage !== null && mileage !== undefined && (
                            <div className="spec-item">
                              <i className="bi bi-speedometer2 spec-icon"></i>
                              <span>{formatMileage(mileage)} км</span>
                            </div>
                          )}
                          {color && (
                            <div className="spec-item">
                              <i className="bi bi-palette spec-icon"></i>
                              <span>{getColorName(color)}</span>
                            </div>
                          )}
                          {modelName && (
                            <div className="spec-item">
                              <i className="bi bi-car-front spec-icon"></i>
                              <span>{modelName}</span>
                            </div>
                          )}
                          {ownershipsQuantity !== null && ownershipsQuantity > 0 && (
                            <div className="spec-item">
                              <i className="bi bi-people spec-icon"></i>
                              <span>{ownershipsQuantity} влад.</span>
                            </div>
                          )}
                        </div>

                        <div className="listing-description mb-4">
                          <p className="text-secondary">
                            {listing.description}
                          </p>
                        </div>

                        <div className="d-flex justify-content-between align-items-center pt-3 border-top">
                          <div className="d-flex align-items-center gap-3">
                            <div className="d-flex align-items-center text-muted" style={{ fontSize: '14px' }}>
                              <i className="bi bi-geo-alt me-1"></i>
                              {listing.location?.city?.name || 'Город не указан'}
                            </div>
                            <div className="d-flex align-items-center text-muted" style={{ fontSize: '14px' }}>
                              <i className="bi bi-person me-1"></i>
                              {listing.author?.surname} {listing.author?.name}
                            </div>
                          </div>
                          <div className="d-flex align-items-center text-muted" style={{ fontSize: '14px' }}>
                            <i className="bi bi-heart me-1"></i>
                            {listing.reactionsQuantity || 0}
                          </div>
                        </div>
                      </div>
                    </div>
                  );
                })}
              </div>

              {loadingMore && (
                <div className="text-center mt-4 py-3">
                  <div className="spinner-border text-primary" role="status"></div>
                </div>
              )}

              {!hasMore && listings.length > 0 && (
                <div className="text-center mt-4">
                  <p className="text-muted">Вы просмотрели все объявления</p>
                </div>
              )}
            </div>
          </div>
        </div>
      </div>
    </>
  );
};

export default FeedPage;