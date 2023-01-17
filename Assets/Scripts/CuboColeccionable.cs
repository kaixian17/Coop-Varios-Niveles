using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class CuboColeccionable : NetworkBehaviour
{
    public GameObject g;

    private void Start()
    {
        g = GameObject.Find("GameController");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("colisiono");
            g.GetComponent<GameController>().RecogerObjetoRecogible(gameObject);

        }
    }
}
