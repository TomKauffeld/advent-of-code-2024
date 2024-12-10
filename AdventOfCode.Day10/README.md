# [Advent of Code 2024](../README.md) - [Day 10: Hoof It](https://adventofcode.com/2024/day/10)

## Part 1
The first part was to create a directional graph where the nodes are the values, and the
edges are from nodes with value `v` to nodes around it with the value `v + 1`.  
Once the graph was created. We need to find all starting positions, and from each of them
run through the graph to find the unique amount of nodes with the value 9 we can find.  
At the end count all the trails, and we have our awnser.

## Part 2
This works almost the same way as for part 1.  
Except we no longer search for unique paths and/or end nodes but use every path found.  
By adding them at the end we have our awnser.