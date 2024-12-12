# [Advent of Code 2024](../README.md) - [Day 12: Garden Groups](https://adventofcode.com/2024/day/12)

## Part 1
This was done using a multi step process:
- Calculate the regions and give each an unique id.
	- loop through each location, and if it isn't already associated to a region give it the next region ID.
		- if it's a new region check the 4 squares around it and if they have the same "plant" give it the same
		  ID.
		- recheck the 4 regions around each new location found until no new locations are found.
- Calculate for each region the area size and perimeter.
	- loop through each location.
		- add 1 to the area for the associated region ID.
		- check the 4 regions around, and for each that is associated with a different region ID, add
		  one to the perimeter of the current region ID.
- Calculate for each region the price by multiplying the area and perimeter.
- Sum the prices, and we have our awnser.

## Part 2
The idea is the same as the first one, except we don't calculate the perimeter, but the amount of sides:  
For this we do the same check as the perimeter as before, but this time we also check the locations around to see
if it's an extension.  
For the location on top, we can check if the location to the left is the same,
but the one to the top left is different.  
For the location on the left, we can check if the location on top is the same,
but the one to the top left is different.  
For the location on the bottom, we can check if the location to the left is the same,
but the one to the bottom left is different.  
For the location on the right, we can check if the location on the top is the same,
but the locaion on the top right is different.  
When these extra checks returns true, we don't add it as it is an extension to an already existing fence.  
Once the sides and areas are calculated, the price is calculated for each region by multiplying the sides by the area.  
And we sum the prices to get our awnser.