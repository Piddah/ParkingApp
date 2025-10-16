import useAuthStore from '../context/AuthStore';

const baseUrl = import.meta?.env?.VITE_API_URL || 'https://localhost:7022';

export async function apiFetch(path, options = {}) {
  const { token } = useAuthStore.getState();
  const headers = {
    'Content-Type': 'application/json',
    ...(options.headers || {}),
  };

  if (token) headers['Authorization'] = `Bearer ${token}`;

  const res = await fetch(`${baseUrl}${path}`, { ...options, headers });

  if (res.status === 401) {
    // optional: auto-logout on unauthorized
    // useAuthStore.getState().logout();
  }

  return res;
}

