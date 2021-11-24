using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Warehouse : MonoBehaviour
{
    private int Food;
    private int Resource;
    [SerializeField]
    private TextMeshProUGUI foodText , resourceText, storageLeft;
    [SerializeField]
    private GameObject Storage;
    [SerializeField]
    private GameObject RepairShop;
    [SerializeField]
    private GameObject LandingPlatform;
    [SerializeField]
    private int StorageCapacity;
    private int CurrentStorageCapacity;
    [SerializeField]
    private GameObject BuildingMenu;
    public int amountLeft = 0;

    private bool swithBuildMenu = true;
    private void Start()
    {
        foodText.text = Food.ToString();
        resourceText.text = Resource.ToString();
        storageLeft.text = StorageCapacity.ToString();
        CurrentStorageCapacity = StorageCapacity;
    }

    public void AddRemoveFood(bool addRemove, bool foodResourse, int amount)
    {
        
        if (addRemove)
        {
            if(StorageCapacity - (Food + Resource) > 0)
            {
                if (foodResourse)
                {
                    amountLeft = StorageCapacity - amount;
                    if (amountLeft >= 0)
                    {
                        Food += amount;
                        StorageCapacity -= amount;
                        amountLeft = 0;
                    }
                    else
                    {
                        Food += amount + amountLeft;
                        amountLeft = Mathf.Abs(amountLeft);
                        StorageCapacity = 0;
                    }

                }
                else
                {
                    amountLeft = StorageCapacity - amount;
                    if (amountLeft >= 0)
                    {
                        Resource += amount;
                        StorageCapacity -= amount;
                        amountLeft = 0;
                    }
                    else
                    {
                        Resource += amount + amountLeft;
                        amountLeft = Mathf.Abs(amountLeft);
                        StorageCapacity = 0;
                    }
                }
            }
        }
        else
        {
            if (foodResourse)
            {
                Food -= amount;
                StorageCapacity += amount;
            }
            else
            {
                Resource -= amount;
                StorageCapacity += amount;
            }
        }
        
        foodText.text = Food.ToString();
        resourceText.text = Resource.ToString();
        storageLeft.text = StorageCapacity.ToString();
    }

    public void BuildStorage(int resources)
    {
        if (resources <= Resource || resources == 0)
        {
            Storage.SetActive(true);
            AddRemoveFood(false, false, resources);
            LandingPlatform.gameObject.tag = "Untagged";
            StorageCapacity += 50;
            CurrentStorageCapacity =StorageCapacity;
            storageLeft.text = StorageCapacity.ToString();
            OpenFogTile fog = Storage.GetComponent<OpenFogTile>();
            fog.OpenFogTileAction();

        }

    }
    public void BuildRepairShop(int resources)
    {
        if (resources <= Resource || resources ==0)
        {
            RepairShop.SetActive(true);
            AddRemoveFood(false, false, resources);
            OpenFogTile fog = RepairShop.GetComponent<OpenFogTile>();
            fog.OpenFogTileAction();
        }

    }

    public void OpenBuildMenu()
    {
        swithBuildMenu = !swithBuildMenu;
        if (!swithBuildMenu) { BuildingMenu.SetActive(true); }
        else
        {
            BuildingMenu.SetActive(false); 
        }

    }

    public void EmptyFoodStored()
    {
        StorageCapacity += Food;
        Food = 0;
        foodText.text = Food.ToString();
        resourceText.text = Resource.ToString();
        storageLeft.text = StorageCapacity.ToString();
    }
    public void EmptyResourceStored()
    {
        StorageCapacity += Resource;
        Resource = 0;
        foodText.text = Food.ToString();
        resourceText.text = Resource.ToString();
        storageLeft.text = StorageCapacity.ToString();
    }

}


