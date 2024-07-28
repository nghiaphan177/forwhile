// src/components/PostList/PostList.jsx

import React, { useEffect, useState } from 'react';
import { getPosts } from '../../services/api';
import { SortDirection, OrderType } from '../../constants/constants';
import { Spinner, Alert } from 'react-bootstrap';
import PostCard from '../postCard/PostCard';
import CustomPagination from '../pagination/CustomPagination';
import SearchBar from '../searchBar/SearchBar';
import SortOptions from '../sortOptions/SortOptions';
import './PostList.css';

const PostList = () => {
  const [posts, setPosts] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [searchQuery, setSearchQuery] = useState('');
  const [sortOrder, setSortOrder] = useState(OrderType.MostVotes);

  const queryParams = {
    TypeId: 1,
    OrderBy: sortOrder,
    SortDirection: SortDirection.Descending,
    PageNumber: currentPage,
    PageSize: 15,
  };

  useEffect(() => {
    const fetchPosts = async () => {
      try {
        setLoading(true);
        const { posts: fetchedPosts, totalPages: fetchedTotalPages } = await getPosts(queryParams);
        setPosts(fetchedPosts);
        setTotalPages(fetchedTotalPages);
      } catch (error) {
        setError('Failed to fetch posts.');
      } finally {
        setLoading(false);
      }
    };

    fetchPosts();
  }, [currentPage, sortOrder]);

  const handlePageChange = (page) => {
    setCurrentPage(page);
  };

  const handleSearch = () => {
    // Implement search functionality here if required
  };

  if (loading) return <Spinner animation="border" variant="primary" />;
  if (error) return <Alert variant="danger">{error}</Alert>;

  return (
    <div className="container mt-4">
      <div className="d-flex justify-content-between">
        <SortOptions setSortOrder={setSortOrder} />
        <SearchBar searchQuery={searchQuery} setSearchQuery={setSearchQuery} handleSearch={handleSearch} />
      </div>
      {posts.length > 0 ? (
        <>
          {posts.map(post => (
            <PostCard key={post.postId} post={post} />
          ))}
          {totalPages > 0 && (
            <CustomPagination 
              currentPage={currentPage} 
              totalPages={totalPages} 
              handlePageChange={handlePageChange} 
            />
          )}
        </>
      ) : (
        <p>No posts available.</p>
      )}
    </div>
  );
};

export default PostList;
