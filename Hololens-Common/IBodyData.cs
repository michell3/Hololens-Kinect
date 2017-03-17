using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBodyData {
    Dictionary<ulong, Vector3[]> GetData();
}
