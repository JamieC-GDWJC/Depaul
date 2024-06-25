using System.Collections.Generic;
using UnityEngine;

public class ProgressionManager : MonoBehaviour
{
    [Header("Each Stage is a list of\nservices / stands that need to be\nbought in order to progress")]
    public PointList listOfStages = new PointList();

    public int stage;
    public bool done = false;
    private CameraManager _cameraM;

    public List<ExternalCommunication> externalCommunications;
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
        
        if (stage >= listOfStages.Stage.Count)
        {
            done = true;
            return;
        }
        _cameraM.SwitchStage(listOfStages.Stage[stage].CameraPositionAtStage, listOfStages.Stage[stage].CameraRotationAtStage);
        externalCommunications = listOfStages.Stage[stage].AssetsToComplete;
    }
}
