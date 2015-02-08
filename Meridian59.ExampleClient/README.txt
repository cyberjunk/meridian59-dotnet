HowTo:
------
1) Copy your crush32.dll from your meridian installation to this application folder.

2) Copy your rsc0000.rsb from your meridian installation to the resource subfolder.
The existing is from the official 101/102 client.

3) Optional: Copy roomtexture bgfs and object bgfs to their subfolders in resource subfolder.
(Will show you a minimap and some nice images in debug window)

3) Possibly adjust the "DownloadVersion" value from 183 to your value from Meridian.ini,
in case you get an error when connecting.
File: Meridian59.ExampleClient.exe.config

4) Possibly adjust the overwritten major/minor buildversion properties to your client buildversion,
in case you get an error when connecting.
File: ExampleClient.cs

-----------------------
Send questions & comments to:
clint@meridian59-project.com