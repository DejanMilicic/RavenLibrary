#!/bin/bash

executeLoadTest () {
  printf "\nLoad test for getAnnotationsForUser_All with $1 concurrent users running for $2 on $3 \n"
  ../wrk2/wrk -d $2 --latency -R $1 -s getAnnotationsForUser_All.lua $3 > getAnnotationsForUser_All_$1_$2.txt
}

executeLoadTest 250 $1 $2
executeLoadTest 500 $1 $2
executeLoadTest 750 $1 $2