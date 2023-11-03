# ANDROMEDA_VR
VR Visualizer using music bais in real time

**# Documentation of the unit's evaluative evidence.

## Link to video on YouTube

Iteration 1: https://youtu.be/VX_d2hwmXvA

Iteration Final: https://youtu.be/i_KdVV2HQcw

## Solution explanation

## Objective of the work:

It consists of developing a visualization program where, through interaction with music, an environment is procedurally deformed, that is, it will never be the same environment under the same conditions, in this case with the music that will be revived through the bits of the song to shape the context, in this case through a VR

![image](https://github.com/jfUPB/evaluaciones-2023-20-NoName588/assets/84156615/7aace593-d436-4286-9119-7ac7bbc1988d)


## Context:

MISSION APOLLO 75: you are stuck at the bottom of space while you wait for a star to pass close to your ship to get back on track, while you enjoy the views of space that is taking different shapes while you appreciate its beauty and exotic figures

## Mathematical concepts:

1. The Cubes: In the code you provided, the object is changing its scale between two values (restScale and beatScale) in response to beat events. If these changes are made at a constant rate and alternate between the two scale values, it could be perceived as a gentle oscillation in the scale of the object.

```c#
void Update()
     {
         // Calculate position as a function of time using the sine function
         float yOffset = amplitude * Mathf.Sin(2 * Mathf.PI * frequency * Time.time);
        
         // Applies the wobble to the object's position
         transform.position = initialPosition + new Vector3(0, yOffset, 0);
     }
```

2. Particles: The particle system was used to create the effects of star dust and waves from the top of the ship, this system is controlled by a code connected to the music inputs to determine by means of a synchronizer which gives it a random channel and then acts according to the assigned music channel

```c#
     public override void Start()
     {
         base.Start();
         particleMainModule = Ps.main;
     }

     public override void OnUpdate()
     {
         base.OnUpdate();

         if (m_isBeat) return;
     }

     public override void OnBeat()
     {
         base.OnBeat();

         // Create a new particle emission on each beat.
         Ps.Emit(emVal); // represents the amount of particles that are emitted in each beat.
         m_isBeat = false; // Reset the beat flag.
     }
}
```
3. Perlin noise: This is used for the implementation of the deformation of the created terrain, it can be seen how the object that begins to surround the ship, like the particles, is deformed by means of the bits, each music channel It affects the terrain differently and since it is placed randomly then the result may be different.
```c#
void ModifyTerrain()
{
     TerrainData terrainData = terrain.terrainData;
     int width = terrainData.heightmapResolution;
     int height = terrainData.heightmapResolution;
     float[,] heights = terrainData.GetHeights(0, 0, width, height);

     int halfWidth = width / 2;
     int halfHeight = height / 2;

     for (int x = 0; x < halfWidth; x++)
     {
         for (int y = 0; y < halfHeight; y++)
         {
             float sample = audioSpectrum[x] * audioSpectrum[y] * sensitivity;
             ModifyHeight(heights, x, y, sample);
             ModifyHeight(heights, width - x - 1, y, sample);
             ModifyHeight(heights, x, height - y - 1, sample);
             ModifyHeight(heights, width - x - 1, height - y - 1, sample);
         }
     }

     terrainData.SetHeights(0, 0, heights);
}
```
4. Prodecural system: according to what is understood, the result must be in such a way that the same result does not always come out, in this case a random was implemented in which in a special way it assigns values to the Bais channels which will never be seen an interaction equal to each music channel that is assigned to each interaction element:
```c#
    public virtual void Start()
     {
         bias = Random.Range(0f, 70f);
     }
```
5. Rotations: The concept of rotation was implemented to show how the planets and elements orbited around the spacecraft to give an effect that it is in space and that the ship is moving and at the same time avoid a static effect. That would ruin the experience:

```c#
void Update()
{
     for (int i = 0; i < hexagons.Count; i++)
     {
         hexagons[i].transform.RotateAround(centerObject.transform.position, Vector3.up, orbitSpeed * Time.deltaTime);

         // Apply changes to the X, Y and Z axes to the hexagon
         Vector3 eulerAngles = hexagons[i].transform.eulerAngles;
         eulerAngles.x += rotationSpeeds[i].x * Time.deltaTime;
         eulerAngles.y += rotationSpeeds[i].y * Time.deltaTime;
         eulerAngles.z += rotationSpeeds[i].z * Time.deltaTime;
         hexagons[i].transform.eulerAngles = eulerAngles;
     }
}

```

6. Music Syncronicer: this part read the beats of the song and give for each code a diferent sound channel to make diferent interaccion by theirselfs.
   
All the concepts used were selected to give an important effect on the environment in an effective and at the same time sensible way so that any other person can replicate it from the comfort of their computer.
## Obstacles and errors that appear

-During the development the system of particles in orbit had a fatal error, they did not move around the orbits of the planets, therefore the idea of ​​putting particles to orbit if not to levitate around the ship without affecting the planets

-The sound control system through VR interaction was discarded since a kit could not be obtained in the established time, so the music change was done manually

-The terrain, using Perlin's ruin concept, never restarted when the scene was played again, the variable multiplied more and more, so it was determined through the code to reset the variables of the scene. ground height

## portfolio link
https://pie-action-566.notion.site/Andromeda-Project-72d31c89494a4b679da8c8381ae78cdd?pvs=4**
