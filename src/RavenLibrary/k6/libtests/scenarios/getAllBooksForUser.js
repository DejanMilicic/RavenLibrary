import http from 'k6/http';
import { check, group, sleep, fail } from 'k6';
import { Rate } from 'k6/metrics'
import faker from 'https://cdnjs.cloudflare.com/ajax/libs/Faker/3.1.0/faker.min.js';

let errorRate = new Rate('error_rate')

export let options = {
  stages: [
    { duration: '30s', target: 100 }, // simulate ramp-up of traffic from 1 to 100
    { duration: '1m', target: 100 }, // stay at 100 users
    { duration: '30s', target: 0 }, // ramp-down to 0 users
  ],

  thresholds: {
    http_req_duration: ['p(99)<1500'], // 99% of requests must complete below 1.5s
  },
};

export default () => {
  let randomUserRes = http.get(`${__ENV.BASE_URL}/user/random`);
  let randomUserId = JSON.parse(randomUserRes.body).id;

  let res = http.get(`${__ENV.BASE_URL}/books/user?userId=${randomUserId}`);

  errorRate.add(res.status >= 400)
  check(res, {'status == 200': (r) => r.status === 200 });

  sleep(1);
};
