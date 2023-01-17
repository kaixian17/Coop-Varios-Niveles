using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class GameController : NetworkBehaviour
{
    
    public int contadorRecogidos = 0;
    public int cantidadDeObjetos = 3;
    private bool cambioEscena = true;
    private bool juegoTerminado = false;
    
    Vector3 posicionRandom;

    public GameObject cuboPrefab;
    private GameObject objetoRecogible;
    private GameObject cubo;

    public GameObject canvas;
    public GameObject btnConexion;
    public GameObject btnReady;

    private void Start()
    {
        NetworkManager.Singleton.OnServerStarted += SpawnearObjetosRecogibles;

        if (cambioEscena)
        {
            SpawnearObjetosRecogibles();
            cambioEscena = false;
        }
        
    }

    private void OnDisable()
    {
        if (NetworkManager.Singleton!= null)
        {
            NetworkManager.Singleton.OnServerStarted -= SpawnearObjetosRecogibles;
        }
    }
    private void Update()
    {
        if (contadorRecogidos >= cantidadDeObjetos && !juegoTerminado)
        {
            CambiarDeEscena();
        }

        if (juegoTerminado && Input.GetKeyDown(KeyCode.Escape))
        {
            Disconnect();
        }

        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Disconnect();
        }

    }

    public void SpawnearObjetosRecogibles()
    {
        
        if (NetworkManager.Singleton.IsHost)
        {
            SpawnearObjetosRecogibleServerRPC();
            Debug.Log("Spawneado:" + NetworkManager.Singleton.LocalClientId);
        }

    }

    [ServerRpc(RequireOwnership = false)]

    public void SpawnearObjetosRecogibleServerRPC()
    {

        for (int i = 0; i < cantidadDeObjetos; i++)
        {
            posicionRandom = new Vector3(Random.Range(-10, 11), 0.5f, Random.Range(-10, 11));
            objetoRecogible = Instantiate(cuboPrefab, posicionRandom, Quaternion.identity);
            objetoRecogible.GetComponent<NetworkObject>().Spawn();
        }
    }

    public void RecogerObjetoRecogible(GameObject c)
    {
        cubo = c;
        RecogerObjetoRecogibleServerRPC();
    }

    [ServerRpc(RequireOwnership = false)]
    public void RecogerObjetoRecogibleServerRPC()
    {
        if (cubo != null)
        {
            contadorRecogidos++;
            cubo.GetComponent<NetworkObject>().Despawn();        
        }         
    }

    public void CambiarDeEscena()
    {
        CambiarDeEscenaServerRPC();
    }

    [ServerRpc(RequireOwnership = false)]
    public void CambiarDeEscenaServerRPC()
    {
        cambioEscena = true;
        if (cantidadDeObjetos == 3)
        {
            NetworkManager.SceneManager.LoadScene("Nivel 02", UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
        else if (cantidadDeObjetos == 5)
        {
            NetworkManager.SceneManager.LoadScene("Nivel 03", UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
        else if (cantidadDeObjetos == 7)
        {
            MostrarPantallaFinalClientRPC();
        }
    }

    [ClientRpc]
    public void MostrarPantallaFinalClientRPC()
    {
        canvas.SetActive(true);
        juegoTerminado = true;
    }


    public void ConnectHost()
    {
        NetworkManager.Singleton.StartHost();
        canvas.SetActive(false);
    }

    public void ConnectClient()
    {
        NetworkManager.Singleton.StartClient();
        canvas.SetActive(false);
    }

    public void Disconnect()
    {
        SceneManager.LoadScene("Menu");
        NetworkManager.Singleton.Shutdown();
    }
}
