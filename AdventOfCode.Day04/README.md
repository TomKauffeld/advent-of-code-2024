# [Advent of Code 2024](../README.md) - [Day 4: Ceres Search](https://adventofcode.com/2024/day/4)

## Part 1
For the input, we can see it as a field of characters, then by checking from each x, y location
we check if the character is X at the location and then take the 6 strings around it starting with
the X if any is equal to XMAS
```
\  |  /
 \ | /
  \|/
---X---
  /|\
 / | \
/  |  \
```

## Part 2
By using the same field of characters, we can check if the character is X at the location and
then the two strings around in a cross if they are both equal to MS or SM.
```
1.2
.A.
2.1
```