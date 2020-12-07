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

  let subjectArr = faker.lorem.words(faker.random.number({min: 1, max: 9})).split(' ');
  let languageVal = faker.random.arrayElement(["en","de","en","es","en"]);
  let bookshelfArr = faker.lorem.words(faker.random.number({min: 1, max: 4})).split(' ');
  let creatorArr = new Array(faker.random.number({min: 1, max: 4})).fill(null)
          .map(e => e = faker.name.findName());

  let data = { 
    subject: subjectArr, 
    language: [ languageVal ],
    bookshelf: bookshelfArr,
    tableOfContents: faker.lorem.sentence(faker.random.number({min: 4, max: 15})),
    downloads: faker.random.number({min: 0, max: 150000000}),
    creator: creatorArr,
    publisher: faker.company.companyName(),
    issued: faker.date.past(faker.random.number({min: 0, max: 450})).toISOString().slice(0, 10),
    title: faker.lorem.sentence(faker.random.number({min: 5, max: 12})),
    rights: faker.lorem.sentence(faker.random.number({min: 5, max: 7})),
    type: faker.random.arrayElement(["Text","Text","Image","Text","Text","Text"])
  };
  
  let res = http.post(`${__ENV.BASE_URL}/book`, JSON.stringify(data), { headers: headers });

  errorRate.add(res.status >= 400)

  check(res, {'status == 200': (r) => r.status === 200 });


  sleep(1);
};
