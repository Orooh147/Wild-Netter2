﻿using UnityEngine.EventSystems;
using System;
using System.Collections;
using UnityEngine;

public enum AttackType {Melee , Ranged, Totem };
public class PlayerCombat : MonoSingleton<PlayerCombat>
{
    // Script References:
    WeaponSO _equippedWeaponSO;
    PlayerMovement _playerMovement;
    PlayerStats _playerStats;
    // Component References:
    
   [SerializeField] Collider _weaponCollider;
    [SerializeField]GameObject _weaponGO;
   
   
    // Variables:
    bool canAttack;
     int  attackDMG;
    public string currentWeaponName;
    event Action AttackAction;

    TotemName currentTotemHolder;
    // Getter & Setters:
    public WeaponSO GetSetWeaponSO {
        get { return _equippedWeaponSO; }
        set { 
            
            _equippedWeaponSO = value;
            currentWeaponName = _equippedWeaponSO.GetData.Name;
        }

    }
    public void SetCurrentTotemHolderByInt(int index) {
        TotemName ttmcache = TotemName.None;
        if (index < 0 || index > 5)
            return;

        switch (index)
        {
           
            case 1:
                ttmcache = TotemName.prey;
                break;
            case 0:
                ttmcache = TotemName.healing;
                break;
            case 2:
                ttmcache = TotemName.detection;
                break;
            case 3:
                ttmcache = TotemName.stamina;
                break;
            case 4:
                ttmcache = TotemName.shock;
                break; 

            //default:
            //case 0:
            //    ttmcache = TotemName.None;
            //    break;
        }

        Debug.Log("Now Holding Totem : " + ttmcache);
        GetSetCurrentTotemToDeploy = ttmcache;
    }
    
    public TotemName GetSetCurrentTotemToDeploy {
        get => currentTotemHolder;

        set {

            if (currentTotemHolder == value)
                return;


            currentTotemHolder = value;
        
        }
    }
    public int GetAttackDMG { 
        get {
            System.Random rnd = new System.Random();
            
            attackDMG = rnd.Next(GetSetWeaponSO.minDMG, GetSetWeaponSO.maxDMG);

            return attackDMG;
        } 
   
    }

    public override void Init() //WeaponSO startingWeapon
    {
        ToggleWeaponCollider(false);
        _playerStats = PlayerStats._Instance;
        _playerMovement = PlayerMovement._Instance;
        currentTotemHolder = TotemName.None;
        canAttack = true;
        GetSetWeaponSO = ItemFactory._Instance.GenerateItem(20000) as WeaponSO; 
        ResetAttackAction();
        AttackAction += MeleeAttack;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            GetSetCurrentTotemToDeploy = (TotemName.detection);
        
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            GetSetCurrentTotemToDeploy = (TotemName.healing);
           
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            GetSetCurrentTotemToDeploy = TotemName.prey;
      
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
GetSetCurrentTotemToDeploy = TotemName.shock;
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            GetSetCurrentTotemToDeploy = TotemName.stamina;

        }
    }

    //move to player manager
    public void GetHit(int RecieveDMG, Vector2 Source)
    {
        PlayerGFX._Instance.ApplyPlayerVFX(((Vector2)transform.position + Source)/2f, VFXWorldType.PlayerGotHit);
        _playerStats.ApplyDMGToPlayer(RecieveDMG);
        _playerMovement.GetPlayerRB.AddForce(new Vector2(Source.x, Source.y));
    }
    public void Attack() {  
        if (canAttack && !EventSystem.current.IsPointerOverGameObject())
        {
            AttackAction?.Invoke();
        }
    }
     void MeleeAttack() {
        Debug.Log("SWord AttacK");
        if (isNotInCooldown) {   
            PlayerGFX._Instance.SetAnimationTrigger("Attack");
            StartCoroutine(MeleeAttackCoroutine());
        }
    // apply GFX Anim, sound
    }   
     void RangeAttack() {
        if (isNotInCooldown)
        {
            Debug.Log("Range AttacK");
            StartCoroutine(FreezeMovement(1f));
        }

    }
    void DeployTotem()
    {
        if (isNotInCooldown)
        {
            //if (TotemManager._Instance.TryDeployAtLocation((transform.position + _playerMovement.GetAngleDirection() * 2f), GetSetCurrentTotemToDeploy))
            //{
            //    InputManager._Instance.FreezeCoroutineForShotPeriodOfTime(1f);
            //    //StartCoroutine(FreezeMovement(1f));
            //    PlayerGFX._Instance.SetAnimationTrigger("PlaceTotem");
            //} 


        }
    }
   
    public void SetAttackType(AttackType type) {
    
     ResetAttackAction();

        switch (type)
        {
            case AttackType.Melee:
                UiManager._Instance.UpdateTotemsFromGamePhase(SceneHandler._Instance.GetSetPlayPhase);
                AttackAction += MeleeAttack; Debug.Log("MeleeAttack");
                break;
            case AttackType.Ranged:
                AttackAction += RangeAttack;
                Debug.Log("Range");
                break;
            case AttackType.Totem:
                AttackAction += DeployTotem;
                Debug.Log("Deploy Totem");
                break;
            default:
                break;
        }
       InputManager._Instance.currectAttackType = type;

    }
    private void ResetAttackAction()
    {
        AttackAction -= RangeAttack;
        AttackAction -= DeployTotem;
        AttackAction -= MeleeAttack;
    }
//  private Vector3 LockOnEnemy() { } <- will be used later on
    private void ToggleWeaponCollider(bool state) => _weaponCollider.enabled = state;

    // ienumerators:
    IEnumerator  MeleeAttackCoroutine() {
       
        canAttack = false;
        ToggleWeaponCollider(true);
        InputManager._Instance.SetFreelyMoveAndRotate(false);
        InputManager._Instance.FreezeRB(true);
           yield return new WaitForSeconds(.3f);
       
        _playerMovement.GetSetCanDash = false;
        yield return new WaitForSeconds(1f);
        _playerMovement.GetSetCanDash = true;

        InputManager._Instance.SetFreelyMoveAndRotate(true) ;
            InputManager._Instance.FreezeRB(false);
        

        ToggleWeaponCollider(false);
        canAttack = true;
    

    }

    bool isNotInCooldown = true;
    IEnumerator FreezeMovement(float duration) {
        isNotInCooldown = false;

        InputManager._Instance.FreezeRB(true);
            InputManager._Instance.SetFreelyMoveAndRotate(false);
     

        yield return new WaitForSeconds(duration);
        InputManager._Instance.FreezeRB(false);
        InputManager._Instance.SetFreelyMoveAndRotate(true);
        isNotInCooldown = true;
    }
    private void OnDestroy()
    {
        ResetAttackAction();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (_weaponCollider.enabled)
            CalculateDMGToEnemy(other.gameObject.GetComponent<EnemyPart>());
        
    }
   public void CalculateDMGToEnemy(EnemyPart enemy) {

        if (enemy == null)
            return;

        int finalDmg = GetAttackDMG;
        int StrengthAgainstArmour = _playerStats.GetSetStrength - enemy.armor;

        if (StrengthAgainstArmour < 0)
            StrengthAgainstArmour = 0;

        // attack dmg of the weapon + attack dmg of the weapon * (playerStength% - enemy armour%)
        finalDmg += Convert.ToInt32(  finalDmg * (StrengthAgainstArmour) * .1f);

         TextPopUp.Create(TextType.NormalDMG, ( transform.position + enemy.transform.root.position)/2f, finalDmg);
         PlayerGFX._Instance.ApplyPlayerVFX((enemy.transform.parent.position + transform.position) / 2f, VFXWorldType.EnemyGotHit);
        enemy.GetDamage(finalDmg, transform.position, GetSetWeaponSO.vulnerabilityActivator);
    }


  
}
