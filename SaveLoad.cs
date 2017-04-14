using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveLoad : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    public void Save(int SaveNum)
    {
            FileStream Fstream = File.Create(Application.persistentDataPath + "/LPSASave" + SaveNum + ".Sv");

            BinaryFormatter binary = new BinaryFormatter();

        
            DataHolder saveFile = new DataHolder();

        if (Dungeon.dungeon != null)
        {
            saveFile.Highscore = Dungeon.dungeon.Highscore;
            saveFile.FarthestFloor = Dungeon.dungeon.MaxFloor;
            saveFile.farthestRoom = Dungeon.dungeon.Maxroom;
            saveFile.MaxArmorUpgradeLevel = Dungeon.dungeon.player.ArmorUpgradeLevel;
            saveFile.ArmorPickupUpgradeLevel = Dungeon.dungeon.player.ArmorPickupLevel;
            saveFile.money = Dungeon.dungeon.player.money;
        }

        binary.Serialize(Fstream, saveFile);
            Fstream.Close();
       
    }

    public void Load(int SaveNum, int Level)
    {
        if (File.Exists(Application.persistentDataPath + "/LPSASave" + SaveNum + ".Sv"))
        {
            BinaryFormatter binary = new BinaryFormatter();
            FileStream fstream = File.Open(Application.persistentDataPath + "/LPSASave" + SaveNum + ".Sv", FileMode.Open, FileAccess.Read);
            DataHolder saveFile = (DataHolder)binary.Deserialize(fstream);
            
            
            if (Level == 0)
            {
                MenuScript MainMenu = GameObject.FindGameObjectWithTag("menu").GetComponent<MenuScript>();
                MainMenu.highscore = saveFile.Highscore;
                MainMenu.FarthestFloor = saveFile.FarthestFloor;
                MainMenu.FarthestRoom = saveFile.farthestRoom;
                MainMenu.ArmorUpgrade = saveFile.MaxArmorUpgradeLevel;
                MainMenu.ArmorPickup = saveFile.ArmorPickupUpgradeLevel;
                MainMenu.Money = saveFile.money;
            }

            if (Level == 2)
            {
                Dungeon.dungeon.Highscore = saveFile.Highscore;
                Dungeon.dungeon.MaxFloor = saveFile.FarthestFloor;
                Dungeon.dungeon.Maxroom = saveFile.farthestRoom;
                Dungeon.dungeon.player.ArmorUpgradeLevel = saveFile.MaxArmorUpgradeLevel;
                Dungeon.dungeon.player.ArmorPickupLevel = saveFile.ArmorPickupUpgradeLevel;
                Dungeon.dungeon.player.money = saveFile.money;
            }
            
            fstream.Close();
        }

    }

    public void Clear(int SaveNum)
    {
        if (File.Exists(Application.persistentDataPath + "/LPSASave" + SaveNum + ".Sv"))
        {
            File.Delete(Application.persistentDataPath + "/LPSASave" + SaveNum + ".Sv");
            
            //fstream.Close();

        }
    }

    public bool Check(int SaveNum)
    {
        if (File.Exists(Application.persistentDataPath + "/LPSASave" + SaveNum + ".Sv"))
        {
            return (true);
        }

        return (false);
    }
}

[Serializable]
class DataHolder
{
    public int Highscore;
    public float FarthestFloor;
    public int farthestRoom;
    public int MaxArmorUpgradeLevel;
    public int ArmorPickupUpgradeLevel;
    public int money;

}
