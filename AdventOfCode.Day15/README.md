# [Advent of Code 2024](../README.md) - [Day 15: Warehouse Woes](https://adventofcode.com/2024/day/15)

## Part 1
For this we build up the map, then when we want to move, we check if we can move what is in front of us.  
In case there is nothing, we can move to the next position.  
In case there is a wall, we can't move.  
In case there is a box, we try to move the box first using the same system (recursively).

Then at the end we get every position and calculate it's "GPS coordinate" and add it to the final sum.

## Part 2
Where we change our map parser to add the new creation rules.  
Then we update our move method to add a check:  
If the position is a LeftBox, we also check the position to the right
(but ignore the fact it is a RightBox, and treat it as a normal Box)  
If the position is a RightBox, we also check the position to the left
(but ignore the fact it is a LeftBox, and treat it as a normal Box)

Then at the end we get every position and calculate it's "GPS coordinate" and add it to the final sum.