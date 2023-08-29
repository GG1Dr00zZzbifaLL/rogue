using UnityEngine;

public class Wall : MonoBehaviour
{
    public GameObject block;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("block"))
        {
            //спавн блоков чтобы не было проходов в никуда
            Instantiate(block, transform.GetChild(0).position, Quaternion.identity); //
            Instantiate(block, transform.GetChild(1).position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}