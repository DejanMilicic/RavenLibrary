import http from 'k6/http';
import { check, group, sleep, fail } from 'k6';

export let options = {
  vus: 1, // 1 user looping for 1 minute
  duration: '20s',

  thresholds: {
    http_req_duration: ['p(95)<1500'], // 95% of requests must complete below 1.5s
  },
};

export default () => {
  let user = http.get(`${__ENV.BASE_URL}/user?id=users/164496167832`).json();

  sleep(1);
};
