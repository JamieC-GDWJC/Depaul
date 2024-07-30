using UnityEngine;

public class InfoModuleTypes : MonoBehaviour
{
    public InfoModuleType[] InfoModules;

    public GameObject UIBubble;
    public GameObject InfoModule;
    public GameObject PopupIcon;

}

[System.Serializable]
public class InfoModuleType
{
    public Sprite Icon;
    public string Type;
}
