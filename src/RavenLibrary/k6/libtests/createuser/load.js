import http from 'k6/http';
import { check, group, sleep, fail } from 'k6';
import { Rate } from 'k6/metrics'
import faker from 'https://cdnjs.cloudflare.com/ajax/libs/Faker/3.1.0/faker.min.js';

let errorRate = new Rate('error_rate')

export let options = {
  vus: 1, // 1 user looping for 1 minute
  duration: '5s',

  thresholds: {
    http_req_duration: ['p(99)<1500'], // 99% of requests must complete below 1.5s
  },
};

const BASE_URL = 'https://localhost:5001';

const COMMON_REQUEST_HEADERS = {
  dnt: '1',
  'user-agent': 'Mozilla/5.0',
  'content-type': 'application/json',
  accept: '*/*',
  origin: BASE_URL,
  referer: BASE_URL
};


export default () => {
  //let productName = faker.name.findName();

  let res = http.post(`${BASE_URL}/user`, `{'name': 'john doe', 'karma_Comments':2, 'karma_Links':25}`, {
    headers: COMMON_REQUEST_HEADERS
  });
  errorRate.add(res.status >= 400)

  check(res, {'status == 200': (r) => r.status === 200 });

  sleep(1);
};
