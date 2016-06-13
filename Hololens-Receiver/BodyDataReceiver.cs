/*
 * BodyDataReceiver.cs
 *
 * Receives body data from the network
 * Requires CustomMessages2.cs
 */

using HoloToolkit.Sharing;
using HoloToolkit.Unity;
using System.Collections.Generic;
using UnityEngine;

// Receives the body data messages
public class BodyDataReceiver : Singleton<BodyDataReceiver> {

    private Dictionary<long, Vector3[]> _Bodies = new Dictionary<long, Vector3[]>();

    public Dictionary<long, Vector3[]> GetData() {
        return _Bodies;
    }

    void Start() {
        CustomMessages2.Instance.MessageHandlers[CustomMessages2.TestMessageID.BodyData] =
            this.UpdateBodyData;
    }

    // Called when reading in Kinect body data
    void UpdateBodyData(NetworkInMessage msg) {
        // Parse the message
        long trackingID = msg.ReadInt64();
        Vector3 jointPos;
        Vector3[] jointPositions = new Vector3[25];

        for (int i = 0; i < 25; i++) {
            jointPos = CustomMessages2.Instance.ReadVector3(msg);
            jointPositions[i] = jointPos;
        }

        _Bodies[trackingID] = jointPositions;
    }
}