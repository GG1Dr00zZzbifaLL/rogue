using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject effect;
    public GameObject FloatingDamage;
    public GameObject deathSound;
    public float speed;
    public float startStopTime;
    public float attackRange;
    public int health;
    public int damage;      
    public Transform attackPos;
    public LayerMask playerMask;
    public LayerMask destractBlockMask;

    private float stopTime;
    private Player player;
    private AddRoom room;
    private bool stopped;
    private DestractionBlock destractBlock;

    [HideInInspector] public bool playerNotInRoom;
   
    private void Start()
    {
        player = FindObjectOfType<Player>();
        room = GetComponentInParent<AddRoom>();
    }

    private void Update()
    {
        //�������� ������
        if (!playerNotInRoom)
        {
            if (stopTime <= 0)
            {
                stopped = false;
            }
            else
            {
                stopped = true;
                stopTime -= Time.deltaTime;
            }
        }
        else
        {
            stopped = true;
        }

        if (!stopped)
        {
            //�������� � ������� ������
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
        
        //������� ������� �����
        if (player.transform.position.x > transform.position.x) 
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        //������ ����� ��� �������� ������ 0
        if (health <= 0)
        {
            Instantiate(deathSound, transform.position, Quaternion.identity);
            Instantiate(effect, transform.position, Quaternion.identity);
            Destroy(gameObject);
            room.enemies.Remove(gameObject);
        }
    }
 
    public void TakeDamage(int damage)
    {
        //��������� �����
        stopTime = startStopTime;
        Instantiate(effect, transform.position, Quaternion.identity);
        health -= damage;
        Vector2 damagePos = new Vector2(transform.position.x, transform.position.y + 2f);
        Instantiate(FloatingDamage, damagePos, Quaternion.identity);
        FloatingDamage.GetComponentInChildren<FloatingDamage>().damage = damage;
    }

    public void OnAttack()
    {
        //����� �� ������ � ������� ��������
        Collider2D[] player = Physics2D.OverlapCircleAll(attackPos.position, attackRange, playerMask);
        for (int i = 0; i < player.Length; i++)
        {
            player[i].GetComponent<Player>().ChangeHealth(-damage);
        }
    }

    public void OnAttackBlock()
    {
        //����� �� ����� � ������� ��������
        Collider2D[] destractBlock = Physics2D.OverlapCircleAll(attackPos.position, attackRange, destractBlockMask);
        for (int i = 0; i < destractBlock.Length; i++)
        {
            destractBlock[i].GetComponent<DestractionBlock>().TakeDamage(-damage);
        }
    }
}