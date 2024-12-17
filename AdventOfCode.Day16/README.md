# [Advent of Code 2024](../README.md) - [Day 16: Reindeer Maze](https://adventofcode.com/2024/day/16)

## Part 1
A day late, but here we are:  
For this the idea was to create a directed weighted graph where  
each node represents a location and a faced direction.  
and each edge has the cost of 1 if you get there by moving forward, and 1000 if it's a rotation.

Then we can use an algorithm to find the "shortest" path in a directed weighted graph without negative costs.  
Where I've used Dijkstra's algorithm (not the fastest, but easy to implement) with the condition to
stop early if the target location has been found.

Once the path has been found, we get the total cost of the path which will be **almost** our answer:  
Because the condition to find the path has been written for the node on the location facing UP, we need to check  
if the previous location in the path was not the same (but with a different direction), if it is we need to remove
the cost of one rotation, and check the location before that one.  
When we find a different location, we have the final answer.


## Part 2
By modifing the algorithm to return every equal path by not only saving the previous node, but a list of previous nodes
where when we check the new potiential cost of a node:  
If the new cost is lower, then we replace the previous nodes and only add the new one.  
If the new cost is the same, then we add the new node to the previous list.

Once we have the paths, we check for each unique location (x, y without rotation), 
and the count of them will be the answer.