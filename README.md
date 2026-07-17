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
[Task 1 folder in task branch](https://github.com/MickWPM/VR-AR-Dev/tree/777238fe439d4a6106448adc9f84a34e0eae74d6/Task-1)

Video:
[Task 1 summary video](https://youtu.be/fLf8Xr39Rd8)

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
[Task 1 folder in task branch](https://github.com/MickWPM/VR-AR-Dev/tree/b055d9f89b266996847a809e8fb802a406dc321c/Task-2)

Video:
[Task 1 summary video](https://youtu.be/CQKCS2rD5ag)
