using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
[System.Serializable]
public class NarPoint
{
    public List<ExternalCommunication> activeObjects;
    public string Title;
    public List<string> Lines;
}
 
[System.Serializable]
public class NarList
{
    public List<NarPoint> Stage;
}