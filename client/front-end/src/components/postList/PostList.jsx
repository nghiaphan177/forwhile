import React, { useEffect, useState } from 'react';
import { getPosts } from '../../services/api';
import { SortDirection, OrderType } from '../../constants/constants';
import formatRelativeTime from '../../utils/formatRelativeTime';
import { Spinner, Button, Alert, Badge } from 'react-bootstrap';
import { FaEye, FaCaretUp } from 'react-icons/fa';
import './PostList.css';

const PostList = () => {
  const [posts, setPosts] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);

  const queryParams = {
    TypeId: 1,
    OrderBy: OrderType.MostVotes,
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
        setTotalPages(fetchedTotalPages); // Assuming API returns totalPages
      } catch (error) {
        setError('Failed to fetch posts.');
      } finally {
        setLoading(false);
      }
    };

    fetchPosts();
  }, [currentPage]);

  const handlePageChange = (page) => {
    setCurrentPage(page);
  };

  const generatePagination = () => {
    const pageNumbers = [];
    let startPage = Math.max(1, currentPage - 2);
    let endPage = Math.min(totalPages, currentPage + 2);

    if (currentPage - 1 <= 2) {
      startPage = 1;
      endPage = Math.min(5, totalPages);
    }

    if (totalPages - currentPage <= 2) {
      endPage = totalPages;
      startPage = Math.max(1, totalPages - 4);
    }

    for (let i = startPage; i <= endPage; i++) {
      pageNumbers.push(i);
    }

    return pageNumbers;
  };

  if (loading) return <Spinner animation="border" variant="primary" />;
  if (error) return <Alert variant="danger">{error}</Alert>;

  return (
    <div className="container mt-4">
      {posts.length > 0 ? (
        <>
          {posts.map(post => (
            <div key={post.postId} className="post-card">
              <img src={post.avatar} alt={post.author} />
              <div className="post-content">
                <div className="post-title">{post.title}</div>
                <div className="post-meta">
                  <span>{post.authorUserName} created at: {formatRelativeTime(post.createdAt)}</span> 
                  | <span>Last Reply: {post.lastCommentAuthor} {formatRelativeTime(post.lastCommentCreatedAt)}</span>
                </div>
                <div className="post-tags">
                  {Array.isArray(post.tags) ? post.tags.map(tag => (
                    <Badge key={tag} pill variant="secondary">{tag}</Badge>
                  )) : 'No tags'}
                </div>
              </div>
              <div className="post-stats">
                <span className="vote-count"><FaCaretUp /> {post.vote}</span>
                <span><FaEye /> {post.views}</span>
              </div>
            </div>
          ))}
          {totalPages > 1 && (
            <div className="d-flex justify-content-between mt-4">
              <Button 
                variant="secondary" 
                onClick={() => handlePageChange(Math.max(currentPage - 1, 1))}
                disabled={currentPage === 1}
              >
                Previous
              </Button>
              <div className="pagination">
                {generatePagination().map((page, index) => (
                  <Button 
                    key={index}
                    variant="secondary" 
                    onClick={() => handlePageChange(page)}
                    active={currentPage === page}
                  >
                    {page}
                  </Button>
                ))}
                {currentPage < totalPages - 2 && <span>...</span>}
                {currentPage < totalPages - 1 && (
                  <Button 
                    variant="secondary" 
                    onClick={() => handlePageChange(totalPages)}
                  >
                    {totalPages}
                  </Button>
                )}
              </div>
              <Button 
                variant="secondary" 
                onClick={() => handlePageChange(Math.min(currentPage + 1, totalPages))}
                disabled={currentPage === totalPages}
              >
                Next
              </Button>
            </div>
          )}
        </>
      ) : (
        <p>No posts available.</p>
      )}
    </div>
  );
};

export default PostList;
