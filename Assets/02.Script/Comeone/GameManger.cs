using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManger : MonoBehaviour
{
   public static GameManger Ginstance;
    public bool isGameOver = false;
    void Awake()
    {
        if (Ginstance == null)
            Ginstance = this;
        else if (Ginstance != null)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

   
    void Update()
    {
        
    }
}
