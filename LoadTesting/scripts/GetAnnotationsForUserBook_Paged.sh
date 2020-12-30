#!/bin/bash

executeLoadTest () {
  printf "\nLoad test for GetAnnotationsForUserBook_Paged with $1 concurrent users\n"
  ../wrk2/wrk -d5m --latency -R $1 -s getAnnotationsForUserBook_Paged.lua $2 > getAnnotationsForUserBook_Paged_$1.txt
}

executeLoadTest 250 $1
executeLoadTest 500 $1
executeLoadTest 750 $1
