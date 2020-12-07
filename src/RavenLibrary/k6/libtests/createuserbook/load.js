import http from 'k6/http';
import { check, group, sleep, fail } from 'k6';
import { Rate } from 'k6/metrics'
import faker from 'https://cdnjs.cloudflare.com/ajax/libs/Faker/3.1.0/faker.min.js';

let errorRate = new Rate('error_rate')

export let options = {
  vus: 1, // 1 user looping
  duration: '5s',

  thresholds: {
    http_req_duration: ['p(99)<5000'], // 99% of requests must complete below 1.5s
  },
};

export default () => {
  let headers = { 'Content-Type': 'application/json' };

  let randomUserRes = http.get(`${__ENV.BASE_URL}/user/random`);
  let randomUserId = JSON.parse(randomUserRes.body).id;

  let randomBookRes = http.get(`${__ENV.BASE_URL}/books/random`);
  let randomBookId = JSON.parse(randomBookRes.body).id;

  let data = 
  {
    user: randomUserId,
    book: randomBookId
  }

  let res = http.post(`${__ENV.BASE_URL}/userbook`, JSON.stringify(data), { headers: headers });

  errorRate.add(res.status >= 400)
  check(res, {'status == 200': (r) => r.status === 200 });

  sleep(1);
};
