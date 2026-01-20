using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkill
{
    public uint SkillID { get; }
    public string SkillName { get; }
    public string SkillDescription { get; }
    public GameObject SkillPrefab { get; }
    public bool IsUseAutomatic { get; }
    public DurationType DurationType { get; }
    public float Duration { get; }

    public CoolTimeRecoveryType CoolTimeRecovery { get; }
    public float CoolTime { get; }
    public int ConsumptionCost { get; }
    public int[,] SkillRange { get; }
}
