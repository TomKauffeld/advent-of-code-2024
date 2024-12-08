# [Advent of Code 2024](../README.md) - [Day 8: Resonant Collinearity](https://adventofcode.com/2024/day/8)

## Part 1
The idea was to create a list of every type of antenna associated with their own list of positions.  
Then for each frequency, take 2 positions from the list, find the offset of the second position compaired to
the first, add it to the second position, and substract it from the first position.  
These two positions are added to a list.  
At the end remove all positions that are out of bounds.  
When you count the unique positions found, this will be the anwser.

## Part 2
Almost the same as the first part, except this time once we have the offset, we take an index from 0 to
to "infinity" (until the position found is out of bounds) and multiply it with the offset and add that to the
first position. Once we are out of bounds, we do the same but from -1 to "negative infinity".  
All these positions are added to the results list.  
When you count the unique positions found, this will be the anwser.