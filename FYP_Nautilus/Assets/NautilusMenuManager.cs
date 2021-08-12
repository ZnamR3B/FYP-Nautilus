using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NautilusMenuManager : MonoBehaviour
{
    GameObject playerObject;
    PlayerInteractionController playerInteractionController;

    public GameObject mainMenu;
    public GameObject oceanMap;
    public GameObject teamMemberMap;

    public PlayerInfo playerInfo;
    public NautilusOceanManager nautilusOceanManager;

    public Transform exit;

    int mapIndex; // 0 = ronda
    bool inMenu;
    bool inSubMenu;

    private void Update()
    {
        if(inMenu)
        {
            if (Input.GetButtonDown("Cancel"))
            {
                closeMenu();
            }
        }
    }
    public void openOceanMap()
    {
        inSubMenu = true;
        inMenu = false;
        nautilusOceanManager.openMenu(mapIndex, playerInfo);
        mainMenu.SetActive(false);
        teamMemberMap.SetActive(false);
    }

    public void openMemberChangeMenu()
    {
        inSubMenu = true;
        inMenu = false;
    }

    public void openMainMenu()
    {
        //menu on
        inMenu = true;
        inSubMenu = false;
        //locked cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //get player info
        if (playerInfo == null)
            playerInfo = FindObjectOfType<PlayerInfo>();

        mapIndex = playerInfo.mapIndex;
        mainMenu.SetActive(true);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerObject = other.gameObject;
            playerInteractionController = other.GetComponent<PlayerInteractionController>();
            playerInteractionController.setPlayerLock(true);
            playerInteractionController.inInteraction = true;
            openMainMenu();
        }
    }

    public void closeMenu()
    {
        //menu off
        mainMenu.SetActive(false);
        //player interaction setting
        playerInteractionController.setPlayerLock(false);
        playerInteractionController.inInteraction = false;
        //unlock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //set player pos
        playerObject.transform.position = exit.position;
        playerObject.transform.localRotation = exit.localRotation;
    }
}
