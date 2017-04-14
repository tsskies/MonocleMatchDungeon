using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Dungeon : MonoBehaviour {

    public static Dungeon dungeon;

    public GameObject currentEncounter;
    public GameObject currentRoom;
    // Getting scripts On the Dungeon GameObject
    public Player player;
    public Shop shop;
    public SaveLoad saveload;
    public SoundManager soundmanager;
    // UI
    // player UI Text
    public Text healthUI;
    public Text armorUI;
    public Text MoneyUI;
    // Dungeon UI Text
    public Text FloorUI;
    public Text RoomUI;
    public Text scoreUI;
    // Enemy UI Text
    public Slider EnemyHealthUI;
    public Slider EnemyattackTimeUI;
    // Shop UI Text
    public Text MaxArmorcost;
    public Text ArmorPickupCost;
    // used to stop and progress the dungeon
    public bool readyToMove = false;
    public bool holdForEncounter = false;
    private bool enemyIsDead = true;
    private bool GameOver = false;
    // used to progress the dungeon at a set rate
    private float gameSpeedCounter = 0;
    public float gameSpeed = 300;
    // Holds the current location of the player in the dungeon and score
    public float floor = 1;
    public int room = 0;
    public int currentScore = 0;
    // Holds farthest distance the player has traveled and Highest score
    public float MaxFloor = 1;
    public int Maxroom = 0;
    public int Highscore;
    // Holds save number
    public int SaveNum = 0;
    // Time before changing scene;
    public int deathTimer = 0;
    public int deathtime = 100;
    public bool deathTimeStart = false;



    private void Awake()
    {
        soundmanager = GameObject.FindGameObjectWithTag("sound").GetComponent<SoundManager>();
        player = GetComponent<Player>();
        shop = GetComponentInChildren<Shop>();
        saveload = GetComponent<SaveLoad>();
        SaveNum = GameObject.FindGameObjectWithTag("data").GetComponent<PersistentData>().savenum;
        UpdateUI();
        dungeon = this;
        saveload.Load(SaveNum,2);
        player.Upgrades();
        shop.UpdatePrices();
    }
    
   public void playSound(int sound)
    {
        if (soundmanager != null)
        {
            soundmanager.play(sound);
        }
    }
    
    public void skip()
    {
        floor++;
        room = 0;
        UpdateUI();
    }	
	// Update is called once per frame
	void FixedUpdate () {
        // Moves the game up if the dungeon has been set to ready to move by another script, it's not being held in place by an Encounter and the gamespeed counter has surpassed the game speed
        if (!GameOver)
        {
            if (readyToMove && !holdForEncounter && gameSpeedCounter >= gameSpeed)
            {
                moveForward();
                gameSpeedCounter = 0;
            }
            gameSpeedCounter++;
        }

        if (deathTimeStart)
        {
            if (deathTimer < deathtime)
            {
                DeathClock();
            }
            else
            {
                deathTimer++;
            }
        }
	}

    public void save()
    {
        if (saveload != null)
        {
            saveload.Save(SaveNum);
        }
    }

    public void EndDungeon()
    {
        playSound(3);
        
        GameOver = true;
        Highscore = currentScore;
        MaxFloor = floor;
        Maxroom = room;
        saveload.Save(SaveNum);
        deathTimeStart = true;
        
    }

    private void DeathClock()
    {
        SceneManager.LoadScene(1);
    }

    public void Crossroads(int direction)
    {
        // direction = 0, is left
        // direction = 1, is right;
        if (direction == 0)
        {
            floor = floor + .5f;
        }
        if (direction == 1)
        {
            floor++;
        }
        holdForEncounter = false;
        moveForward();
    }

    public void Score(int AddScore)
    {
        currentScore = currentScore + AddScore;
    }

    public void UpdateUI()
    {
        // Getting variables for player UI
        if (player.health <= 0)
        {
            healthUI.text = 0.0f.ToString("N0");
        }
        else
        {
            healthUI.text = player.health.ToString("N0");
        }
        
        armorUI.text = player.armor.ToString("N0");
        MoneyUI.text = player.money.ToString("N0");
        // Getting variables for dungeon UI
        FloorUI.text = floor.ToString("N0");
        RoomUI.text = room.ToString("N0");
        // Getting variables for Shop UI
        MaxArmorcost.text = ("cost: "+ (shop.CurrentMaxArmorCost));
        ArmorPickupCost.text = ("cost: " + (shop.CurrntArmorPickupCost));
        // Getting varibles for Enemy UI
        if (currentEncounter != null)
        {
            float attacktime = currentEncounter.GetComponent<Enemy>().attackSpeedPerSecond;
            float attackcounter = currentEncounter.GetComponent<Enemy>().attackSpeedCounter;

            attacktime = attackcounter/attacktime;

            if(currentEncounter.GetComponent<Enemy>().health <= 0)
            {
                EnemyHealthUI.value = 0.0f;
            }
            else
            {
                EnemyHealthUI.value = currentEncounter.GetComponent<Enemy>().health / currentEncounter.GetComponent<Enemy>().maxHealth;
            }
           

            EnemyattackTimeUI.value = (attacktime);
        }

    }

    public void moveForward()
    {
        if (currentRoom != null)
        {
            currentRoom.GetComponent<MovingWalls>().Move();
        }

        room++;
        Score((int)(10*floor));

        if (room > 10)
        {
            floor++;
            room = 0;
           // Score((int)(100f*floor));
            //holdForEncounter = true;
        }

        UpdateUI();
    }

    public void Damageplayer(float enemyDMG)
    {
        player.Damage(enemyDMG);
    }

    public void RewardPlayer(int reward)
    {
        player.GiveMoney(reward);
        Score(reward);
    }

    public void affectEncounter(int colorNum)
    {
        if (!GameOver)
        {
            Score((int)(1 * floor));
            // num 3 is the number that armor repair pieces are assigned. This calls the repair function on the player when an armor piece is cleared off the board
            if (colorNum == 3)
            {
                player.Repair();
            }
            // num 4 is the number that money pieces are assigned. This calls the giveMoney function on the player when a money piece is clear off the board
            if (colorNum == 4)
            {
                player.GiveMoney(0);
            }

            if (currentEncounter != null && !currentEncounter.GetComponent<Enemy>().IsDead)
            {
                currentEncounter.GetComponent<Enemy>().DamageEnemy(colorNum);
            }
        }
    }
}
