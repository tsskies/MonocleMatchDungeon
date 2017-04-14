using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour {


    public Dungeon dungeon;

    // base cost for items in the shop
    public int UpgradeMaxArmorCost = 100;
    public int UpgradeArmorPickupCost = 100;
    // Current Level of upgrades 
    public int UpgradeMaxArmorLevel = 0;
    public int UpgradeArmorPickupLevel = 0;
    // current cost
    public int CurrentMaxArmorCost = 100;
    public int CurrntArmorPickupCost = 100;

    // Use this for initialization
    void Awake()
    {
        dungeon = GetComponentInParent<Dungeon>();
    }

    public void UpdatePrices()
    {
        if (dungeon != null)
        {
            UpgradeMaxArmorLevel = dungeon.player.ArmorUpgradeLevel;
            UpgradeArmorPickupLevel = dungeon.player.ArmorPickupLevel;
            CurrentMaxArmorCost = (UpgradeMaxArmorCost + (UpgradeMaxArmorCost * UpgradeMaxArmorLevel));
            CurrntArmorPickupCost = (UpgradeArmorPickupCost + (UpgradeArmorPickupCost * UpgradeArmorPickupLevel));
            dungeon.UpdateUI();
        }
    }

    public void buy(int itemIndex)
    {
        // itemIndex map
        // 0 = UpgradeMaxArmor
        // 1 = UpgradeArmorPickup

        //UpdatePrices();

        int money = (int)dungeon.player.money;

        CurrentMaxArmorCost = (UpgradeMaxArmorCost + (UpgradeMaxArmorCost * UpgradeMaxArmorLevel));
        CurrntArmorPickupCost = (UpgradeArmorPickupCost + (UpgradeArmorPickupCost * UpgradeArmorPickupLevel));
        // checking if the moneny given is enough to buy the upgrade at it's current level
        print(CurrentMaxArmorCost);
        print(CurrntArmorPickupCost);
        print(money);
        if (itemIndex == 0 && money >= CurrentMaxArmorCost)
        {
            dungeon.player.money = dungeon.player.money - CurrentMaxArmorCost;
            dungeon.player.ArmorUpgradeLevel++;
        }

        if (itemIndex == 1 && money >= CurrntArmorPickupCost)
        {
            print("Inside");
            dungeon.player.money = dungeon.player.money - CurrntArmorPickupCost;
            dungeon.player.ArmorPickupLevel++;
        }


        CurrentMaxArmorCost = (UpgradeMaxArmorCost + (UpgradeMaxArmorCost * UpgradeMaxArmorLevel));
        CurrntArmorPickupCost = (UpgradeArmorPickupCost + (UpgradeArmorPickupCost * UpgradeArmorPickupLevel));
        dungeon.player.Upgrades();

    }

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
