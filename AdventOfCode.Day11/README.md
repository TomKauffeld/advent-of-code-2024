# [Advent of Code 2024](../README.md) - [Day 11: Plutonian Pebbles](https://adventofcode.com/2024/day/11)

## Part 1
The first way was to just brute force it by following the rules 25 times.  
Then counting the amount of items in the list at the end to find the awnser.  

## Part 2
For this part I've started using bruteforce. However the application was at more than 10GB of memory use
and started to take a minute to find the next item at only index 40. So this has been stopped in order to find 
a more eloquent solution.  
The first step as the complete calculations are too long was to change the loop to a recursive function that just
calculates the amount of stones after a certain amounts of steps.  
Once this method was created, a cache was added to remember the result for each number with a depth remaining.  
With the recursive function together with the cache, the calculation was sped up, and the result could be calculated.

In the end I needed to sleep a night over the problem in order to find the idea to use a recursive function.
So the total time spent is large, however this was an interesting problem to try and solve.