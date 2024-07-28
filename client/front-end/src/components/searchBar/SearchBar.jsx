// src/components/SearchBar/SearchBar.jsx

import React from 'react';
import { Form, FormControl, Button } from 'react-bootstrap';
import './SearchBar.css';

const SearchBar = ({ searchQuery, setSearchQuery, handleSearch }) => {
  return (
    <Form inline className="my-2 my-lg-0">
      <FormControl 
        type="text" 
        placeholder="Search topics or comments..." 
        className="mr-sm-2"
        value={searchQuery}
        onChange={(e) => setSearchQuery(e.target.value)}
      />
      <Button variant="outline-success" onClick={handleSearch}>Search</Button>
    </Form>
  );
};

export default SearchBar;
