// src/App.js

import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import PostList from './components/postList/PostList';

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<PostList />} />
      </Routes>
    </Router>
  );
}

export default App;
