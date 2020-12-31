#!/bin/bash

executeLoadTest () {
  printf "\nLoad test for GetAnnotationsForUserBook_All with $1 concurrent users\n"
  ../wrk2/wrk -d5m --latency -R $1 -s getAnnotationsForUserBook_All.lua $2 > getAnnotationsForUserBook_All_$1.txt
}

executeLoadTest 250 $1
executeLoadTest 500 $1
executeLoadTest 750 $1
