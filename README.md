# Hololens-Kinect
#### Unity scripts to send Kinect data to the Hololens

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
###Hololens-Sender
The Hololens-Sender Unity app is a program that reads in Kinect skeletal data and broadcasts it to other devices through the Sharing Session Manager. It can be run from the Unity editor and displays the resulting data.

####Setup
Make sure you include the Kinect Unity Package and HoloToolkit in your new Unity project. See above instructions for setting those up
The scene should have 5 things in the hierarchy:

1. Directional Light
2. Sharing (Empty GameObject with 4 scripts attached)
   * **SharingStage.cs** - From HoloToolkit Sharing. You will need to change the Server Address here
   * **SharingSessionTracker.cs** - From HoloToolkit Sharing. 
   * **AutoJoinSession.cs** - From HoloToolkit Sharing.
   * **CustomMessages2.cs** - Modified from HoloToolkit Sharing. Allows for customized messages to be broadcasted. In this case, body tracking IDs and joint data.
3. BodyManager (Empty GameObject with 3 scripts attached)
   * **BodyDataSender.cs**
   * **KinectBodyData.cs**
   * **BodyDataConverter.cs**
4. BodyView (Empty GameObject with 1 script and 1 child)
   * **BodyView.cs**
   * Cube - You can disable the Mesh Renderer
5. MainCamera (HoloToolkit camera prefab)

###Hololens-Receiver
The Hololens-Receiver Unity app is a program that can be deployed on the Hololens that listens for messages about Kinect skeletal data and renders it to screen.
