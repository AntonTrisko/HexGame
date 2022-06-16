using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtScript : MonoBehaviour
{
    private Transform _target;
    // Start is called before the first frame update
    void Start()
    {
        _target = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (_target)
        {
            transform.LookAt(_target);
        }
    }
}
