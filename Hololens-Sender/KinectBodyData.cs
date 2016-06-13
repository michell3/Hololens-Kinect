/*
 * KinectBodyData.cs
 *
 * Retrieves Kinect skeletal data
 */

using UnityEngine;
using System.Collections;
using Windows.Kinect;

public class KinectBodyData : MonoBehaviour {

    private KinectSensor _Sensor;
    private BodyFrameReader _Reader;
    private Body[] _Bodies = null;
    
    // Public function so other scripts can grab the Kinect Data
    public Body[] GetData() {
        return _Bodies;
    }
    
    // Open connections to the Kinect
    void Start () {
        _Sensor = KinectSensor.GetDefault();

        if (_Sensor != null) {
            _Reader = _Sensor.BodyFrameSource.OpenReader();
            
            if (!_Sensor.IsOpen) {
                _Sensor.Open();
            }
        }   
    }
    
    // Update Kinect body data on every frame
    void Update () {

        if (_Reader != null) {
            var frame = _Reader.AcquireLatestFrame();

            if (frame != null) {

                if (_Bodies == null) {
                    _Bodies = new Body[_Sensor.BodyFrameSource.BodyCount];
                }
                
                frame.GetAndRefreshBodyData(_Bodies);
                
                frame.Dispose();
                frame = null;
            }
        }    
    }
    
    // Close connections to the Kinect
    void OnApplicationQuit() {

        if (_Reader != null) {
            _Reader.Dispose();
            _Reader = null;
        }
        
        if (_Sensor != null) {

            if (_Sensor.IsOpen) {
                _Sensor.Close();
            }
            
            _Sensor = null;
        }
    }
}
