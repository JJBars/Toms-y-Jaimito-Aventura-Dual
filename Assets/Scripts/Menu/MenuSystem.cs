using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Jugar()
    {
        SceneManager.LoadScene("Game");
    }

    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
