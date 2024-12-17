# [Advent of Code 2024](../README.md) - [Day 17: Chronospatial Computer](https://adventofcode.com/2024/day/17)

## Part 1
For this I've just created a VM that executes the instructions as defined in the puzzle.  
When executed this gives the answer :)


## Part 2
Brute force is not the way :'(  
The first idea was to just test different inputs for the A register, however the number is at least 550'000'000 already.
So this probably won't give the answer in time to solve the puzzle.

The next idea will be to find how the output is influenced by the A register.  
By checking for each item in the list what the conditions for the A register are.
And then combining these conditions to find the final number.