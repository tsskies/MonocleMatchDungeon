using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    // public GameObject dungeonObject;
    public GameObject dungeon;
    private Dungeon dungeonscript;
    public AnimationClip Deathani;
    public AnimationClip Attackani;
    public AnimationClip Hitani;
    public SpriteRenderer spriteR;

    public Color Red;
    public Color Blue;
    public Color Green;

    public enum ColorType
    {
        RED,
        BLUE,
        GREEN,
        COUNT
    };

    [System.Serializable]
    public struct ColorSprite
    {
        public ColorType color;
        public Sprite sprite;
    }

    private bool isDead = false;

    public bool IsDead
    {
        get { return isDead; }
    }


    public bool interruptable = false;
    public float hitdamage = 33.33f;
    public float mismatchDamagePercentageDrop = 33.33f;
    public float enemyDamage = 10;
    public int rewardgiven = 10;
    public int enemyColor = 0;

    public float health = 100;
    public float maxHealth = 100;
    public float attackSpeedCounter = 0;
    public float attackSpeedPerSecond = 200f;

    public bool colorchange = false;
    public int changetimer = 0;
    public int changetime = 300;
    public int colorWheel = 0;

    public ColorSprite[] colorSprites;

    public ColorType color;

    private SpriteRenderer sprite;
    private Dictionary<ColorType, Sprite> colorSpriteDict;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();

        colorSpriteDict = new Dictionary<ColorType, Sprite>();

        //dungeon.GetComponent<Dungeon>().holdForEncounter = true;

        for (int i = 0; i < colorSprites.Length; i++)
        {
            if (!colorSpriteDict.ContainsKey(colorSprites[i].color))
            {
                colorSpriteDict.Add(colorSprites[i].color, colorSprites[i].sprite);
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        SetColor((ColorType)Random.Range(0, (int)ColorType.COUNT));

        //StartCoroutine(AttackCoroutine());
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        while (health > 0 && attackSpeedCounter >= attackSpeedPerSecond)
        {
            attack();
            attackSpeedCounter = 0;
        }
        attackSpeedCounter++;

        if (dungeon != null)
        {
            dungeon.GetComponent<Dungeon>().UpdateUI();
        }

        if (colorchange)
        {
            if (changetimer > changetime)
            {
                if (colorWheel == 0)
                {
                    //sprite.sprite = colorSpriteDict[ColorType.RED];
                    color = ColorType.RED;
                    enemyColor = (int)color;
                   // Animator animator = GetComponent<Animator>();
                   // animator.SetInteger("EnemyColor", enemyColor);
                    colorWheel = 1;
                    spriteR.color = Red;
                    changetimer = 0;

                }
                if (colorWheel == 1)
                {
                   // sprite.sprite = colorSpriteDict[ColorType.RED];
                    color = ColorType.BLUE;
                    enemyColor = (int)color;
                    //Animator animator = GetComponent<Animator>();
                    //animator.SetInteger("EnemyColor", enemyColor);
                    colorWheel = 2;
                    spriteR.color = Blue;
                    changetimer = 0;
                }
                if (colorWheel == 2)
                {
                    //sprite.sprite = colorSpriteDict[ColorType.RED];
                    color = ColorType.GREEN;
                    enemyColor = (int)color;
                    //Animator animator = GetComponent<Animator>();
                    //animator.SetInteger("EnemyColor", enemyColor);
                    colorWheel = 0;
                    spriteR.color = Green;
                    changetimer = 0;
                }

                changetimer = 0;
            }

            changetimer++;
        }
    }

    public void SetDifficulty(float floor)
    {
        if (floor > 1)
        {
            floor = (floor - (floor * .5f));
        }

        //health = health * floor;
        attackSpeedPerSecond = attackSpeedPerSecond - (attackSpeedPerSecond * (.1f * floor));
        //enemyDamage = enemyDamage * floor;
        rewardgiven = (int)(rewardgiven * floor);
    }

    public void attack()
    {
        //print("attack");    
        dungeon.GetComponent<Dungeon>().Damageplayer(enemyDamage);
        StartCoroutine(AttacksCoroutine());

    }

    public void SetColor(ColorType newColor)
    {
        sprite.sprite = colorSpriteDict[newColor];
        color = newColor;
        enemyColor = (int)color;
        Animator animator = GetComponent<Animator>();
        animator.SetInteger("EnemyColor", enemyColor);

        //Red/Bar/Skele'man
        if (enemyColor == 0)
        {
            interruptable = false;
            hitdamage = 33.33f;
            mismatchDamagePercentageDrop = .33f;
            enemyDamage = 10;
            rewardgiven = 10;

            health = 100;

            attackSpeedPerSecond = 200f;

            
        }
        //Blue/Mage
        if (enemyColor == 1)
        {
            interruptable = true;
            hitdamage = 50.33f;
            mismatchDamagePercentageDrop = .50f;
            enemyDamage = 20;
            rewardgiven = 7;

            health = 75;

            attackSpeedPerSecond = 150f;
        }
        //Green
        if (enemyColor == 2)
        {
            interruptable = true;
            hitdamage = 10.33f;
            mismatchDamagePercentageDrop = .12f;
            enemyDamage = 3;
            rewardgiven = 5;

            health = 50;

            attackSpeedPerSecond = 100f;
        }

        if (dungeon != null)
        {
            if (dungeon.GetComponent<Dungeon>().floor == 1)
            {

                newColor = ColorType.RED;
                sprite.sprite = colorSpriteDict[newColor];
                color = newColor;
                enemyColor = (int)color;
                animator = GetComponent<Animator>();
                animator.SetInteger("EnemyColor", enemyColor);

                interruptable = false;
                hitdamage = 33.33f;
                mismatchDamagePercentageDrop = .33f;
                enemyDamage = 10;
                rewardgiven = 10;

                health = 100;

                attackSpeedPerSecond = 200f;
            }

            if (dungeon.GetComponent<Dungeon>().floor == 2)
            {

                newColor = ColorType.BLUE;
                sprite.sprite = colorSpriteDict[newColor];
                color = newColor;
                enemyColor = (int)color;
                animator = GetComponent<Animator>();
                animator.SetInteger("EnemyColor", enemyColor);

                interruptable = true;
                hitdamage = 50.33f;
                mismatchDamagePercentageDrop = .50f;
                enemyDamage = 20;
                rewardgiven = 7;

                health = 75;

                attackSpeedPerSecond = 150f;
            }


            if (dungeon.GetComponent<Dungeon>().floor == 3)
            {
                newColor = ColorType.GREEN;
                sprite.sprite = colorSpriteDict[newColor];
                color = newColor;
                enemyColor = (int)color;
                animator = GetComponent<Animator>();
                animator.SetInteger("EnemyColor", enemyColor);

                interruptable = true;
                hitdamage = 10.33f;
                mismatchDamagePercentageDrop = .12f;
                enemyDamage = 2;
                rewardgiven = 5;

                health = 50;

                attackSpeedPerSecond = 50f;

                if (dungeon.GetComponent<Dungeon>().room == 10)
                {
                    newColor = ColorType.BLUE;
                    sprite.sprite = colorSpriteDict[newColor];
                    color = newColor;
                    enemyColor = (int)color;
                    animator = GetComponent<Animator>();
                    animator.SetInteger("EnemyColor", enemyColor);

                    interruptable = true;
                    hitdamage = 15.33f;
                    mismatchDamagePercentageDrop = .60f;
                    enemyDamage = 25;
                    rewardgiven = 150;

                    health = 100;

                    attackSpeedPerSecond = 400f;
                }
            }

            if (dungeon.GetComponent<Dungeon>().floor == 4)
            {
                float chance = Random.Range(0, 1);
                if (chance == 0)
                {
                    newColor = ColorType.RED;
                }

                if (chance == 1)
                {
                    newColor = ColorType.BLUE;
                }

                sprite.sprite = colorSpriteDict[newColor];
                color = newColor;
                enemyColor = (int)color;
                animator = GetComponent<Animator>();
                animator.SetInteger("EnemyColor", enemyColor);

                if (enemyColor == 0)
                {
                    interruptable = false;
                    hitdamage = 33.33f;
                    mismatchDamagePercentageDrop = .33f;
                    enemyDamage = 10;
                    rewardgiven = 10;

                    health = 100;

                    attackSpeedPerSecond = 200f;
                }

                if (enemyColor == 1)
                {
                    interruptable = true;
                    hitdamage = 50.33f;
                    mismatchDamagePercentageDrop = .50f;
                    enemyDamage = 20;
                    rewardgiven = 7;

                    health = 75;

                    attackSpeedPerSecond = 150f;
                }
            }

            if (dungeon.GetComponent<Dungeon>().floor == 5)
            {
                float chance = Random.Range(0, 1);
                if (chance == 0)
                {
                    newColor = ColorType.RED;
                }

                if (chance == 1)
                {
                    newColor = ColorType.GREEN;
                }

                sprite.sprite = colorSpriteDict[newColor];
                color = newColor;
                enemyColor = (int)color;
                animator = GetComponent<Animator>();
                animator.SetInteger("EnemyColor", enemyColor);

                if (enemyColor == 0)
                {
                    interruptable = false;
                    hitdamage = 33.33f;
                    mismatchDamagePercentageDrop = .33f;
                    enemyDamage = 10;
                    rewardgiven = 10;

                    health = 100;

                    attackSpeedPerSecond = 200f;
                }

                if (enemyColor == 2)
                {
                    interruptable = true;
                    hitdamage = 10.33f;
                    mismatchDamagePercentageDrop = .12f;
                    enemyDamage = 3;
                    rewardgiven = 5;

                    health = 50;

                    attackSpeedPerSecond = 100f;
                }
            }

            if (dungeon.GetComponent<Dungeon>().floor == 6)
            {
                newColor = ColorType.BLUE;
                sprite.sprite = colorSpriteDict[newColor];
                color = newColor;
                enemyColor = (int)color;
                animator = GetComponent<Animator>();
                animator.SetInteger("EnemyColor", enemyColor);

                interruptable = true;
                hitdamage = 10.33f;
                mismatchDamagePercentageDrop = .12f;
                enemyDamage = 20;
                rewardgiven = 20;

                health = 70;

                attackSpeedPerSecond = 400f;

                if (dungeon.GetComponent<Dungeon>().room == 10)
                {
                    newColor = ColorType.RED;
                    sprite.sprite = colorSpriteDict[newColor];
                    color = newColor;
                    enemyColor = (int)color;
                    animator = GetComponent<Animator>();
                    animator.SetInteger("EnemyColor", enemyColor);

                    interruptable = true;
                    hitdamage = 15.33f;
                    mismatchDamagePercentageDrop = .60f;
                    enemyDamage = 15;
                    rewardgiven = 150;

                    health = 100;

                    attackSpeedPerSecond = 250f;
                    colorchange = true;
                    spriteR.color = Red;
                }
            }

            if (dungeon.GetComponent<Dungeon>().floor == 10)
            {
                newColor = ColorType.BLUE;
                sprite.sprite = colorSpriteDict[newColor];
                color = newColor;
                enemyColor = (int)color;
                animator = GetComponent<Animator>();
                animator.SetInteger("EnemyColor", enemyColor);

                interruptable = true;
                hitdamage = 10.33f;
                mismatchDamagePercentageDrop = .12f;
                enemyDamage = 20;
                rewardgiven = 20;

                health = 70;

                attackSpeedPerSecond = 400f;

                if (dungeon.GetComponent<Dungeon>().room == 3)
                {
                    newColor = ColorType.BLUE;
                    sprite.sprite = colorSpriteDict[newColor];
                    color = newColor;
                    enemyColor = (int)color;
                    animator = GetComponent<Animator>();
                    animator.SetInteger("EnemyColor", enemyColor);

                    interruptable = true;
                    hitdamage = 15.33f;
                    mismatchDamagePercentageDrop = .60f;
                    enemyDamage = 25;
                    rewardgiven = 150;

                    health = 100;

                    attackSpeedPerSecond = 400f;
                }

                if (dungeon.GetComponent<Dungeon>().room == 6)
                {
                    newColor = ColorType.RED;
                    sprite.sprite = colorSpriteDict[newColor];
                    color = newColor;
                    enemyColor = (int)color;
                    animator = GetComponent<Animator>();
                    animator.SetInteger("EnemyColor", enemyColor);

                    interruptable = true;
                    hitdamage = 15.33f;
                    mismatchDamagePercentageDrop = .60f;
                    enemyDamage = 15;
                    rewardgiven = 150;

                    health = 100;

                    attackSpeedPerSecond = 250f;
                    colorchange = true;
                    spriteR.color = Red;
                }

                if (dungeon.GetComponent<Dungeon>().room == 10)
                {
                    newColor = ColorType.GREEN;
                    sprite.sprite = colorSpriteDict[newColor];
                    color = newColor;
                    enemyColor = (int)color;
                    animator = GetComponent<Animator>();
                    animator.SetInteger("EnemyColor", enemyColor);

                    interruptable = true;
                    hitdamage = 15.33f;
                    mismatchDamagePercentageDrop = .60f;
                    enemyDamage = 25;
                    rewardgiven = 150;

                    health = 100;

                    attackSpeedPerSecond = 100f;
                    colorchange = true;
                    spriteR.color = Green;
                }
            }
        }

        if (dungeon != null)
        {
            SetDifficulty(dungeon.GetComponent<Dungeon>().floor);
        }
        maxHealth = health;
    }

    private void Death()
    {
        if (dungeon != null)
        {
            dungeon.GetComponent<Dungeon>().playSound(1);
        }
        Destroy(this.gameObject);
        dungeon.GetComponent<Dungeon>().holdForEncounter = false;
        dungeon.GetComponent<Dungeon>().readyToMove = true;
        dungeon.GetComponent<Dungeon>().RewardPlayer(rewardgiven);
        dungeon.GetComponent<Dungeon>().Score((int)rewardgiven);
    }

    private void CheckIfdead()
    {
        if (health <= 0)
        {
            isDead = true;
            StartCoroutine(DieCoroutine());
            health = 0;
        }

    }


    private IEnumerator DieCoroutine()
    {
        Animator animator = GetComponent<Animator>();
        animator.SetBool("isdead", true);
        // animator.Play(Deathani.name);

        yield return new WaitForSeconds(Deathani.length);
        Death();
    }

    private IEnumerator AttacksCoroutine()
    {
        Animator animator = GetComponent<Animator>();
        animator.SetBool("attacking", true);

        yield return new WaitForSeconds(Attackani.length);
        animator.SetBool("attacking", false);
    }

    private IEnumerator HitCoroutine()
    {
        Animator animator = GetComponent<Animator>();
        animator.SetBool("hit", true);

        yield return new WaitForSeconds(Hitani.length);
        animator.SetBool("hit", false);
    }

    public void DamageEnemy(int DMGcolor)
    {
        //print(color);
        StartCoroutine(HitCoroutine());
        if (dungeon != null)
        {
            dungeon.GetComponent<Dungeon>().playSound(0);
        }
        if (interruptable)
        {
            attackSpeedCounter = 0;
        }

        if (DMGcolor < 3 && !IsDead)
        {
            if (DMGcolor == (int)color)
            {
                // print("hit");
                health = health - (hitdamage / 3);
            }
            else
            {
                // print("Weak hit");
                health = health - ((hitdamage / 3) * mismatchDamagePercentageDrop);
            }

            CheckIfdead();
            // print(health);
        }
    }
}
