using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NautilusOceanManager : MonoBehaviour
{
    public PlayerInfo playerInfo;

    public GameObject[] maps;

    int mapIndex;

    int currentIndex;
    int maxIndex;

    //UI element
    public TextMeshProUGUI pointName;
    public TextMeshProUGUI pointDepth;
    public TextMeshProUGUI pointCoordinate;

    bool inMenu;

    public void openMenu(int index, PlayerInfo info)
    {
        inMenu = true;
        playerInfo = info;
        mapIndex = index;
        gameObject.SetActive(true);
        currentIndex = 0;
        maps[mapIndex].gameObject.SetActive(true);
        if(mapIndex == 0)
        {
            //ronda ocean
            maxIndex = playerInfo.RondaOceanDivePoints.Length - 1;
        }
        for(int i = 0; i < maxIndex; i++)
        {
            if(playerInfo.RondaOceanDivePoints[i].unlocked)
            {
                Debug.Log(maps[mapIndex].transform.GetChild(0).GetChild(i).gameObject);
                maps[mapIndex].transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
            }
        }
        showPointInfo();
    }

    private void Update()
    {
        Debug.Log(currentIndex);
        if(inMenu)
        {
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
        }
    }

    public void showPointInfo()
    {
        GameObject pointObj = maps[mapIndex].transform.GetChild(0).GetChild(currentIndex).gameObject;
        pointObj.GetComponent<Image>().color = Color.red;
        pointObj.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
        if (mapIndex == 0)
        {
            //if the map is Ronda
            pointName.text = playerInfo.RondaOceanDivePoints[currentIndex].pointName;
            pointDepth.text = playerInfo.RondaOceanDivePoints[currentIndex].depth.ToString();
            pointCoordinate.text = playerInfo.RondaOceanDivePoints[currentIndex].coordinate.x + " , " + playerInfo.RondaOceanDivePoints[currentIndex].coordinate.y;
        }
    }
}
