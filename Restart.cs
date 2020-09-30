using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    // Start is called before the first frame update
 public void restart()
    {
        print("restart!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
