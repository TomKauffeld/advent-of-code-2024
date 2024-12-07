# [Advent of Code 2024](../README.md) - [Day 6: Guard Gallivant](https://adventofcode.com/2024/day/6)

## Part 1
We can read the input as a field, then we take our guard position and rotation and just let him "walk" his path.  
For every position he goes, we add it to a list if it's not yet there.  
At the end we can just count the items in the list.

## Part 2
We start the same way as last time, except of counting at the end, we remove the starting position from the list and
then try to set a wall at each point seperatly and check if the guard gets stuck in a loop.  
To see if he's stuck, we keep every location he has been with a certain rotation, then we check if he has been there
before with the same rotation (which means he is in a loop).  
At the end we count the amount of times he was stuck in a loop.