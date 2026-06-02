import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import api from '../api/axiosConfig';

const SearchPage = () => {
  const [searchText, setSearchText] = useState('');
  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [cityId, setCityId] = useState('');
  const [cities, setCities] = useState([]);

  const fetchCities = async () => {
    try {
      const response = await api.get('/cities?Size=500');
      setCities(response.data);
    } catch (err) {
      console.error('Ошибка загрузки городов', err);
    }
  };

  const fetchUsers = async () => {
    setLoading(true);
    setError('');

    try {
      const params = {
        Page: 1,
        Size: 50,
        SearchText: searchText.trim(),
        SortByAsc: true,
        ...(cityId && { CityId: parseInt(cityId) })
      };

      const response = await api.get('/users', { params });
      setUsers(response.data);
    } catch (err) {
      console.error('Ошибка поиска', err);
      setError('Не удалось найти пользователей');
    } finally {
      setLoading(false);
    }
  };

  const handleKeyPress = (e) => {
    if (e.key === 'Enter') {
      fetchUsers();
    }
  };

  useEffect(() => {
    fetchCities();
    fetchUsers();
  }, []);

  useEffect(() => {
    fetchUsers();
  }, [searchText, cityId]);

  const getInitials = (name, surname) => {
    return `${name?.charAt(0) || ''}${surname?.charAt(0) || ''}`.toUpperCase();
  };

  return (
    <>
      <style>{`
        .search-bg {
          background: linear-gradient(135deg, #e0f2fe 0%, #bae6fd 50%, #7dd3fc 100%);
          min-height: 100vh;
        }
        .search-header {
          background: linear-gradient(135deg, #1e3c72 0%, #2a5298 100%);
          padding: 20px 0;
          box-shadow: 0 4px 20px rgba(0,0,0,0.1);
          margin-bottom: 32px;
          position: sticky;
          top: 76px;
          z-index: 1000;
        }
        .search-container {
          max-width: 800px;
          margin: 0 auto;
        }
        .search-card {
          background: white;
          border-radius: 24px;
          padding: 24px;
          box-shadow: 0 8px 32px rgba(0,0,0,0.08);
        }
        .search-input-group {
          display: flex;
          gap: 12px;
          align-items: center;
          flex-wrap: wrap;
        }
        .search-input-wrapper {
          flex: 1;
          position: relative;
        }
        .search-icon {
          position: absolute;
          left: 16px;
          top: 50%;
          transform: translateY(-50%);
          color: #94a3b8;
          font-size: 18px;
        }
        .search-input {
          width: 100%;
          padding: 14px 20px 14px 48px;
          border: 2px solid #e2e8f0;
          border-radius: 60px;
          font-size: 15px;
          outline: none;
          transition: all 0.2s;
          background: white;
        }
        .search-input:focus {
          border-color: #1e3c72;
          box-shadow: 0 0 0 3px rgba(30,60,114,0.1);
        }
        .city-select-wrapper {
          position: relative;
          min-width: 180px;
        }
        .city-select {
          width: 100%;
          padding: 10px 32px 10px 16px;
          border: none;
          border-radius: 40px;
          background: rgba(255,255,255,0.95);
          font-weight: 500;
          color: #1e3c72;
          cursor: pointer;
          appearance: none;
          font-size: 14px;
        }
        .select-arrow {
          position: absolute;
          right: 14px;
          top: 50%;
          transform: translateY(-50%);
          color: #1e3c72;
          pointer-events: none;
          font-size: 14px;
        }
        .user-card {
          background: white;
          border-radius: 20px;
          padding: 20px;
          transition: all 0.3s ease;
          box-shadow: 0 4px 12px rgba(0,0,0,0.05);
          border: 1px solid #f0f2f5;
        }
        .user-card:hover {
          transform: translateY(-3px);
          box-shadow: 0 12px 28px rgba(0,0,0,0.1);
        }
        .user-avatar {
          width: 60px;
          height: 60px;
          border-radius: 50%;
          object-fit: cover;
          flex-shrink: 0;
        }
        .avatar-placeholder {
          width: 60px;
          height: 60px;
          border-radius: 50%;
          background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
          display: flex;
          align-items: center;
          justify-content: center;
          font-size: 24px;
          font-weight: 600;
          color: white;
          flex-shrink: 0;
        }
        .user-name {
          font-size: 18px;
          font-weight: 700;
          margin-bottom: 4px;
          color: #1e293b;
        }
        .user-location {
          font-size: 13px;
          color: #64748b;
          display: flex;
          align-items: center;
          gap: 4px;
        }
        .user-stats {
          display: flex;
          gap: 16px;
        }
        .user-stat {
          text-align: center;
        }
        .user-stat-value {
          font-size: 16px;
          font-weight: 700;
          color: #1e3c72;
        }
        .user-stat-label {
          font-size: 11px;
          color: #64748b;
        }
        .profile-link {
          background: #1e3c72;
          color: white;
          border: none;
          border-radius: 40px;
          padding: 8px 24px;
          font-size: 13px;
          font-weight: 500;
          text-decoration: none;
          transition: all 0.2s;
          white-space: nowrap;
          display: inline-flex;
          align-items: center;
          gap: 6px;
        }
        .profile-link:hover {
          background: #2a5298;
          transform: translateY(-1px);
        }
        .empty-state {
          text-align: center;
          padding: 60px 20px;
          background: white;
          border-radius: 24px;
        }
        .empty-icon {
          font-size: 64px;
          color: #cbd5e1;
          margin-bottom: 16px;
        }
        .empty-title {
          font-size: 20px;
          font-weight: 600;
          color: #1e293b;
          margin-bottom: 8px;
        }
        .empty-text {
          color: #64748b;
          font-size: 14px;
        }
        .results-header {
          display: flex;
          justify-content: space-between;
          align-items: center;
          margin: 24px 0 16px;
        }
        .results-count {
          font-size: 14px;
          color: #475569;
          background: #f1f5f9;
          padding: 6px 14px;
          border-radius: 30px;
        }
        @media (max-width: 768px) {
          .user-card .d-flex {
            flex-direction: column;
            text-align: center;
            gap: 12px;
          }
          .user-stats {
            justify-content: center;
          }
          .profile-link {
            justify-content: center;
          }
          .search-input-group {
            flex-direction: column;
          }
          .city-select-wrapper {
            width: 100%;
          }
        }
      `}</style>

      <div className="search-bg">
        <div className="search-header">
          <div className="container">
            <div className="search-container">
              <div className="d-flex justify-content-between align-items-center flex-wrap gap-3">
                <h2 className="h3 fw-bold mb-0" style={{ color: 'white' }}>Поиск пользователей</h2>
                <div className="city-select-wrapper">
                  <select className="city-select" value={cityId} onChange={(e) => setCityId(e.target.value)}>
                    <option value="">Все города</option>
                    {cities.map(city => (
                      <option key={city.id} value={city.id}>{city.name}</option>
                    ))}
                  </select>
                  <i className="bi bi-chevron-down select-arrow"></i>
                </div>
              </div>
            </div>
          </div>
        </div>

        <div className="container">
          <div className="search-container">
            <div className="search-card">
              <div className="search-input-group">
                <div className="search-input-wrapper">
                  <i className="bi bi-search search-icon"></i>
                  <input
                    type="text"
                    className="search-input"
                    placeholder="Имя или фамилия..."
                    value={searchText}
                    onChange={(e) => setSearchText(e.target.value)}
                    onKeyPress={handleKeyPress}
                  />
                </div>
              </div>
            </div>

            {error && (
              <div className="alert alert-danger rounded-4 mt-4">{error}</div>
            )}

            {!loading && users.length === 0 && (
              <div className="empty-state">
                <div className="empty-icon">
                  <i className="bi bi-person-x"></i>
                </div>
                <div className="empty-title">Никого не нашли</div>
                <div className="empty-text">Попробуйте изменить имя или выбрать другой город</div>
              </div>
            )}

            {!loading && users.length > 0 && (
              <>
                <div className="results-header">
                  <div className="results-count">
                    <i className="bi bi-people me-1"></i>
                    {users.length} {users.length === 1 ? 'пользователь' : users.length < 5 ? 'пользователя' : 'пользователей'}
                  </div>
                </div>

                <div className="d-flex flex-column gap-3">
                  {users.map((user) => (
                    <div className="user-card" key={user.id}>
                      <div className="d-flex align-items-center gap-4 flex-wrap flex-md-nowrap">
                        {user.avatarUrl ? (
                          <img src={user.avatarUrl} alt={user.name} className="user-avatar" />
                        ) : (
                          <div className="avatar-placeholder">
                            {getInitials(user.name, user.surname)}
                          </div>
                        )}
                        <div className="flex-grow-1">
                          <div className="user-name">{user.name} {user.surname}</div>
                          {user.location?.city?.name && (
                            <div className="user-location">
                              <i className="bi bi-geo-alt"></i>
                              {user.location.city.name}
                            </div>
                          )}
                        </div>
                        <div className="user-stats">
                          <div className="user-stat">
                            <div className="user-stat-value">{user.followersQuantity || 0}</div>
                            <div className="user-stat-label">подписчиков</div>
                          </div>
                          <div className="user-stat">
                            <div className="user-stat-value">{user.reviewsQuantity || 0}</div>
                            <div className="user-stat-label">отзывов</div>
                          </div>
                        </div>
                        <Link to={`/profile/${user.id}`} className="profile-link">
                          Перейти <i className="bi bi-arrow-right"></i>
                        </Link>
                      </div>
                    </div>
                  ))}
                </div>
              </>
            )}
          </div>
        </div>
      </div>
    </>
  );
};

export default SearchPage;