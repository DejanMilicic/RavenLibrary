#!/usr/bin/python3

import sys

for fname in sys.argv[1:]:

    file = open(fname, "r")
    data = file.read()
    file.close()

    start_key = "Latency Distribution (HdrHistogram - Recorded Latency)"
    start = data.index(start_key) + len(start_key)
    end = data.index("Detailed Percentile spectrum:")

    data = data[start:end]
    lines = data.splitlines()
    print(fname, end =", ")
    for line in lines:
        v = line.split()
        if len(v) < 2:
            continue
        print(v[1], end =", ")

    print()