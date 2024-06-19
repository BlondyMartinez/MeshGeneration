# Unity Terrain Mesh Generator
## Overview
This is a Mesh Generator script that procedurally generates a terrain mesh using Perlin Noise. The script allows for customization of various parameters to create diverse and unique terrains.

## Features
- Procedurally generates a terrain mesh.
- Customizable parameters for terrain generation:
  - Height multiplier
  - Mesh dimensions (xSize, zSize)
  - Noise scale
  - Octaves
  - Persistence
  - Lacunarity
  - Gradient for vertex coloring based on height
- Real-time updates to the terrain when parameters are changed.

## Getting Started
- Clone the repository.
- Drag and drop the script.
- Setup the Scene:
    - Create a new GameObject in the scene.
    - Attach a MeshFilter and MeshRenderer component to the GameObject.
    - Create a new Material and assign it to the MeshRenderer.
    - Attach the MeshGenerator script to the GameObject.
    - Script Parameters
       - Height Multiplier: Controls the height of the terrain.
       - xSize and zSize: Dimensions of the terrain mesh.
       - Scale: Scale of the Perlin Noise.
       - Octaves: Number of layers of Perlin Noise to add detail.
       - Persistence: Controls the amplitude of each octave.
       - Lacunarity: Controls the frequency of each octave.
       - Color Gradient: Gradient used to color the vertices based on height.

## Demonstration
[![Watch the video](https://img.youtube.com/vi/LZbPeqAh5Mk/0.jpg)](https://www.youtube.com/watch?v=LZbPeqAh5Mk)
