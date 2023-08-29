using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomVariants : MonoBehaviour
{
    public GameObject[] topRooms;
    public GameObject[] bottomRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;
    public GameObject[] GunTypes;
    public GameObject key;
    public GameObject boss;

    [HideInInspector] public List<GameObject> rooms;

    [HideInInspector] public List<GameObject> Gunes;

    private void Start()
    {
        StartCoroutine(RandomSpawner());
    }

    IEnumerator RandomSpawner()
    {
        //ключ, босс и оружие появятся с задержкой в 4 секунды
        yield return new WaitForSeconds(4f);
        AddRoom lastRoom = rooms[rooms.Count - 1].GetComponent<AddRoom>();
        int rand = Random.Range(0, rooms.Count - 2);

        Instantiate(key, rooms[rand].transform.position, Quaternion.identity);
        Instantiate(boss, rooms[rooms.Count - 1].transform.position, Quaternion.identity);

        lastRoom.door.SetActive(true); 
        lastRoom.DestroyWalls();

        int random = Random.Range(0, 11);

        if (random <= 11)
        {
            GameObject GunType = GunTypes[Random.Range(0, GunTypes.Length)];
            GameObject Guns = Instantiate(GunType, rooms[rooms.Count - 2].transform.position, Quaternion.identity) as GameObject;
            Guns.transform.parent = transform;
            Gunes.Add(Guns);
        }
    }
}