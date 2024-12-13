# [Advent of Code 2024](../README.md) - [Day 13: Claw Contraption](https://adventofcode.com/2024/day/13)

## Part 1
Started with brute force again...  
The idea is to loop over every prize, and for each we check what happens if we press x times the A button
(with x between 0 and 100 inclusive).  
To find the amount of times we press the B button, we can divide the remainder of the Prize - x * A by B.
If this is a solution we keep it with the cost.  
Then we just have to take the smallest cost if it exists, and add it to our final sum which will be our
answer.

## Part 2
We have to do maths :'(  
For each prize we have a system with 2 equations and two unknown variables:

```
A * a_x + B * b_x = p_x
A * a_y + B * b_y = p_y
```

And we need to solve the equations for `A` and `B`, so we can try to isolate `B` on the second line:

```
A * a_x + B * b_x = p_x
B * b_y = p_y - A * a_y
```
```
A * a_x + B * b_x = p_x
B = (p_y - A * a_y) / b_y
```

Then we can replace `B` on the first line with the equation on the second line:

```
A * a_x + ((p_y - A * a_y) / b_y) * b_x = p_x
B = (p_y - A * a_y) / b_y
```

And try to isolate A on the first line:

```
A * a_x + ((p_y - A * a_y) / b_y) * b_x = p_x
B = (p_y - A * a_y) / b_y
```
```
A * a_x + ((p_y - A * a_y) * b_x) / b_y = p_x
B = (p_y - A * a_y) / b_y
```
```
A * a_x + (p_y * b_x - A * a_y * b_x) / b_y = p_x
B = (p_y - A * a_y) / b_y
```
```
A * a_x * b_y + p_y * b_x - A * a_y * b_x = p_x * b_y
B = (p_y - A * a_y) / b_y
```
```
A * (a_x * b_y - a_y * b_x) + p_y * b_x = p_x * b_y
B = (p_y - A * a_y) / b_y
```
```
A * (a_x * b_y - a_y * b_x) = p_x * b_y - (p_y * b_x)
B = (p_y - A * a_y) / b_y
```
```
A = (p_x * b_y - (p_y * b_x)) / (a_x * b_y - a_y * b_x)
B = (p_y - A * a_y) / b_y
```

Lastly as we are working with int64s, we need to check if the solution is valid:

```
t_x = A * a_x + B * b_x
t_y = A * a_y + B * b_y
```

And we check if ``t_x == p_x`` and ``t_y == p_y``.  
If this is the case, we can add to the final cost `A * 3 + B` and we have our answer