/*
 * BodyDataSender.cs
 *
 * Broadcasts body data over the network
 * Requires CustomMessages2.cs
 */

using HoloToolkit.Sharing;
using HoloToolkit.Unity;
using System.Collections.Generic;
using UnityEngine;

public class BodyDataSender : Singleton<BodyDataSender> {

    public GameObject BodyDataConverter;

    private BodyDataConverter _BodyDataConverter;

    void Update() {

        // Get the parsed Kinect body data
        if (BodyDataConverter == null) {
            return;
        }

        _BodyDataConverter = BodyDataConverter.GetComponent<BodyDataConverter>();
        if (_BodyDataConverter == null) {
            return;
        }

        Dictionary<ulong, Vector3[]> bodyData = _BodyDataConverter.GetData();
        if (bodyData == null) {
            return;
        }

        // Send over the bodyData one tracked body at a time
        List<ulong> trackingIDs = new List<ulong>(bodyData.Keys);
        foreach (ulong trackingID in trackingIDs) {
            CustomMessages2.Instance.SendBodyData(trackingID, bodyData[trackingID]);
        }
    }
}