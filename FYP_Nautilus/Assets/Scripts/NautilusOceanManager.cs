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
                maps[mapIndex].transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    private void Update()
    {
        Debug.Log(currentIndex);
        if(inMenu)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                maps[mapIndex].transform.GetChild(0).GetChild(currentIndex).GetComponent<Image>().color = Color.white;
                currentIndex--;
                if (currentIndex < 0)
                {
                    currentIndex = maxIndex;
                }
                showPointInfo();
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                currentIndex++;
                if (currentIndex > maxIndex)
                {
                    currentIndex = 0;
                }
                showPointInfo();
            }
        }
    }

    public void showPointInfo()
    {
        maps[mapIndex].transform.GetChild(0).GetChild(currentIndex).GetComponent<Image>().color = Color.red;
        if (mapIndex == 0)
        {
            pointName.text = playerInfo.RondaOceanDivePoints[currentIndex].pointName;
            pointDepth.text = playerInfo.RondaOceanDivePoints[currentIndex].depth.ToString();
            pointCoordinate.text = playerInfo.RondaOceanDivePoints[currentIndex].coordinate.x + " , " + playerInfo.RondaOceanDivePoints[currentIndex].coordinate.y;
        }
    }
}
