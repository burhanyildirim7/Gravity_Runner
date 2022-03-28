using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KarakterPaketiMovement : MonoBehaviour
{
    public float _speed;

    public static KarakterPaketiMovement instance; // Singleton yapisi icin gerekli ornek

    // singleton yapisi burada kuruluyor.
    private void Awake()
    {
        if (instance == null) instance = this;
        //else Destroy(this);
    }


    void FixedUpdate()
    {
        if (GameController.instance.isContinue == true)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * _speed);
        }
        
    }

}
