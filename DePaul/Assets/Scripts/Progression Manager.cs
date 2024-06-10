using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ProgressionManager : MonoBehaviour
{
    [Header("Each Stage is a list of\nservices / stands that need to be\nbought in order to progress")]
    public PointList listOfStages = new PointList();

    public int stage;
    public bool done = false;
    private CameraManager _cameraM;

    public ExternalCommunication[] externalCommunications;
    // Start is called before the first frame update
    void Start()
    {
        _cameraM = FindObjectOfType<CameraManager>();
        stage = -1;

        ChangeStage();
    }

    // Update is called once per frame
    void Update()
    {
        if (stage >= listOfStages.Stage.Count || done)
            return;
        
        foreach (ExternalCommunication EC in externalCommunications)
        {
            if(!EC.active)
                return;
        }
        ChangeStage();
        
    }

    void ChangeStage()
    {
        stage ++;
        _cameraM.SwitchStage(stage);
        if (stage >= listOfStages.Stage.Count)
        {
            done = true;
            return;
        }
        externalCommunications = listOfStages.Stage[stage].activeObjects;
    }
}
