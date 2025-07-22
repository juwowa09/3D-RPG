using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanosRaio : MonoBehaviour
{
    public int hit = 20;
    public float speed = 5f;
    public float proximityThreshold = 0.1f; // Distância para considerar que o alvo foi alcançado

    private GameObject targetEnemy;
    private GameObject boss; // Referência ao Boss, que é único
    GameObject[] enemies;
    

    void Start()
    {
          try
    {
        boss = GameObject.FindGameObjectWithTag("Boss");
    }
    catch
    {
       return;
    }
        // Inicializa o alvo inicial
        SetNextTarget();
    }

    void Update()
    {
        if (targetEnemy != null)
        {
            // Move o objeto atual em direção ao alvo
            Vector3 direction = (targetEnemy.transform.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // Verifica se o objeto alcançou o alvo
            if (Vector3.Distance(transform.position, targetEnemy.transform.position) < proximityThreshold)
            {
                SetNextTarget(); // Define o próximo alvo quando o alvo atual é alcançado
            }
        }
        else
        {
            // Se nenhum alvo estiver definido, tenta encontrar um novo alvo
            SetNextTarget();
        }

       
    }
    

    private void SetNextTarget()
    {
        // Encontra todos os inimigos na cena   
    try
    {
   
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        
    }
    catch
    {
       return;
    }
        
        

        if (enemies.Length > 0)
        {
            float closestDistance = Mathf.Infinity;
            GameObject closestEnemy = null;

            foreach (GameObject enemy in enemies)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy;
                }
            }

            targetEnemy = closestEnemy;
        }
    }


    // Detecta colisões usando OnTriggerEnter
    private void OnTriggerEnter(Collider other)
    {
        // Se o objeto colidido tiver a tag "Enemy"
        if (other.CompareTag("Enemy"))
        {
            // Envia a mensagem "Danos" para o objeto colidido, aplicando 20 de dano
            other.gameObject.SendMessage("Damage", hit, SendMessageOptions.DontRequireReceiver);
        }
    }
}
