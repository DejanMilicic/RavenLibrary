import http from 'k6/http';
import { check, group, sleep, fail } from 'k6';

export let options = {
  stages: [
    { duration: '1m', target: 100 }, // simulate ramp-up of traffic from 1 to 100 users over 1 minutes.
    { duration: '2m', target: 100 }, // stay at 100 users for 2 minutes
    { duration: '1m', target: 0 }, // ramp-down to 0 users
  ],
  thresholds: {
    http_req_duration: ['p(99)<1500'], // 99% of requests must complete below 1.5s
    'user retreived': ['p(99)<1500'], // 99% of requests must complete below 1.5s
  },
};

const BASE_URL = 'https://localhost:5001';

export default () => {
  let res = http.get(`${BASE_URL}/user?id=users/164496167832`);
  check(res, {'status == 200': (r) => r.status === 200 });

  let user = res.json();
  check(user, { 'user retreived': (u) => u.id == 'users/164496167832' });

  sleep(1);
};
