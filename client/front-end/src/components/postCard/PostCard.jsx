// src/components/PostCard/PostCard.jsx

import React from 'react';
import { Badge } from 'react-bootstrap';
import { FaEye, FaCaretUp } from 'react-icons/fa';
import formatRelativeTime from '../../utils/formatRelativeTime';
import './PostCard.css';

const PostCard = ({ post }) => {
  return (
    <div className="post-card">
      <img src={post.avatar} alt={post.author} />
      <div className="post-content">
        <div className="post-title">{post.title}</div>
        <div className="post-meta">
          <span>{post.author} created at: {formatRelativeTime(post.createdAt)}</span>
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
        <span><FaEye /> {post.view}</span>
      </div>
    </div>
  );
};

export default PostCard;
