using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingWalls : MonoBehaviour {

    public GameObject Nextwall;
    public GameObject enemytype;
    public GameObject dungeon;

    public GameObject enemy;

    public AnimationClip moveAnimation;

    public int spawnenemynum = 0;

    public bool isCurrentRoom = false;

    private bool hasEnemy = false;

    public bool HasEnemy
    {
        get { return hasEnemy; }
    }

    private bool enemySpawned = false;

    private bool isBeingMoved = false;

    public bool IsbeingMoved
    {
        get { return isBeingMoved; }
    }

    //protected GamePiece wall;

    void Awake()
    {
        if (Random.value >= 0.5 || spawnenemynum > 1)
        {
            hasEnemy = true;
            spawnenemynum = 0;
        }
        else
        {
            hasEnemy = false;
            spawnenemynum++;
        }
    }
    // Use this for initialization
    void Start()
    {
        dungeon.GetComponent<Dungeon>().currentRoom = this.gameObject;
        
        if (hasEnemy && !enemySpawned)
        {
            GameObject newEnemy = (GameObject)Instantiate(enemytype, new Vector3(this.transform.position.x, this.transform.position.y - .5f, this.transform.position.z - 1.01f), Quaternion.identity);  
            newEnemy.transform.parent = this.transform;
            newEnemy.transform.localScale = new Vector3 (1f,1f,1f);
            enemy = newEnemy;
            dungeon.GetComponent<Dungeon>().currentEncounter = newEnemy;
            enemySpawned = true;
            enemy.GetComponent<Enemy>().dungeon = dungeon;
            enemy.GetComponent<Enemy>().SetDifficulty(dungeon.GetComponent<Dungeon>().floor);
            dungeon.GetComponent<Dungeon>().holdForEncounter = true;
        }
        else
        {
            dungeon.GetComponent<Dungeon>().readyToMove = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        if (!isBeingMoved)
        Move();   
    }

    public void Move()
    {
        isBeingMoved = true;
        StartCoroutine(ClearCoroutine());
    }

    private IEnumerator ClearCoroutine()
    {
        Animator animator = GetComponent<Animator>();

        if (animator)
        {

            GameObject wall = (GameObject)Instantiate(Nextwall, new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity);

            if (dungeon.GetComponent<Dungeon>().room == 10)
            {
                spawnenemynum = 5;
            }
            wall.GetComponent<MovingWalls>().spawnenemynum = spawnenemynum;
            wall.GetComponent<MovingWalls>().Awake();

            animator.Play(moveAnimation.name);

            yield return new WaitForSeconds(moveAnimation.length);



            Destroy(this.gameObject);

            isBeingMoved = false;
        }
    }
}
