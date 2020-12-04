import http from 'k6/http';
import { check, group, sleep, fail } from 'k6';

export let options = {
  vus: 1, // 1 user looping for 1 minute
  duration: '20s',

  thresholds: {
    http_req_duration: ['p(99)<1500'], // 99% of requests must complete below 1.5s
  },
};

const BASE_URL = 'https://localhost:5001';

export default () => {
  let user = http.get(`${BASE_URL}/user?id=users/164496167832`).json();
  check(user, { 'user retreived': (u) => u.id == 'users/164496167832' });

  sleep(1);
};
