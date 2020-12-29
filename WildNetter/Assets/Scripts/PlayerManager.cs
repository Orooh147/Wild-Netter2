﻿
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    // Script References
    static PlayerManager _instance;
    PlayerMovement _playerMovement;
    //PlayerGFX _playerGFX;
    PlayerInventory _playerInventory;
    PlayerCombat _playerCombat;
    PlayerStats _playerStats;


    //EventsAndAction

    //Getter And Setter
    public PlayerCombat GetPlayerCombat { get { return _playerCombat; } }
    public PlayerInventory GetPlayerInventoryScript { get { return _playerInventory; } }

    public PlayerStats GetPlayerStatsScript { get { return _playerStats; } }

    public Transform GetPlayerTransform { get { return transform; } }

    //Functions:

    public static PlayerManager GetInstance() {
        if (_instance == null)
        {
            _instance = new PlayerManager();

        }
        return _instance;
    }
    private void Awake()
    {
        _instance = this;
    }

    public void Init(WeaponSO playersWeapon)
    {
        AssignScriptsComponentsReferences();
        _playerMovement.Init();
        GetPlayerCombat.Init(playersWeapon);
    }

    public void Respawn() { }

    public void PlayerDead() { }

    private void AssignScriptsComponentsReferences()
    {
        _playerInventory =  PlayerInventory.GetInstance ;
      _playerMovement = GetComponent<PlayerMovement>();
        _playerCombat = GetComponent<PlayerCombat>();
        // _playerGFX = GetComponentInChildren<PlayerGFX>();
        _playerStats = GetComponent<PlayerStats>();
      
    }


}
