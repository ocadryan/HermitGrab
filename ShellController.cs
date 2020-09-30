using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellController : MonoBehaviour
{
    public Sprite ShellCrab;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            print("Shell obtained");
            Destroy(this.gameObject);

            collision.gameObject.GetComponent<PlayerController>().hasShell = true;
            collision.gameObject.GetComponent<ListenForPlayers2>().hasShell = true;

        }

        if (collision.CompareTag("Player2"))
        {
            print("Shell obtained");
            Destroy(this.gameObject);

            
            collision.gameObject.GetComponent<ListenForPlayers>().hasShell = true;
            collision.gameObject.GetComponent<Player2Controller>().hasShell = true;
        }
    }
}
