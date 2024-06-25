using System.Collections.Generic;
using UnityEngine;
 
[System.Serializable]
public class Point
{
    public Vector3 CameraPositionAtStage;
    public Vector3 CameraRotationAtStage;
    public List<ExternalCommunication> AssetsToComplete;
}
 
[System.Serializable]
public class PointList
{
    public List<Point> Stage;
}