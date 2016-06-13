/*
 * BodyDataConverter.cs
 *
 * Converts Kinect body data into a format that can easily be
 * sent and read over the network
 * Requires KinectBodyData.cs
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;

public class BodyDataConverter : MonoBehaviour {

    public GameObject KinectBodyData;
    
    // Dictionary that maps body trackingIDs to an array of joint locations
    private Dictionary<ulong, Vector3[]> _Bodies =
        new Dictionary<ulong, Vector3[]>();
    private KinectBodyData _BodyManager;

    // Public function so other scripts can send out the data
    public Dictionary<ulong, Vector3[]> GetData() {
        return _Bodies;
    }
    
    void Update() {

        if (KinectBodyData == null) {
            return;
        }
        
        _BodyManager = KinectBodyData.GetComponent<KinectBodyData>();
        if (_BodyManager == null) {
            return;
        }
        
        Kinect.Body[] data = _BodyManager.GetData();
        if (data == null) {
            return;
        }

        // Get tracked IDs
        List<ulong> trackedIds = new List<ulong>();
        foreach (var body in data) {

            if (body == null) {
                continue;
            }
                
            if (body.IsTracked) {
                trackedIds.Add(body.TrackingId);
            }
        }
        
        List<ulong> knownIds = new List<ulong>(_Bodies.Keys);
        
        // Delete untracked bodies
        foreach (ulong trackingId in knownIds){

            if (!trackedIds.Contains(trackingId)) {
                _Bodies.Remove(trackingId);
            }
        }

        // If a body is tracked, update the dictionary
        foreach (var body in data) {

            if (body == null) {
                continue;
            }
            
            if (body.IsTracked) {

                if (!_Bodies.ContainsKey(body.TrackingId)) {
                    _Bodies[body.TrackingId] = CreateBodyData();
                }
                
                UpdateBodyData(body, _Bodies[body.TrackingId]);
            }
        }
    }
    
    private Vector3[] CreateBodyData() {
        Vector3[] body = new Vector3[25];
        return body;
    }
    
    private void UpdateBodyData(Kinect.Body body, Vector3[] bodyData) {

        for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++) {
            Kinect.Joint sourceJoint = body.Joints[jt];
            bodyData[(int)jt] = GetVector3FromJoint(sourceJoint);
        }
    }

    private static Vector3 GetVector3FromJoint(Kinect.Joint joint) {
        return new Vector3(joint.Position.X * 10, joint.Position.Y * 10, joint.Position.Z * 10);
    }
}
