#!/bin/bash

for var in "$@"
do

  executeLoadTest () {
    printf "\nLoad test for $var with $1 concurrent users running for $2 on $3 \n"
    ../wrk2/wrk -d $2 --latency -R $1 -s $var $3 > $var_$1_$2.txt
  }

  executeLoadTest 250 $1 $2
  executeLoadTest 500 $1 $2
  executeLoadTest 750 $1 $2


done