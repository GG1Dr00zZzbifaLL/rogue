using UnityEngine;

public class EnemyGhost : Enemy
{
    private float stopTime;
    private Player player;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //������� ������ ������� ��� ����
            Collider2D[] player = Physics2D.OverlapCircleAll(attackPos.position, attackRange, playerMask);
            for (int i = 0; i < player.Length; i++)
            {
                player[i].GetComponent<Player>().ChangeHealth(-damage);
            }
        }
    }
}