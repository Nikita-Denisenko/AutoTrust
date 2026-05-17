import { createContext, useState, useContext, useEffect } from 'react';
import api from '../api/axiosConfig';
import { getToken, setToken, removeToken } from '../services/tokenService';

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const token = getToken();
    if (token) {
      api.defaults.headers.common['Authorization'] = `Bearer ${token}`;
      api.get('/users/me')
        .then(response => {
          setUser(response.data);
        })
        .catch(() => {
          removeToken();
          delete api.defaults.headers.common['Authorization'];
        })
        .finally(() => setLoading(false));
    } else {
      setLoading(false);
    }
  }, []);

  const login = async (email, password) => {
    const response = await api.post('/auth/login', { email, password });
    const { token } = response.data;
    setToken(token);
    api.defaults.headers.common['Authorization'] = `Bearer ${token}`;

    const profileResponse = await api.get('/users/me');
    setUser(profileResponse.data);
    return response.data;
  };

  const register = async (userData) => {
    const response = await api.post('/auth/register', userData);
    const { token } = response.data;
    setToken(token);
    api.defaults.headers.common['Authorization'] = `Bearer ${token}`;

    const profileResponse = await api.get('/users/me');
    setUser(profileResponse.data);
    return response.data;
  };

  const logout = () => {
    removeToken();
    delete api.defaults.headers.common['Authorization'];
    setUser(null);
  };

  return (
    <AuthContext.Provider value={{ user, loading, login, register, logout, isAuthenticated: !!user }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => useContext(AuthContext);