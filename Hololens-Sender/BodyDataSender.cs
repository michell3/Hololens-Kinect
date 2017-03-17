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
    private List<ulong> _knownIDs;

    protected override void Awake()
    {
        _knownIDs = new List<ulong>();
    }
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

        // remove ids
        var keys = new List<ulong>(bodyData.Keys);
        var remove_ids = new List<ulong>();
        foreach(var id in _knownIDs)
        if(!keys.Contains(id))
        {
                CustomMessages2.Instance.SendBodyData(id, false, null);
                remove_ids.Add(id);
        }
        foreach(var id in remove_ids)
        {
            _knownIDs.Remove(id);
        }

        // Send over the bodyData one tracked body at a time
        List<ulong> trackingIDs = new List<ulong>(bodyData.Keys);
        foreach (ulong trackingID in trackingIDs) {
            CustomMessages2.Instance.SendBodyData(trackingID, true, bodyData[trackingID]);
            
            // add if new
            if (!_knownIDs.Contains(trackingID))
                _knownIDs.Add(trackingID);
        }
    }
}