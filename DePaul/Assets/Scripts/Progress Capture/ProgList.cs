using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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

[System.Serializable]
public class Scenario
{
    public string title;
    public List<string> story;

    public ExternalCommunication optionOne;
    public string optionOneReason;
    public ExternalCommunication optionTwo;
    public string optionTwoReason;
    public ExternalCommunication correctOption;
    public string correctOptionReason;

    public int peopleHelped;
}

[System.Serializable]
public class NarPoint
{
    public string Title;
    public List<string> Lines;
    public List<ExternalCommunication> activeObjects;
}