#!/bin/bash

executeLoadTest () {
  printf "\nLoad test for getAnnotationsForUser_Paged with $1 concurrent users\n"
  ../wrk2/wrk -d5m --latency -R $1 -s getAnnotationsForUser_Paged.lua $2 > getAnnotationsForUser_Paged_$1.txt
}

executeLoadTest 250 $1
executeLoadTest 500 $1
executeLoadTest 750 $1
