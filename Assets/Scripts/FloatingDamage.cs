using UnityEngine;

public class FloatingDamage : MonoBehaviour
{
    private TextMesh textMesh;

    [HideInInspector] public float damage;

    private void Start()
    {
        //��� ������ ���������� ���� ��������� ������� 
        textMesh = GetComponent<TextMesh>();
        textMesh.text = "-" + damage;
    }

    public void OnAnimationOver()
    {
        Destroy(gameObject);
    }
}