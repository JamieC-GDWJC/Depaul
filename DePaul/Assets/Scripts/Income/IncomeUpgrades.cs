
using System.Collections.Generic;

[System.Serializable]
public class IncomeUpgrades
{
    public List<IncomeUpgradeStats> upgradesInOrder;
}
[System.Serializable]
public class IncomeUpgradeStats
{
    public int cost;
    public float waitTime;
    public int collectionAmount;
}

//services

[System.Serializable]
public class ServiceUpgrades
{
    public List<ServiceUpgradeStats> upgradesInOrder;
}

[System.Serializable]
public class ServiceUpgradeStats
{
    public int cost;
    public float waitTime;
    public int peopleHelped;
    public int costToRun;
}

