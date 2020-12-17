import http from 'k6/http';
import { check, sleep } from 'k6';
import { Rate } from 'k6/metrics'

let errorRate = new Rate('error_rate')

export let options = {
  stages: [
    { duration: '10s', target: 500 }, // simulate ramp-up of traffic
    { duration: '30s', target: 500 }, // stay at top limit of users
    { duration: '10s', target: 0 },   // ramp-down to 0 users
  ],

  summaryTrendStats: ['avg', 'min', 'med', 'max', 'p(90)', 'p(95)', 'p(99)', 'p(99.9)', 'p(99.99)'],

  thresholds: {
    http_req_duration: ['p(99)<500', 'p(99.9)<1000'],
  },
};

let userbooks = JSON.parse(open('./data.json'));

export function setup() {
  return { userbooks: userbooks }
}

export default (data) => {
  let entry = data.userbooks[__VU % data.userbooks.length];
  let userBookId = entry.id;

  let res = http.get(`${__ENV.BASE_URL}/annotations/userbook?userBookId=${userBookId}`);

  console.log(res.body);

  errorRate.add(res.status >= 400)
  check(res, {'status == 200': (r) => r.status === 200 });

  sleep(1);
};
