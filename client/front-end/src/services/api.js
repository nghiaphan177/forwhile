import axios from 'axios';

const api = axios.create({
  baseURL: 'https://localhost:7247/api',
  headers: {
    'Content-Type': 'application/json',
  },
});

export const getPosts = async (params) => {
    try {
      const response = await api.get('/Post', { params });
      console.log("response:" + response)
      return response.data;
    } catch (error) {
      console.error('Failed to fetch posts', error);
      throw error;
    }
  };