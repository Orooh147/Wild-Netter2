﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemOfStamina : TotemSO
{
    public TotemOfStamina(string[] lootData, string[] totemData) : base(lootData, totemData)
    {
        
    }

    public override void DoEffect(Vector3 totemLocation, Vector3 targetLocation)
    {
        if (CheckRange(totemLocation, targetLocation, range))
        {
            PlayerStats._Instance.AddStaminaAmount((int)((this.effectAmountPrecentage * PlayerStats._Instance.GetSetMaxStamina) / 100));
        }
    }
    public override IEnumerator ActivateTotemEffect(Transform targetPos, GameObject totem)
    {
        float timeToDestroy = duration + GetCurrentTime();
        while (true)
        {
            this.DoEffect(totem.transform.position, targetPos.position);
            if (GetCurrentTime() > timeToDestroy)
            {
                break;
            }
            currentRealTime = GetCurrentTime();
            yield return new WaitForSeconds(1);
        }
    }
}