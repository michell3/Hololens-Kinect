/*
 * BodyView.cs
 *
 * Displays spheres for Kinect body joints
 * Requires the BodyDataConverter script or the BodyDataReceiver script
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;

public class BodyView : MonoBehaviour {

    public GameObject BodySourceManager;

    // Dictionary relating tracking IDs to displayed GameObjects
    private Dictionary<ulong, GameObject> _Bodies = new Dictionary<ulong, GameObject>();
    private BodyDataConverter _BodyDataConverter;
    private BodyDataReceiver _BodyDataReceiver;

    void Update() {

        if (BodySourceManager == null) {
            return;
        }

        // Dictionary of tracked bodies from the Kinect or from data
        // sent over the server
        Dictionary<ulong, Vector3[]> bodies;

        // Is the body data coming from the BodyDataConverter script?
        _BodyDataConverter = BodySourceManager.GetComponent<BodyDataConverter>();
        if (_BodyDataConverter == null) {
            // Is the body data coming from the BodyDataReceriver script?
            _BodyDataReceiver = BodySourceManager.GetComponent<BodyDataReceiver>();
            if (_BodyDataReceiver == null) {
                return;
            } else {
                bodies = _BodyDataReceiver.GetData();
            }
        } else {
            bodies = _BodyDataConverter.GetData();
        }

        if (bodies == null) {
            return;
        }

        // Delete untracked bodies
        List<ulong> trackedIDs = new List<ulong>(bodies.Keys);
        List<ulong> knownIDs = new List<ulong>(_Bodies.Keys);
        foreach (ulong trackingID in knownIDs) {

            if (!trackedIDs.Contains(trackingID)) {
                Destroy(_Bodies[trackingID]);
                _Bodies.Remove(trackingID);
            }
        }

        // Add and update tracked bodies
        foreach (ulong trackingID in bodies.Keys) {

            // Add tracked bodies if they are not already being displayed
            if (!_Bodies.ContainsKey(trackingID)) {
                _Bodies[trackingID] = CreateBodyObject(trackingID);
            }

            // Update the positions of each body's joints
            RefreshBodyObject(bodies[trackingID], _Bodies[trackingID]);
        }
    }

    // Create a GameObject given a tracking ID
    private GameObject CreateBodyObject(ulong id) {

        GameObject body = new GameObject("Body:" + id);

        for (int i = 0; i < 25; i++) {
            GameObject jointObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            jointObj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            jointObj.name = i.ToString();
            jointObj.transform.parent = body.transform;
        }

        return body;
    }

    // Update the joint GameObjects of a given body
    private void RefreshBodyObject(Vector3[] jointPositions, GameObject bodyObj) {

        for (int i = 0; i < 25; i++) {
            Vector3 jointPos = jointPositions[i];

            Transform jointObj = bodyObj.transform.FindChild(i.ToString());
            jointObj.localPosition = jointPos;
        }
    }
}
