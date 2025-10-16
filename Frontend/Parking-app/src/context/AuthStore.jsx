import { create } from 'zustand';

const persisted = (() => {
  try {
    const raw = localStorage.getItem('auth');
    return raw ? JSON.parse(raw) : { userId: null, token: null };
  } catch {
    return { userId: null, token: null };
  }
})();

const useAuthStore = create((set, get) => ({
  userId: persisted.userId,
  token: persisted.token,

  setAuth: (userId, token) => {
    const next = { userId, token };
    localStorage.setItem('auth', JSON.stringify(next));
    set(next);
  },
  logout: () => {
    localStorage.removeItem('auth');
    set({ userId: null, token: null });
  },
}));

export default useAuthStore;
