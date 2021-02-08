﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemOfShock : TotemSO
{
   
    private List<GameObject> enemyShocked = new List<GameObject>();
    public TotemOfShock(string[] lootData, string[] totemData) : base(lootData, totemData)
    {

    }

    public override void DoEffect(Vector3 totemLocation, GameObject totem)
    {
        Collider[] objectCollider;
        objectCollider = Physics.OverlapSphere(totemLocation, range, TotemManager._Instance.enemiesLayer);
        Debug.Log("enemyShocked DO effect");
        foreach (Collider col in objectCollider)
        {
            if (CheckRange(totemLocation, col.transform.position, range))
            {
                //apply slow to enemy - from totemSO
                if (enemyShocked.Contains(col.transform.root.gameObject))
                {
                     col.transform.root.GetComponent<Enemy>().SlowSetter(this.effectAmountPrecentage.GetValueOrDefault(), true);
                    continue;
                }
                else
                {

                    enemyShocked.Add(col.transform.root.gameObject);
                    col.transform.root.GetComponent<Enemy>().TotemEffect(TotemName.shock, totem);
                    col.transform.root.GetComponent<Enemy>().FlatDamage(this.GetTotemEffectAmount);
                   
                }
            }
            else
            {
                //this.effectAmountPrecentage.GetValueOrDefault()
                col.transform.root.GetComponent<Enemy>().SlowSetter(this.effectAmountPrecentage.GetValueOrDefault(), false);
                //remove slow
            }
        }
    }
    public override IEnumerator ActivateTotemEffect(GameObject totem)
    {
        float timeToDestroy = duration + GetCurrentTime();
        while (true)
        {
            this.DoEffect(totem.transform.position, totem);
            if (GetCurrentTime() > timeToDestroy)
            {
                foreach (GameObject go in enemyShocked)
                {
                    go.GetComponent<Enemy>().CancelEffect();
                }
                break;
            }
            currentRealTime = GetCurrentTime();
            yield return new WaitForSeconds(1);
        }
    }
}
