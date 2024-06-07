using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizarPosicaoZ : MonoBehaviour
{
    void Awake()
    {
        this.transform.Translate(new Vector3(0, 0, Random.Range(-0.01f, 0.01f)));
    }
}
