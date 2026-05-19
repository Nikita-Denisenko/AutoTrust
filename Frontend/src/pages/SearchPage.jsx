import { useState } from 'react';
import api from '../api/axiosConfig';

const SearchPage = () => {
  const [query, setQuery] = useState('');
  const [results, setResults] = useState([]);
  const [loading, setLoading] = useState(false);

  const handleSearch = async () => {
    if (!query.trim()) return;
    setLoading(true);
    try {
      const response = await api.get(`/users/search?name=${query}`);
      setResults(response.data);
    } catch (err) {
      console.error('Ошибка поиска', err);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="container py-5" style={{ marginTop: '76px' }}>
      <h1 className="h2 mb-4">Поиск людей</h1>
      <div className="input-group mb-4" style={{ maxWidth: '500px' }}>
        <input
          type="text"
          className="form-control form-control-lg"
          placeholder="Имя пользователя..."
          value={query}
          onChange={(e) => setQuery(e.target.value)}
          onKeyPress={(e) => e.key === 'Enter' && handleSearch()}
        />
        <button className="btn btn-primary" onClick={handleSearch} disabled={loading}>
          {loading ? 'Поиск...' : 'Искать'}
        </button>
      </div>

      {results.length > 0 && (
        <div className="row g-3">
          {results.map(user => (
            <div className="col-md-4" key={user.id}>
              <div className="card h-100 shadow-sm border-0 rounded-4">
                <div className="card-body">
                  <div className="d-flex align-items-center gap-3 mb-2">
                    {user.avatarUrl ? (
                      <img src={user.avatarUrl} width="48" height="48" className="rounded-circle" alt="avatar" />
                    ) : (
                      <div className="bg-secondary rounded-circle d-flex align-items-center justify-content-center" style={{ width: '48px', height: '48px', color: 'white', fontSize: '18px' }}>
                        {user.name?.charAt(0)}{user.surname?.charAt(0)}
                      </div>
                    )}
                    <div>
                      <h5 className="mb-0">{user.name} {user.surname}</h5>
                      <small className="text-muted">{user.city?.name || 'Город не указан'}</small>
                    </div>
                  </div>
                  <Link to={`/profile/${user.id}`} className="btn btn-outline-primary w-100 rounded-pill">Перейти</Link>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}

      {results.length === 0 && query && !loading && (
        <p className="text-muted">Ничего не найдено</p>
      )}
    </div>
  );
};

export default SearchPage;