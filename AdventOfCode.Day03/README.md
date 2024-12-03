# [Advent of Code 2024](../README.md) - [Day 3: Mull It Over](https://adventofcode.com/2024/day/3)

## Part 1
By using a regex getting all ``mul(X,Y)`` instances we get every valid multiplication function.  
Then by doing for each found instance X * Y and adding them together, we have the result

## Part 2
We can almost solve it the same way, except the regex now also accepts ``do()`` and ``don't()``.  
When a ``do()`` is seen, the ``enabled`` bool is set to ``true``.  
When a ``don't()`` is seen, the ``enabled`` bool is set to ``false``.
When a ``mul(X,Y)`` is seen and ``enabled`` is set to ``true``
then we multiply the values and add it to the sum.