import { createContext, useState, useContext, useEffect } from 'react';
import { login as loginApi, register as registerApi } from '../api/authApi';
import { getToken, setToken, removeToken } from '../services/tokenService';

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const token = getToken();
    if (token) {
      // TODO: можно декодировать токен или запросить /users/me
      setUser({ email: 'user@example.com' });
    }
    setLoading(false);
  }, []);

  const login = async (email, password) => {
    const response = await loginApi(email, password);
    const { token } = response;
    setToken(token);
    setUser({ email });
    return response;
  };

  const register = async (userData) => {
    const response = await registerApi(userData);
    const { token } = response;
    setToken(token);
    setUser({ email: userData.email });
    return response;
  };

  const logout = () => {
    removeToken();
    setUser(null);
  };

  return (
    <AuthContext.Provider value={{ user, loading, login, register, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => useContext(AuthContext);