using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
[System.Serializable]
public class NarPoint
{
    public ExternalCommunication[] activeObjects;
    public int Lines;
}
 
[System.Serializable]
public class NarList
{
    public List<NarPoint> Stage;
}