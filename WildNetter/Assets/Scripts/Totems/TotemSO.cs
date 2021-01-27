﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class TotemSO : Item
{
    // Object References:
    public Item[] itemsToBuildThis;
    // Variables:

    public TotemName totemName;
    public TotemType totemType;
    public float duration;
    public float range;
    public float currentRealTime;
    public int MinimumPlayerLevel;
    public int currentZone;
    public Animator _animator;

    public TotemSO(string[] lootData, string[] totemData) : base(lootData)
    {
        if (totemData == null)
            return;
        if (totemData[1] != "")
        {
            int intTotemType;
            if (int.TryParse(totemData[1], out intTotemType))
            {
                switch (intTotemType)
                {
                    case 1:
                        totemType = TotemType.baiting;
                        break;
                    case 2:
                        totemType = TotemType.buff;
                        break;
                    case 3:
                        totemType = TotemType.debuff;
                        break;
                    case 4:
                        totemType = TotemType.detection;
                        break;
                    case 5:
                        totemType = TotemType.trapping;
                        break;
                    default:
                        break;
                }
            }
        }
        if (totemData[2] != "")
        {
            base.Name = totemData[2].ToString();
        }
        if (totemData[3] != "")
        {
            string[] durationString = totemData[3].Split(new char[] { '-' });
            int durationType = 0;
            int.TryParse(durationString[0], out durationType);
            float.TryParse(durationString[1], out duration);   
            if(durationType == 2)
            {
                int.TryParse(durationString[1], out currentZone);
            }
        }
        if (totemData[4] != "")
        {
            float.TryParse(totemData[4], out range);
        }
        if (totemData[5] != "")
        {
            int.TryParse(totemData[5], out MinimumPlayerLevel);
        }
    }

    public virtual void DoEffect(Vector3 totemLocation, Vector3 targetLocation) { }
    public virtual void DoEffect(Vector3 totemLocation) { }
    public virtual void DoEffect(Vector3 totemLocation, GameObject totem) { }
    public bool CheckRange(Vector3 totemLocation, Vector3 targetLocation, float _range)
    {
        bool isRange = (Vector3.Distance(totemLocation, targetLocation) < _range);
        return isRange;
    }

    public float GetCurrentTime()
    {
        float time = ((float)System.DateTime.Now.Hour * 3600) + ((float)System.DateTime.Now.Minute * 60) + (float)System.DateTime.Now.Second;
        return time;
    }

   public virtual IEnumerator ActivateTotemEffect(GameObject totem) { yield return null; }

    public virtual IEnumerator ActivateTotemEffect(Transform targetPos, GameObject totem) { yield return null; }

    public virtual IEnumerator ActivateTotemEffect(bool toContinuteSpawning, GameObject totem) { yield return null; }



    public virtual void PlayAnimation(string animName)
    {
        _animator.Play(animName);
    }
}