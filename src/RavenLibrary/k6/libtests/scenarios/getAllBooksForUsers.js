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

  summaryTrendStats: ['avg', 'min', 'med', 'max', 'p(90)', 'p(95)', 'p(99)', 'p(99.9)', 'p(99.99)'],

  thresholds: {
    http_req_duration: ['p(99)<500', 'p(99.9)<1000'],
  },
};

export function setup() {
  // sort users by number of books descending
  // fetch first 100.000

  return { "users": [{"id":"users/164496167832"},{"id":"users/417-A"}] }
}

export default (data) => {
  // let randomUserRes = http.get(`${__ENV.BASE_URL}/user/random`);
  // let randomUserId = JSON.parse(randomUserRes.body).id;

  // let res = http.get(`${__ENV.BASE_URL}/books/user?userId=${randomUserId}`);

  // errorRate.add(res.status >= 400)
  // check(res, {'status == 200': (r) => r.status === 200 });

  //const userId = data[__VU];
  //const userId = data[__VU];
  
  //console.log(__VU);
  console.log(__VU, data.users[__VU % data.users.length].id);

  sleep(1);
};
