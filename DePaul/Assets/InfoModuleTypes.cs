using UnityEngine;

public class InfoModuleTypes : MonoBehaviour
{
    public InfoModuleType[] InfoModules;
}

[System.Serializable]
public class InfoModuleType
{
    public Sprite Icon;
    public string Type;
}
