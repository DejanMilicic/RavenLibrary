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

let users = JSON.parse(open('./data.json'));

export function setup() {
  return { users: users }
}

export default (data) => {
  let entry = data.users[__VU % data.users.length];
  
  var midSection = entry.id.split("/")[2] + '';
  let userId = "users/" + midSection.substring(0, midSection.lastIndexOf("-"));
  let annotations = entry.annotations;

  let pageSize = 10;
  let totalPages = Math.floor(annotations / pageSize) + 1;

  for (let page = 0; page < totalPages; page++) {
    let skip = page * pageSize
    
    let res = http.get(`${__ENV.BASE_URL}/annotations/user/${skip}/${pageSize}?userId=${userId}`);

    //console.log(res.body);

    errorRate.add(res.status >= 400)
    check(res, {'status == 200': (r) => r.status === 200 });
  
    sleep(1);
  }
};
