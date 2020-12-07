import http from 'k6/http';
import { check, group, sleep, fail } from 'k6';
import { Rate } from 'k6/metrics'
import faker from 'https://cdnjs.cloudflare.com/ajax/libs/Faker/3.1.0/faker.min.js';

let errorRate = new Rate('error_rate')

export let options = {
  stages: [
    { duration: '10s', target: 500 }, // simulate ramp-up of traffic
    { duration: '30s', target: 500 }, // stay at top limit of users
    { duration: '10s', target: 0 }, // ramp-down to 0 users
  ],

  thresholds: {
    http_req_duration: ['p(99)<500', 'p(99.9)<1000'],
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
