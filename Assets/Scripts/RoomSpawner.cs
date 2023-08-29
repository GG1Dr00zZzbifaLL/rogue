using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public enum Direction
    {
        Top,
        Bottom,
        Left,
        Right,
        None
    }
    public Direction direction;

    private RoomVariants variants;
    private int rand;
    private float waitTime = 3f;
    
    
    private void Start()
    {
        variants = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomVariants>();
        Destroy(gameObject, waitTime);
        Invoke("Spawn", 0.2f);
    }

    private bool spawned = false;
    public void Spawn()
    {
        //случайное появление комнат в зависимости от расположения spawnpoint'a
        if (!spawned)
        {
            if (direction == Direction.Top)
            {
                rand = Random.Range(0, variants.topRooms.Length);
                Instantiate(variants.topRooms[rand], transform.position, variants.topRooms[rand].transform.rotation); 
            }
            else if (direction == Direction.Bottom)
            {
                rand = Random.Range(0, variants.bottomRooms.Length);
                Instantiate(variants.bottomRooms[rand], transform.position, variants.bottomRooms[rand].transform.rotation);
            }
            else if (direction == Direction.Right)
            {
                rand = Random.Range(0, variants.rightRooms.Length);
                Instantiate(variants.rightRooms[rand], transform.position, variants.rightRooms[rand].transform.rotation);
            }
            else if (direction == Direction.Left)
            {
                rand = Random.Range(0, variants.leftRooms.Length);
                Instantiate(variants.leftRooms[rand], transform.position, variants.leftRooms[rand].transform.rotation);
            }
            spawned = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        //комнаты не будут появлятся друг на друге, а предварительно будут уничтожены
        if (other.CompareTag("RoomPoint") && other.GetComponent<RoomSpawner>().spawned)
        {
            Destroy(gameObject);
        }
    }
}