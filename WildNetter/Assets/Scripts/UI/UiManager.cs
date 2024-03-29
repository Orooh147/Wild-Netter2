﻿
using TMPro;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UiManager : MonoSingleton<UiManager>
{
    // Script References:

    PlayerInventory _playerInventory;
    PlayerWallet _wallet;
    // Component References:
    // ****Add here all the panel/buttons/images component of the UI****
    [SerializeField] GameObject playerInventoryUIWindow;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject loadGameMenu;
    [SerializeField] GameObject newsMenu;
    [SerializeField] GameObject optionsMenu;
    [SerializeField] GameObject inventorySlotHolder;
    [SerializeField] GameObject playerMenu;
    [SerializeField] GameObject mapMenu;
    [SerializeField] GameObject zoneMap;
    [SerializeField] GameObject worldMap;
    [SerializeField] GameObject inGamePopUp;
    [SerializeField] GameObject pausePopUp;
    [SerializeField] GameObject pauseScreen;
    [SerializeField] GameObject inGameOptionsMenu;
    [SerializeField] GameObject inGameSoundOptions;
    [SerializeField] GameObject inGameGraphicsOptions;
    [SerializeField] GameObject inGameControlsOptions;
    [SerializeField] GameObject exitZonePopUp;
    [SerializeField] GameObject exitZoneMap;
    [SerializeField] GameObject gui;
    [SerializeField] GameObject InventroyHUD;
    [SerializeField] GameObject[] Slots;
    [SerializeField] Slider healthSlider;
    [SerializeField] Slider staminaSlider;
    //[SerializeField] Gradient gradient;
    //[SerializeField] Image fill;
    [SerializeField] TextMeshProUGUI currencyTMP;
    [SerializeField] TextMeshProUGUI inventoryCapacityTMP;
    //player
    [SerializeField] PlayerManager player;


    Sprite defaultSpriteForSlot;
    ItemData[] inventory;
    //SerializeField] TextMeshProUGUI inventoryCapacityText;


    // Variables:

    // Getter & Setters:


    public override void Init()
    {
        _wallet = PlayerWallet.GetInstance;
        _playerInventory = PlayerInventory.GetInstance;
        inventory = _playerInventory.GetInventory;
        Slots = new GameObject[_playerInventory.maxCapacityOfItemsInList];
        lastSlot = totemIcons.Length - 1;
        UpdateTotemsFromGamePhase(SceneHandler._Instance.GetSetPlayPhase);
        inGamePopUp.SetActive(false);
        //currencyTMP = playerInventoryUIWindow.transform.Find("CurrencyText").GetComponent<TextMeshProUGUI>();
        //inventoryCapacityTMP = playerInventoryUIWindow.transform.Find("CapacityText").GetComponent<TextMeshProUGUI>();
        //for (int i = 0; i < _playerInventory.maxCapacityOfItemsInList; i++)
        //{

        //    Slots[i] = inventorySlotHolder.transform.GetChild(i).gameObject;

        //}
        //defaultSpriteForSlot = inventorySlotHolder.transform.GetChild(0).GetComponent<Image>().sprite;
        //UpdateInventory();
    }
    public void ToggleMainMenu(bool state)
    {
        mainMenu.SetActive(state);
    }


    public void UpdateInventory()
    {
        if (!playerInventoryUIWindow.activeSelf)
            return;

        //Need to create PlayerWallet - > _wallet
        currencyTMP.text = string.Format("Gold : {0}    Silver : {1}    Copper : {2}", _wallet.GetSetPlayersGold, _wallet.GetSetPlayersSilver, _wallet.GetSetPlayersCopper);
        inventoryCapacityTMP.text = string.Format("{0}/{1}", inventory.Length - _playerInventory.GetAmountOfItem(null), inventory.Length);
        string text = " / " + _playerInventory.maxCapacityOfItemsInSlot;

        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null)
            {
                Slots[i].GetComponentInChildren<Text>().text = "";
                Slots[i].GetComponent<Image>().sprite = defaultSpriteForSlot;
                continue;
            }




            if (i < inventory.Length)
            {
                Slots[i].GetComponent<Image>().sprite = ItemFactory._Instance.GetItemSprite(inventory[i].GetData.ID);
                if (inventory[i].amount > 1)
                {
                    Slots[i].GetComponentInChildren<Text>().text = inventory[i].amount + text;

                }
            }
        }
    }

    public void SetMaxHealth(float health)
    {
        healthSlider.maxValue = health;
        healthSlider.value = health;

        //fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(float health)
    {
        healthSlider.value = health;

        //fill.color = gradient.Evaluate(healthSlider.normalizedValue);
    }

    public void SetMaxStamina(float stamina)
    {
        staminaSlider.maxValue = stamina;
        staminaSlider.value = stamina;
    }

    public void SetStamina(float stamina)
    {
        staminaSlider.value = stamina;
    }


    public void ToggleInventoryMenu(bool state)
    {

        //if (state)
        //{
        //    UpdateInventory();
        //}
        playerInventoryUIWindow.SetActive(state);
    }
    public void ToggleStatsMenu(bool state) { }
    public void ToggleMapUI(bool state)
    {
        mapMenu.SetActive(state);
    }
    public void ToggleZoneMap(bool state)
    {
        zoneMap.SetActive(state);
    }
    public void ToggleWorldMap(bool state)
    {
        worldMap.SetActive(state);
    }
    public void ToggleTotemModifierMSG(bool state) { }
    public void ToggleMissionMenu(bool state) { }
    public void ToggleInGamePopUp(bool state)
    {
        inGamePopUp.SetActive(state);
    }
    public void TogglePlayerMenu(bool state)
    {
        playerMenu.SetActive(state);
    }
    public void ToggleGUIinScene(bool state)
    {
        gui.SetActive(state);
    }
    public void ToggleOptionsMenu(bool state)
    {
        optionsMenu.SetActive(state);
    }
    public void ToggleLoadGameMenu(bool state)
    {
        loadGameMenu.SetActive(state);
    }
    public void ToggleNewsMenu(bool state)
    {
        newsMenu.SetActive(state);
    }

    public void TogglePauseMenu(bool state)
    {
        pauseScreen.SetActive(state);
    }

    public void TogglePauseMenuPopUp(bool state)
    {
        pausePopUp.SetActive(state);
    }
    public void ToggleSoundSettings(bool state)
    {
        inGameSoundOptions.SetActive(state);
    }
    public void ToggleGraphicsSettings(bool state)
    {
        inGameGraphicsOptions.SetActive(state);
    }
    public void ToggleControlsSettings(bool state)
    {
        inGameControlsOptions.SetActive(state);
    }

    public void ToggleInGameOptionsMenu(bool state)
    {
        inGameOptionsMenu.SetActive(state);
    }

    public void ExitZone()
    {
        if (exitZonePopUp.activeInHierarchy)
        {
            //InputManager._Instance.GetSetCanPlayerRotate = true;
            exitZonePopUp.SetActive(false);
            Time.timeScale = 1f;
    
        }

        else
        {
            exitZonePopUp.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void ExitZoneMap()
    {
        if (exitZoneMap.activeInHierarchy)
        {
            //player.getInputManager.GetSetCanPlayerRotate = true;
            Time.timeScale = 1f;
            exitZoneMap.SetActive(false);
            exitZonePopUp.SetActive(false);
            
        }

        else
        {
            Time.timeScale = 0f;
            exitZoneMap.SetActive(true);
        }
    }

    public void ReturnBack()
    {
        exitZoneMap.SetActive(false);
        exitZonePopUp.SetActive(false);
        player.getInputManager.ResetInputManager();
        Time.timeScale = 1f;
       //SceneHandler._Instance.SetPlayerToScene(SceneHandler._Instance.spawningPoint);
        // deploy platypus?
        TotemManager._Instance.CheckIfToSpawnBeastAtDetectionLocation();
    }
    public void CloseAllMenus() { }


    public void DropItemFromInventory(int i) {
        if (inventory[i] != null)
        {
            var itemToDrop = ItemFactory._Instance.GenerateItem(inventory[i].GetData.ID);
            itemToDrop.amount = inventory[i].amount;
          //  PickUpObject.SpawnItemInWorld(itemToDrop, PlayerManager._Instance.GetPlayerTransform.position, PlayerManager._Instance.GetPlayerTransform);
            _playerInventory.RemoveItemFromInventory(inventory[i]);
            //UpdateInventory();

        }
    }

    public void LoadLevel(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }










    #region Totem Icons
    [SerializeField] TotemIconImageScript[] totemIcons;
    int currentHighlighted = 0;
    int lastSlot ;
    public void HighLightNextImage() {

        currentHighlighted++;

        if (currentHighlighted >= totemIcons.Length)
            currentHighlighted = 0;


        if (totemIcons[currentHighlighted].GetIsDark) {
            HighLightNextImage();
            return;
        }
        totemIcons[lastSlot].HighLightImage(false);
        totemIcons[currentHighlighted].HighLightImage(true);
        PlayerManager._Instance.getPlayerCombat.SetCurrentTotemHolderByInt(currentHighlighted);
        lastSlot = currentHighlighted;
    }




  
   public void UpdateTotemsFromGamePhase(PlayPhase playPhase) {
        for (int i = 0; i < totemIcons.Length; i++)
            totemIcons[i].SetMySprite(playPhase);
    }
    #endregion
}
