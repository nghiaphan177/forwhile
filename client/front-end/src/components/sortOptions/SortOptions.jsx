// src/components/SortOptions/SortOptions.jsx

import React from 'react';
import { DropdownButton, Dropdown } from 'react-bootstrap';
import { OrderType } from '../../constants/constants';
import './SortOptions.css';

const SortOptions = ({ setSortOrder }) => {
  return (
    <DropdownButton id="dropdown-basic-button" title="Sort By">
      <Dropdown.Item onClick={() => setSortOrder(OrderType.MostVotes)}>Most Votes</Dropdown.Item>
      <Dropdown.Item onClick={() => setSortOrder(OrderType.MostViews)}>Most Views</Dropdown.Item>
      <Dropdown.Item onClick={() => setSortOrder(OrderType.Recent)}>Recent</Dropdown.Item>
    </DropdownButton>
  );
};

export default SortOptions;
