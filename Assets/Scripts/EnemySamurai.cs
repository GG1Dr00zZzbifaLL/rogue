using UnityEngine;

public class EnemySamurai : Enemy
{
    private Player player;

    public void Attack()
    {
        //наносит урон на любом расстоянии
        Collider2D[] player = Physics2D.OverlapCircleAll(attackPos.position, attackRange, playerMask);
        for (int i = 0; i < player.Length; i++)
        {
            player[i].GetComponent<Player>().ChangeHealth(-damage);
        }
    }
}