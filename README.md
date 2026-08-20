# SIT756 - Development for Virtual and Augmented Reality - Assessment 1

## Scenario 1 - Conveyer Belt

Features:
- Hand grabbable interaction
- Conveyer belt motion (Set velocity directly over addforce for 'nicest' behaviour when interacting)
- Item type (custom enum)
- "Bins" for objects to fall/be placed in to.
	- Can be set up to accept specific object type only and optionally destroy incorrect types that enter
	- Uses event callbacks when correct object enters the bin and on destruction of incorrect object.
- "Scoreboard" using world space UI, updated based on bin event callbacks.
- Custom physics material to improve belt slide feel and add a little bounce for juice.

The custom item type and consumer allows trivial extension to support any number of items. The event based consumer approach using the enum type supports any additional types by default and using the event chain we can remain decoupled and hook in any additional functionality related to item conumption we wish (eg. SFX, VFX, UI, subsequent experience triggers etc)

Code:
[Task 1 folder in task branch](https://github.com/MickWPM/VR-AR-Dev/tree/37504e23bd5aad5234c0f0e8690a55d03b8c20c6/Task-1)

Video:
[Task 1 summary video](https://youtu.be/Z60_fYfdPLE)

## Scenario 2 - Paint Gun

Features:
- State based gun controller to manage colour sampling vs firing
- Colour sample surface compoent to enable any object to be sampleable; requirements are simply that the material mainTexture property is defined; the sampling uses the interpolated UV coordinates based on the world sample location.
- Painter Canvas component to allow any object to act as a painting canvas. The canvas stores all painted objects as children to enable canvas reset. This functionality also allows GetComponentInParent<PainterCanvas> to be used to allow paint objects to stick to each other; if this is not desired behaviour, using GetComponent<PainterCanvas> instead will only allow collisions with the base canvas collider to register.
- Painter component developed to allow any object to act as a painter.
- Painting bullet leverages the painter component and separate bullet script with modifiable local gravity force for generalisability.
- UnityEvents leveraged over C# events to maintain fully decoupled code (eg. Painter and Bullet)

As an example of extensibility; a "paintbrush" effect could be easily developed by creating a small emitter (likely an object that is just parented to the brush and replaced once it is removed) using a painter variant that used GetComponent instead of GetComponentInParent to allow "brushstrokes".


Code:
[Task 2 folder in task branch](https://github.com/MickWPM/VR-AR-Dev/tree/b055d9f89b266996847a809e8fb802a406dc321c/Task-2)

Video:
[Task 2 summary video](https://youtu.be/CQKCS2rD5ag)


## Scenario 3 - Aircraft controller

Features:
- Spline based aircraft paths with dynamic position readjustment based on spline length change to preserve relative world space location
- Aircraft mid air and ground collision with effects. Collision matrix customised to maximise control over interactions
- Event based callbacks on spawn/landing/collision
- Spawn manager spawns in random locations from 'probability distribution' of candidate aircraft
- 3 aircraft type with unique speed, collision sizes and path visualisations
- Dynamic spline connection between aircraft flight waypoints and landing approach/landing static waypoints
- Waypoint interaction disabled once aircraft pass
- Experience manager event based communication pipeline 
- "Scoreboard" using world space UI, updated based experience manager events

Communication is managed through events; unity events where static scene objects exits, C# manual events where required dynamically at runtime. This allows significant extensibility. Aircraft type are generalised and can be extended trivially with new models, flight speed, waypoint quantities and more.

Code:
[Task 3 folder in task branch](https://github.com/MickWPM/VR-AR-Dev/tree/d7c81b53be82a0c291166700022b4a5d031343cc/Task%203)

Video:
[Task 3 summary video](https://youtu.be/zltqQmo73zE)

## Scenario 4 - Alchemy

Features:
- Created visuals for Flame, Steam, Storm, Frost, Mud, Vine, Sand, Ember, Spark, Smoke, Stone, Ash, Crystal, Glow, Water, Ice
- Implemented texturing, particles, custom shaders, custom 2D art and 3D models
- Custom shader detail:
  - Mud - Used texture heightmap to show varying level of water
  - Water - Used world space up and sine wave to dynamically display varied levels of water inside a container
  - Ash - Dissolve over time using noise including edge threshold to show "fire" edges
  - Vine - Custom model to 'grow' the vine based off model UV coordinate using emission to fake volume
  - Crystal - Custom model with crystal faces scaled to full UV to allow edge highlighting. Scene colour sampling to emulate refraction
- Interaction including:
  - complete vine growth/retraction when consuming water/fire.
  - Mud water level slowly changing when exposed to water/fire
  - Water level slowly lowering while producing steam when exposed to fire

The interaction approach leveraged scriptable objects for element definition (over eg. enum) to maximise future scalability. Element consumption through exposed events allows designer friendly interactions (both on collision and stay) while allowing for more advanced specific implementations through code. 


Code:
[Task 4 folder in task branch](https://github.com/MickWPM/VR-AR-Dev/tree/2290693237b3b1acd222813db99dd14ed5f71a82/Task%204)

Video:
[Task 4 summary video](https://youtu.be/GV403oGluyU)



## Scenario 6 - Fishtank

Features:
- Pure C# states, defined by interfaces with senses propogated through C# events
- Dependency injection through state constructors supports flexibility in state design while adhering to Entry, Update and Exit state calls
- Extensible through integration of additional senses and states as well as arbitrary sense (events) processing; transitions are not just immediate on event fire
- Scenario 4 lessons brought in to animate fish using vertex offsets with custom shader which supports arbitrary offsets for threshold between 'stable' body and 'swimming' tail.
- Integrated dynamic rigidbody updates for 'fish food' dropped in from above the tank
- Integrated player spawning of fish and food to influence ecosystem 

The event based architecture of the state machine supports optional components for sensing/event behaiour, in line with the Unity component system approach. The tradeoff for this is the `brain' script requires additional conditional setup for each new component but this affords the design time flexibility of mixing and matching sensing components. 

Code:
[Task 6 folder in task branch](https://github.com/MickWPM/VR-AR-Dev/tree/c278a2c9164dab67347cb2b93fbbcacadf745f2f/Task%206)

Video:
[Task 6 summary video](https://youtu.be/QWv0F3EjT3A)
