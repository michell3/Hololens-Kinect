# Hololens-Kinect
Unity scripts to send Kinect data to the Hololens. The kinect joints are displayed as spheres in front of the user.

## Sources
These scripts are derived from the [Hololens Sharing](https://github.com/Microsoft/HoloToolkit-Unity/tree/master/Assets/HoloToolkit/Sharing/Tests) example in the [HoloToolkit](https://github.com/Microsoft/HoloToolkit-Unity) and the KinectView example from the [Kinect Tool Unity Pro Packages](https://developer.microsoft.com/en-us/windows/kinect/tools).

## Requirements
In order to use these scripts, you must have:
- **Windows 10**
- **Kinect 2**
- [**Hololens Unity**](http://unity3d.com/pages/windows/hololens)
- [**Visual Studio 2015 Update 2**](https://developer.microsoft.com/en-us/windows/downloads)
- The [**Kinect SDK 2.0**](https://developer.microsoft.com/en-us/windows/kinect/tools)

In addition, you will need to download:
- [**HoloToolkit**](https://github.com/Microsoft/HoloToolkit-Unity) - Make sure the Assets, External, and ProjectSettings folder are in your Unity Hololens-Sender and Hololens-Receiver Unity project.
- [**Kinect Unity Pro Packages**](https://developer.microsoft.com/en-us/windows/kinect/tools) from Kinect Tools - Make sure the Kinect.2.0.XXXXXXXXXX Unity package is imported in your Hololens-Sender Unity Project - Read [this](http://www.imaginativeuniversal.com/blog/post/2015/03/27/unity-5-and-kinect-2-integration.aspx) post for a good tutorial on Unity 5 and Kinect 2 integration)

Here are some useful developer links to get started with the Hololens:
- [Hololens Academy](https://developer.microsoft.com/en-us/windows/holographic/academy)
- [Hololens Installation Checklist](https://developer.microsoft.com/en-us/windows/holographic/install_the_tools)
- [Windows Holographic Developer Forum](https://forums.hololens.com/)
- [Using the Windows Device Portal](https://developer.microsoft.com/en-us/windows/holographic/using_the_windows_device_portal)

## Usage
Before running/deploying either app, start the **Sharing Session Manager** from the HoloToolkit menu in Unity. Take note of the IP address for the Sharing objects in each app.

### Hololens-Sender
The Hololens-Sender Unity app is a program that reads in Kinect skeletal data and broadcasts it to other devices through the Sharing Session Manager. It can be run from the Unity editor and displays the resulting data.

### Setup
Make sure you include the Kinect Unity Package and HoloToolkit in your new Unity project. See above instructions for setting those up.
The scene should have 5 things in the hierarchy:

1. Directional Light
2. Sharing (Empty GameObject with 4 scripts attached)
   * **SharingStage.cs** - From HoloToolkit Sharing. You will need to change the Server Address here
   * **SharingSessionTracker.cs** - From HoloToolkit Sharing. 
   * **AutoJoinSession.cs** - From HoloToolkit Sharing.
   * **CustomMessages2.cs** - Modified from HoloToolkit Sharing. Allows for customized messages to be broadcasted. In this case, body tracking IDs and joint data.
3. BodyManager (Empty GameObject with 3 scripts attached)
   * **KinectBodyData.cs** - From Kinect View example. Reads in Kinect skeletal data.
   * **BodyDataConverter.cs** - Parses the Kinect body data. Takes the scene's BodyManager as a parameter.
   * **BodyDataSender.cs** - Broadcasts the converted body data using custom messages. Takes the scene's BodyManager as a parameter.
4. BodyView (Empty GameObject with 1 script and 1 child)
   * **BodyView.cs** - Displays the converted body data. Takes the scene's BodyManager as a parameter.
   * Cube - You can disable the Mesh Renderer
5. MainCamera (HoloToolkit camera prefab)

When you run the app you should be able to see your machine join the Sharing Session in the console.

### Hololens-Receiver
The Hololens-Receiver Unity app is a program that can be deployed on the Hololens that listens for messages about Kinect skeletal data and renders it to screen.

### Setup
Make sure you include HoloToolkit in your new Unity project. Also make sure that Virtual Reality is enabled in your player settings, and that **internetClient**, **internetClientServer**, and **privateNetworkClientServer** are enabled in the checklist.
The scene should have 5 things in the hierarchy:

1. Directional Light
2. Sharing (Empty GameObject with 4 scripts attached, same as above)
3. BodyReceiver (Empty GameObject with 1 script attached)
   * **BodyDataReceiver.cs** - Listens for custom messages containing skeletal data
4. BodyView (Empty GameObject with 1 script and 1 child)
   * **BodyView.cs** - Displays the body data. Takes the scene's BodyReceiver as a parameter.
   * Cube - You can disable the Mesh Renderer
5. MainCamera (HoloToolkit camera prefab)

Build the Hololens app and deploy. If you don't know how to do this be sure to read over the developer links.
When you deploy the app you should be able to see your Hololens device join the Sharing Session in the console.

## Needed Improvements
- Properly scale the Kinect skeletons
- Delete skeletons that leave the screen
- Render lines for bones like in the Kinect View example
- Reduce noise in the movements
- Track the location of the Kinect so that bodies can be placed in their real location

## Known Errors
- Sometimes the Hololens-Receiver app crashes in the Unity Editor upon receiving skeletal data

## Credits
This was made for Hololens development at the Studio for Creative Inquiry. Feel free to use these scripts for your own Hololens + Kinect projects.
