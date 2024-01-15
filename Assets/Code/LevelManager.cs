using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public static LevelManager main;

    public Transform startPoint;
    public Transform[] path;
    public int currency;
    private void Awake()
    {
        main = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        currency = 100;
    }

    public void IncreaseCur(int amount)
    {
        currency += amount;
    }

    public bool SpendCurrency(int amount)
    {
        if (amount <= currency)
        {
            currency -= amount;
            return true;
        } else
        {
            return false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
