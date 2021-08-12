using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NautilusOceanManager : MonoBehaviour
{
    public NautilusMenuManager nautilusMenuManager;
    public PlayerInfo playerInfo;

    public GameObject[] maps;

    int mapIndex;

    int currentIndex;
    int maxIndex;

    //UI element
    public TextMeshProUGUI pointName;
    public TextMeshProUGUI pointDepth;
    public TextMeshProUGUI pointCoordinate;

    public GameObject confirmPanel;
    public int confirm; //0 is not input yet, 1 is confirm and -1 is denied

    bool inMenu;

    int sceneIndex;
    int pointIndex;

    public void openMenu(int index, PlayerInfo info)
    {
        //init menu
        inMenu = true;
        playerInfo = info;
        mapIndex = index;
        gameObject.SetActive(true);
        currentIndex = 0;
        //set map active
        maps[mapIndex].gameObject.SetActive(true);
        if(mapIndex == 0)
        {
            //ronda ocean
            maxIndex = playerInfo.RondaOceanDivePoints.Length - 1;
        }
        //set available points active
        for(int i = 0; i < maxIndex; i++)
        {
            if(playerInfo.RondaOceanDivePoints[i].unlocked)
            {
                maps[mapIndex].transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
            }
        }
        showPointInfo();
    }

    public void closeMenu()
    {
        //close menu 
        inMenu = false;
        gameObject.SetActive(false);
        foreach(GameObject map in maps)
        {
            map.SetActive(false);
        }
        nautilusMenuManager.openMainMenu();
    }

    private void Update()
    {     
        if(inMenu)
        {
            //input choosing points
            if (Input.GetKeyDown(KeyCode.W))
            {
                int originalIndex = currentIndex;
                GameObject pointObj = maps[mapIndex].transform.GetChild(0).GetChild(currentIndex).gameObject;
                pointObj.GetComponent<Image>().color = Color.white;
                pointObj.transform.localScale = new Vector3(1, 1, 1);
                currentIndex--;
                if (currentIndex < 0)
                {
                    currentIndex = maxIndex;
                }
                while (!playerInfo.RondaOceanDivePoints[currentIndex].unlocked)
                {
                    currentIndex--;
                    if (currentIndex < 0)
                    {
                        currentIndex = maxIndex;
                    }
                    if (currentIndex == originalIndex)
                    {
                        break;
                    }
                }
                showPointInfo();
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                int originalIndex = currentIndex;
                GameObject pointObj = maps[mapIndex].transform.GetChild(0).GetChild(currentIndex).gameObject;
                pointObj.GetComponent<Image>().color = Color.white;
                pointObj.transform.localScale = new Vector3(1, 1, 1);
                currentIndex++;
                if (currentIndex > maxIndex)
                {
                    currentIndex = 0;
                }
                while (!playerInfo.RondaOceanDivePoints[currentIndex].unlocked)
                {
                    currentIndex--;
                    if (currentIndex > maxIndex)
                    {
                        currentIndex = 0;
                    }
                    if(currentIndex == originalIndex)
                    {
                        break;
                    }
                }
                showPointInfo();
            }
            //input confirm
            if(Input.GetButtonDown("Submit"))
            {
                switch(mapIndex)
                {
                    case 0:
                        OceanDivePoint point = playerInfo.RondaOceanDivePoints[currentIndex];
                        sceneIndex = point.sceneIndex;
                        pointIndex = point.pointIndex;
                        StartCoroutine(confirmTravel());
                        break;
                    //more maps...
                }
            }
        }
    }

    public void showPointInfo()
    {
        GameObject pointObj = maps[mapIndex].transform.GetChild(0).GetChild(currentIndex).gameObject;
        pointObj.GetComponent<Image>().color = Color.red;
        pointObj.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        if (mapIndex == 0)
        {
            //if the map is Ronda
            pointName.text = playerInfo.RondaOceanDivePoints[currentIndex].pointName;
            pointDepth.text = playerInfo.RondaOceanDivePoints[currentIndex].depth.ToString();
            pointCoordinate.text = playerInfo.RondaOceanDivePoints[currentIndex].coordinate.x + " , " + playerInfo.RondaOceanDivePoints[currentIndex].coordinate.y;
        }
    }

    public IEnumerator confirmTravel()
    {
        confirmPanel.SetActive(true);
        while(confirm == 0)
        {
            yield return null;
        }
        confirmPanel.SetActive(false);
        if(confirm == 1)
        {
            //confirm travel
            //close menu
            closeMenu();
            nautilusMenuManager.closeMenu();
            TransferManager transferManager = FindObjectOfType<TransferManager>();
            transferManager.toScene = sceneIndex;
            transferManager.toIndex = pointIndex;
            StartCoroutine(FindObjectOfType<TransferManager>().transfer(nautilusMenuManager.playerObject));
        }
        else
        {
            confirm = 0;
        }
    }

    public void buttonConfirm(bool confirmation)
    {
        confirm = confirmation ? 1 : -1; 
    }
}
