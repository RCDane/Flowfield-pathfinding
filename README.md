# Flowfield pathfinding and dungeon generation
What is this project? \
An example project of a water shader

# Goal / Motivation
This project was made as my programming c exam project during my time at Game College Grenaa, Which is a HTX(High school) education in denmark focused on enginering and game development.

The project started out as a dungeon generator focused on placing rooms and then connecting them, After having a basic system of placing rooms and creating doorways between them, I started creating the pathfinding system which then became the main focus.


# Features:
- Dungeon generation
- Connection algorithm that connects rooms until two rooms predefined rooms are connected
- Flow Field Generation
- Pathfinding using said Flow Field
- Quadtree data structure for logarithmic look up in flow field
- Quadtree visualizer for 
- built in debugging console for easier switching between debug views
- Basic particle movement using flowfield and unity Rigidbodies
# Example
FlowField example scene:
Particles using the flowfield to move level, the current bottleneck of the system is unity slowing down considerably when a big set of rigidbodies trying to move when near eachother.
![FlowField](https://github.com/RCDane/Flowfield-pathfinding/blob/master/Images/flowfield%20example.gif)
Quadtree example scene:
For this project it was necessary for each particle to find the nearest flowfield point to get it's movement vector. here is the quadtree being visualized, while getting points added to it in a painting like manner.
![FlowField](https://github.com/RCDane/Flowfield-pathfinding/blob/master/Images/quadtree%20example.gif)

Since the programs pathfinding system running time would scale by n*k using a brute force algorithm for flowfield lookup, where n the amount of particles and k is the size of the flowfield. Since the lookup in a quadtree scales logarithmically as the flowfield gets bigger, the algorithm can go from n*k to n*log(k)


Tested in Unity 2018.4.12

