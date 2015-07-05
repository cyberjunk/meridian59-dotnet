1) crush32.dll:
---------------
To support older version 9 compressed BGF, you need to copy a crush32.dll file from an old M59 client to this application folder.

2) Frames:
---------------
This is a "pool" of images available in the BGF.
Add your images here, make sure they are BMP format and use the default Meridian 59 colortable.
Otherwise they will either give you a not supported warning or mystically change their color.

3) HotSpot:
---------------
A HotSpot is a 2D point on an image/frame, used as attach-anchor.
Like an arm is attached to a corpse or a nose to an head.
You need to define these for each frame and place them correctly, if you want to use them.
They are identified by an "index", which can be negative or positive.
Negative means, the attached image will be an underlay, so the parts which are covered by the parent,
are not visible. If you set this to same positive value it's an overlay, your attached image will cover the parent.
The hierarchy of an object (if there are attachements or not and which are attached at which hotspot) is server transmitted.

4) FrameSet / Group
----------------
A Group (or FrameSet) is a set of frames linked together from the pool mentioned in (2).
A Group is "a single moment", but in different viewer's angles on an object.
If you reference 8 frames in your framset, then each frame represents an anglewidth of 45°.

5) Animation Test
-----------------
Use this to test an animation of this object. The basic three types are:
(a) None (only a fixed group shown all the time)
(b) Cycle (cycles from a low group to an high number, then restarts)
(c) Once (cyclces from a low group to an high number, then shows a final group)
You can "rotate" the object by using the angle slider or change the color palette (=coloring). 

6) Export / XML format
------------------
Exports the frames of the BGF to separated BMP files and creates an additional metadata XML file.
