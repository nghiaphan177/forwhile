import React from 'react';
import Pagination from 'react-bootstrap/Pagination';
import './CustomPagination.css';

const CustomPagination = ({ currentPage, totalPages, handlePageChange }) => {
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

  const pageNumbers = generatePagination();

  return (
    <Pagination size='sm' className="custom-pagination">
      <Pagination.Prev 
        onClick={() => handlePageChange(Math.max(currentPage - 1, 1))} 
        disabled={currentPage === 1} 
      />
      {currentPage > 3 && (
        <>
          <Pagination.Item onClick={() => handlePageChange(1)}>{1}</Pagination.Item>
          {currentPage > 4 && <Pagination.Ellipsis />}
        </>
      )}
      {pageNumbers.map((page) => (
        <Pagination.Item
          key={page}
          active={page === currentPage}
          onClick={() => handlePageChange(page)}
        >
          {page}
        </Pagination.Item>
      ))}
      {currentPage < totalPages - 2 && (
        <>
          {currentPage < totalPages - 3 && <Pagination.Ellipsis />}
          <Pagination.Item onClick={() => handlePageChange(totalPages)}>{totalPages}</Pagination.Item>
        </>
      )}
      <Pagination.Next 
        onClick={() => handlePageChange(Math.min(currentPage + 1, totalPages))} 
        disabled={currentPage === totalPages} 
      />
    </Pagination>
  );
};

export default CustomPagination;
