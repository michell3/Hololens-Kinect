/*
 * BodyView.cs
 *
 * Displays spheres for Kinect body joints
 * Requires the BodyDataConverter script or the BodyDataReceiver script
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BodyView : MonoBehaviour
{

    public GameObject BodySourceManager;
    public Material _sphereMaterial;

    // Dictionary relating tracking IDs to displayed GameObjects
    private Dictionary<ulong, GameObject> _gameBodies = new Dictionary<ulong, GameObject>();
    private IBodyData _bodyData;

    void Update()
    {

        if (BodySourceManager == null)
        {
            return;
        }

        // Dictionary of tracked bodies from the Kinect or from data
        // sent over the server
        Dictionary<ulong, Vector3[]> bodies = null;

        // Is the body data coming from the BodyDataConverter script?
        _bodyData = BodySourceManager.GetComponents(typeof(IBodyData))[0] as IBodyData;
        if (_bodyData == null)
        {
            return;
        }

        bodies = _bodyData.GetData();

        if (bodies == null)
        {
            return;
        }

        // Delete untracked bodies
        var trackedIDs = new List<ulong>(bodies.Keys);
        var knownIDs = new List<ulong>(_gameBodies.Keys);
        foreach (ulong trackingID in knownIDs)
        {

            if (!trackedIDs.Contains(trackingID))
            {
                Destroy(_gameBodies[trackingID]);
                _gameBodies.Remove(trackingID);
            }
        }

        // Add and update tracked bodies
        foreach (ulong trackingID in bodies.Keys)
        {

            // Add tracked bodies if they are not already being displayed
            if (!_gameBodies.ContainsKey(trackingID))
            {
                _gameBodies[trackingID] = CreateBodyObject(trackingID);
            }

            // Update the positions of each body's joints
            RefreshBodyObject(bodies[trackingID], _gameBodies[trackingID]);
        }
    }

    // Create a GameObject given a tracking ID
    private GameObject CreateBodyObject(ulong id)
    {

        GameObject body = new GameObject("Body:" + id);

        for (int i = 0; i < 25; i++)
        {
            GameObject jointObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            // add a material if we've set one
            if(_sphereMaterial != null)
            {
                jointObj.GetComponent<Renderer>().material = _sphereMaterial;
            }
            jointObj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            jointObj.name = i.ToString();
            jointObj.transform.parent = body.transform;
        }

        return body;
    }

    // Update the joint GameObjects of a given body
    private void RefreshBodyObject(Vector3[] jointPositions, GameObject bodyObj)
    {

        for (int i = 0; i < 25; i++)
        {
            Vector3 jointPos = jointPositions[i] + gameObject.transform.position;

            Transform jointObj = bodyObj.transform.FindChild(i.ToString());
            jointObj.localPosition = jointPos;
        }
    }
}

