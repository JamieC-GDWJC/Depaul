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

[System.Serializable]
public class StoryScenario
{
    public string title;
    public List<string> story;

    public ExternalCommunication o1;
    public string o1Reason;
    public ExternalCommunication o2;
    public string o2Reason;
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

[System.Serializable]
public class PopupScenario
{
    public string title;
    public List<string> story;

    public ExternalCommunication affectedAsset;

    public int cost;
    public float time;

    public int peopleHelped;
    public int donationesEarned;

    public bool complete = false;
    public bool isBought = false;
}