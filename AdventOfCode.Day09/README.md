# [Advent of Code 2024](../README.md) - [Day 9: Disk Fragmenter](https://adventofcode.com/2024/day/9)

## Part 1
I was afraid there would be too much data, but in the end the "easy" solution worked:  
We begin by blowing up the text to create a list of numbers where each position is the position on the "disk"  
and each number is the "block ID" associated (with -1 if it's an empty space).  
Once we have this list, we compact it just like in the example:  
- loop backwards through the list using the index `scan_location`
- if the value smaller than 0, go to the next number
- else find the smallest index `target_location` in the list where the "block ID" is -1
- if `target_location` is bigger than `scan_location`, go to the next number
- copy the value from `scan_location` to `target_location`
- set the value at `scan_location` to -1

When the list is compacted, create a new list where each item is equal to the index times the value at that index
from the first list.  
And lastly sum the values from the new list, and we have our answer.  

(the code is written exactly as described, however the result is the same, but more optimised)

## Part 2
This works almost the same way as for part 1.  
But this time when we have our value, we check the length of the block.  
And when searching for the empty space, we search for the first location with at least `length` empty spaces after it.  
Then we copy the whole block.  
And our next index will directly be the index just before the start of the block.  
The sum is also the same.
**Just be sure to check the value isn't -1 when multiplying and adding it to the result !!**
(this took me far too long to realize why the result was too small)