using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TurretData
{
    public GameObject turretPrefab;
    public int cost; //花费
    public GameObject turretUpgradedPrefab;
    public int costUpgraded; //升级花费
    public TurretType type;
}

public enum TurretType //炮台类型
{
    LaserTurret,
    MissileTurret,
    StandardTurret
}