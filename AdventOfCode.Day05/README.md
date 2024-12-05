# [Advent of Code 2024](../README.md) - [Day 5: Print Queue](https://adventofcode.com/2024/day/5)

## Part 1
By iterating each print job, we can first create a list of applicable rules,
then for each rule, we check the index of the first item against the index of the second item.  
If the first index is larger than the second index, we know the rule was not respected.  
If the first index is smaller than the second index, we know the rule was respected, and can check the next rule.  
Once we know if the job respects the rules, we can take the middle item of the job, and add it to the results list  
in order to sum them at the end and get our awnser

## Part 2
The first part is the same at Part 1, however this time once we know it job doesn't respect the rules  
we can run the rules again.
But this time, when we check the index of the first item against the index of the second item, if it isn't respected
we change the items in the job against each other.  
We continue to correct each job until no further corrections are made.  
Once the list is corrected, we again take the middle item of the job, add it to the results list, and sum them to get
our awnser.  
(the job must be able to be sorted for this technique, else the program will never finish and is stuck in a infinite loop)