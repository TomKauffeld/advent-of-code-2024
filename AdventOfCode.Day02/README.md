# [Advent of Code 2024](../README.md) - [Day 2: Red-Nosed Reports](https://adventofcode.com/2024/day/2)

## Part 1
This one could be solved by checking each list by first
taking the first two items to determine if it's increasing or decreasing.  
Then by checking each item after the first index to see if it continues to increase/decreate
and to get the difference between them.  
This created a new list with 1 if the items respect the rules and 0 otherwise.  
Then a ``.Sum()`` can be used to count the correct reports.  
(Yes, a ``.Count()`` could also have worked if the check returned a bool instead)

## Part 2
By reusing the first solution, but adapted to return the index where the error has been found,
We can try to create 3 lists each with missing one of the 3 preceding indexes and check those lists
as well.  
If any of the 4 lists returns it's safe, we know it's safe, otherwise it's unsafe.  
Using the same ``.Sum()`` we find to final answer.