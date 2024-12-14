# [Advent of Code 2024](../README.md) - [Day 14: Restroom Redoubt](https://adventofcode.com/2024/day/14)

## Part 1
To go faster, the way we could find the position of a robot after `S` steps / seconds,
We can take the original position `P` of the robot and add the velocity `V` times the steps:  
`T = P + V * S` then we need to clamp it to remain inside the field:  
`F.x = T.x % Width`  
`F.y = T.y % Height`  
And then check if the value is negative, and if it is, add it to the `Width` and `Height`.  
At the end when we have the final location `F` of each robot, we can count them from each quadrant.

## Part 2
So this one was fun to see what the result was, however as a puzzle I like it less:  
In my own opinion, I like to we able to find a solution by just having the test examples, and the question.  
Whereas with this one, we didn't directly even know what we were looking for.  

Anyway, to solve it, this solution creates an Image for every step and saves it as `image_{step}.png`.  
Then this was first looped for 1000 steps, but didn't find anything (other than a sort of loop where the robots
got together to form some sort of line).  
After setting it to 10000 steps, and looking at windows explorer for far too long, I've found the 
Christmas tree, looked at the name and got the result.