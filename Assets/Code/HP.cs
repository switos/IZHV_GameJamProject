using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP : MonoBehaviour
{


    // Start is called before the first frame update
    [Header("Attribute")]
    [SerializeField] private int hits = 2;

    private bool isDestroyed = false;

    public void TakeDamage(int dmg)
    {
        hits -= dmg;

        if (hits <= 0 && !isDestroyed)
        {
            isDestroyed = true;
            LevelManager.main.IncreaseCur(50);
            EnemySpawner.onEnemyKill.Invoke();
            Destroy(gameObject);
        }
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
