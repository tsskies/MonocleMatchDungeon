using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    public Dungeon dungeon;

    public float health = 100;
    public float armor = 50;
    public float armorRepairAmount = 1;
    public int money = 0;
    public float maxArmor = 50;

    public int ArmorUpgradeLevel = 1;
    public int ArmorPickupLevel = 1;

    private void Awake()
    {
        dungeon = GetComponent<Dungeon>();
        Upgrades();

    }

    public void Upgrades()
    {
        maxArmor = (ArmorUpgradeLevel * 50);
       // print(maxArmor);
        armorRepairAmount = (ArmorPickupLevel * 5);
        //print(armorRepairAmount);
        armor = maxArmor;
        if (dungeon != null)
        {
            dungeon.save();
        }
        
    }

    public void GiveMoney(int loot)
    {
        if (loot == 0)
        {
            money = money + 1;
        }
        else
        {
            money = money + loot;
        }
        dungeon.UpdateUI();
    }

    public void Repair()
    {
        //print(armor);
        armor = armor + (armorRepairAmount / 3);

        if (armor >= maxArmor)
        {
            armor = maxArmor;
        }

        dungeon.UpdateUI();
    }

    public bool Isdead()
    {
        if (health <= 0)
        {
            dungeon.EndDungeon();
            return (true);         
        }
        else
        {
            return (false);
        }
    }

    public void Damage(float dmg)
    {
        float remainingDmg = dmg;

        if (dungeon != null)
        {
            dungeon.playSound(2);
        }

        if (armor > 0)
        {
            remainingDmg = dmg - armor;
            armor = armor - dmg;

            if (armor <= 0)
            {
                armor = 0;
                health = health - dmg;
            }
        }
        else
        {
            health = health - dmg;
            if (health <= 0)
            {
                health = 0;
            }
        }

        Isdead();
        dungeon.UpdateUI();

    }


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
