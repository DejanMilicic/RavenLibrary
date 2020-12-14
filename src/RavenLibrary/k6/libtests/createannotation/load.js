import http from 'k6/http';
import { check, group, sleep, fail } from 'k6';
import { Rate } from 'k6/metrics'
import faker from 'https://cdnjs.cloudflare.com/ajax/libs/Faker/3.1.0/faker.min.js';

let errorRate = new Rate('error_rate')

export let options = {
  stages: [
    { duration: '10s', target: 500 }, // simulate ramp-up of traffic
    { duration: '30s', target: 500 }, // stay at top limit of users
    { duration: '5s', target: 1 },   // ramp-down to 0 users
  ],

  summaryTrendStats: ['avg', 'min', 'med', 'max', 'p(90)', 'p(95)', 'p(99)', 'p(99.9)', 'p(99.99)'],

  thresholds: {
    http_req_duration: ['p(99)<500', 'p(99.9)<1000'],
  },
};

export default () => {
  let headers = { 'Content-Type': 'application/json' };

  let randomUserBookRes = http.get(`${__ENV.BASE_URL}/userbook/random`);
  let randomUserBookId = JSON.parse(randomUserBookRes.body).id;

  let data = 
  {
    userBookId: randomUserBookId,
    text: faker.lorem.words(faker.random.number({min: 1, max: 9})),
    start: faker.random.number({min: 0, max: 250000}),
    note: faker.lorem.words(faker.random.number({min: 1, max: 15}))
  }

  let res = http.post(`${__ENV.BASE_URL}/annotation`, JSON.stringify(data), { headers: headers });

  errorRate.add(res.status >= 400)
  check(res, {'status == 200': (r) => r.status === 200 });

  sleep(1);
};
