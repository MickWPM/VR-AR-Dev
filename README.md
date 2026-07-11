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
