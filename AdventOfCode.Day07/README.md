# [Advent of Code 2024](../README.md) - [Day 7: Bridge Repair](https://adventofcode.com/2024/day/7)

## Part 1
Bruteforce for the win :)  
The idea is to first see the operations as a list of bits, where every bit indicates:
- 0: do a multiplication
- 1: do a sum

Then by knowing there must be `list.size - 1` operators, we know there are `2 ^ (list.size - 1)` possible
ways to arrange them. So we create a list from 0 to `2 ^ (list.size - 1)` and check for each if they are the solution
by taking the last bit of the operators number for each number and then shifting the bits for the next number.

When we've found a soltion for a list, we add the number to a list.  
And at the end, we just have to sum the numbers in the list.

## Part 2
Almost the same as last time, except as we have 3 operators:
- 00: do a multiplication
- 01: do a sum
- 10: do a concatination
- 11: invalid operation, skip the solution

Having the invalid operation makes creating the list easier as this means we always have 2 bits for the operation,
and so we have `4 ^ (list.size - 1)` possible ways to arrange them.

Once we have the list, we do the same thing as for part 1 again, but this time we take 2 bits every time.

Add the possible solutions to a list, and perform a sum to get the final answer.