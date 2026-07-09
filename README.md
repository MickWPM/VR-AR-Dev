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

Code:
[Task 1 folder in task branch](https://github.com/MickWPM/VR-AR-Dev/tree/d3f94b9bad047e649ef09241b0ecaedd9228e165/Task-1)

Video:
TBC
