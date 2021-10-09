using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floater : MonoBehaviour
{

    public float amplitude = 0.5f;
    public float frequency = 1f;

    Vector3 tempPos;
    Vector3 posOffset;

    // Start is called before the first frame update
    void Start()
    {
        posOffset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

        transform.position = tempPos;
    }
}
