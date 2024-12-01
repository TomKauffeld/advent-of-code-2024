# [Advent of Code 2024](../README.md) - [Day 1: Historian Hysteria](https://adventofcode.com/2024/day/1)

## Part 1
As the goal is to get the smallest numbers of each list,
the solution was quite easy to implement using any language / framework
other than maybe Assembler.  
Using the ``.Order()`` on the List containing the numbers,
we know they are sorted from the smallest number first to
the biggest number last.  
And for the result list we just go through the list one item at the time and
handle the cases as  
``difference = AbsoluteValue(leftItem, rightItem)``  
And for the last part, using the ``.Sum()`` on the resulting list to get the sum, and by extension, the answer.

## Part 2
As the lists aren't too big, I just used a ``Dictionary`` as a
container to count how many times each number was in the right list.  
Then by running over each number in the left list, we just have to get the
count from the dictionary (and use 0 if not found) and multiply it by the
number from the left list.  
At the end, again just use the ``.Sum()`` on the resulting list to get the sum,
and by extension, the answer. 