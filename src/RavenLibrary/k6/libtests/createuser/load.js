import http from 'k6/http';
import { check, group, sleep, fail } from 'k6';
import { Rate } from 'k6/metrics'
import faker from 'https://cdnjs.cloudflare.com/ajax/libs/Faker/3.1.0/faker.min.js';

let errorRate = new Rate('error_rate')

export let options = {
  vus: 1, // 1 user looping
  duration: '20s',

  thresholds: {
    http_req_duration: ['p(99)<5000'], // 99% of requests must complete below 1.5s
  },
};

const BASE_URL = 'https://localhost:5001';

const COMMON_REQUEST_HEADERS = {
  'Accept': 'application/json',
  'Content-Type': 'application/json',
  accept: '*/*'
};


export default () => {
  const url = 'https://localhost:5001/user';
  let headers = { 'Content-Type': 'application/json' };
  let data = { name: faker.name.findName(), karma_comments: 15, karma_links: 66 };
  let res = http.post(`${BASE_URL}/user`, JSON.stringify(data), { headers: headers });

  headers = { 'Content-Type': 'application/x-www-form-urlencoded' };
  res = http.post(url, data, { headers: headers });

  errorRate.add(res.status >= 400)

  check(res, {'status == 200': (r) => r.status === 200 });


  sleep(1);
};
