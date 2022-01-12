# Flowfield pathfinding and dungeon generation
What is this project? \
A Flowfield pathfinding system implemented in unity.

# Goal / Motivation
This project was made as my programming c exam project(Ran from april-may 2018) during my time at Game College Grenaa, Which is a HTX(High school) education in denmark focused on enginering and game development.

The project started out as a dungeon generator focused on placing rooms and then connecting them, After having a basic system of placing rooms and creating doorways between them, I started creating the pathfinding system which then became the main focus.


# Features:
- Dungeon generation
- Connection algorithm that connects rooms until two predefined rooms are connected
- Flow Field Generation
- Pathfinding using said Flow Field
- Quadtree data structure for logarithmic look up in flow field
- Quadtree visualizer for debugging purposes
- built in debugging console for easier switching between debug views
- Basic particle movement using flowfield and unity Rigidbodies
# Example
FlowField example scene:
Particles using the flowfield to move level, the current bottleneck of the system is unity slowing down considerably when using a big set of rigidbodies.
![FlowField](https://github.com/RCDane/Flowfield-pathfinding/blob/master/Images/flowfield%20example.gif)
Quadtree example scene:
For this project it was necessary for each particle to find the nearest flowfield point to get it's movement vector. here is the quadtree being visualized, while points are being added.
![FlowField](https://github.com/RCDane/Flowfield-pathfinding/blob/master/Images/quadtree%20example.gif)

A naive solution would loop through all points in the flowfield, to find the closest point to a particle, the running time of this would be O(n\*k), where n is the amount of particles and k is the size of the flowfield. Since the lookup in a quadtree scales logarithmically as the flowfield gets bigger, the algorithm goes from O(n\*k) to (n\*log(k))


Tested in Unity 2018.4.12

