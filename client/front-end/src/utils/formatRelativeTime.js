import { formatDistanceToNow } from 'date-fns';

// Function to format date in a relative format or full date based on age
const formatRelativeTime = (dateString) => {
  const now = new Date();
  const date = new Date(dateString);
  const daysDiff = Math.floor((now - date) / (1000 * 60 * 60 * 24));

  if (daysDiff <= 1) {
    // Format relative time for the past 3 days
    return formatDistanceToNow(date, { addSuffix: true });
  } else {
    // Format full date for times older than 3 days
    return new Intl.DateTimeFormat(undefined, {
      year: 'numeric',
      month: 'long',
      day: 'numeric',
      hour: 'numeric',
      minute: 'numeric',
      second: 'numeric'
    }).format(date);
  }
};

export default formatRelativeTime;
