#!/bin/bash

duration="30s"
url="http://18.194.13.34:5000"

for var in "$@"
do

  [ "json.lua" == $var ] && continue

  executeLoadTest () {
    printf "\nLoad test for $var with $1 concurrent users running for $duration on $url \n"
    ../wrk2/wrk -d $duration --latency -R $1 -s $var $url > "$var-$1-$duration.txt"
  }

  executeLoadTest 250
  executeLoadTest 500
  executeLoadTest 750


done