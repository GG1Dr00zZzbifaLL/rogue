using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRoom : MonoBehaviour
{
    [Header("walls")]
    public GameObject[] walls;
    public GameObject door;
    public GameObject wallSound;
    public GameObject wallEffect;

    [Header("Enemies")]
    public GameObject[] enemyTypes;
    public Transform[] enemySpawners; 

    [Header("PowerUps")]
    public GameObject shield;
    public GameObject healthPotion;

    [HideInInspector] public List<GameObject> enemies;

    [HideInInspector] public bool spawned;

    private RoomVariants variants;
    private bool wallsDestroyed;

    private void Awake()
    {
        variants = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomVariants>();
    }

    private void Start()
    {
        variants.rooms.Add(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //настройка случайного появления врагов и бонусов 
        if (other.CompareTag("Player") && !spawned)
        {
            spawned = true;
            foreach (Transform spawner in enemySpawners)
            {
                int rand = Random.Range(0, 101);
                if (rand < 91)
                {
                    GameObject enemyType = enemyTypes[Random.Range(0, enemyTypes.Length)];
                    GameObject enemy = Instantiate(enemyType, spawner.position, Quaternion.identity) as GameObject;
                    enemy.transform.parent = transform;
                    enemies.Add(enemy);
                }
                else if (rand >= 91 && rand <= 95 )
                {
                    Instantiate(healthPotion, spawner.position, Quaternion.identity);
                }
                else if (rand > 95)
                {
                    Instantiate(shield, spawner.position, Quaternion.identity);
                }
            }
            StartCoroutine(CheckEnemies()); 
        }
        else if(other.CompareTag("Player") && spawned)
        {
            foreach(GameObject enemy in enemies)
            {
                enemy.GetComponent<Enemy>().playerNotInRoom = false;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && spawned)
        {
            foreach (GameObject enemy in enemies)
            {
                enemy.GetComponent<Enemy>().playerNotInRoom = true;
            }
        }
    }

    IEnumerator CheckEnemies()
    {
        //уничтожение стен при отсутствии врагов
        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => enemies.Count == 0); 
        DestroyWalls();
    }

    public void DestroyWalls()
    {
        //само уничтожение стен
        Instantiate(wallSound, transform.position, Quaternion.identity);

        foreach (GameObject wall in walls)
        {
            if (wall != null && wall.transform.childCount != 0)
            {
                Instantiate(wallEffect, wall.transform.position, Quaternion.identity);
                Destroy(wall);
            }
        }
        wallsDestroyed = true;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        //Чтобы стены не спавнились друг в друге 
        if (wallsDestroyed && other.CompareTag("wall"))
        {
            Destroy(other.gameObject);
        }
    }
}