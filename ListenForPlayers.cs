using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubNubAPI;




public class ListenForPlayers : MonoBehaviour
{

    /*
    This example listens for data and moves the object to the position corresponding 
    with the X, Y of the last click.

    It is reading from a message structure
    {
      x:
      y:
    }

    */

    //publish and subscribe keys
    public string pubKey = "pub-c-2fa8dff9-af20-4bda-ab8e-b4edfdecb436";
    public string subKey = "sub-c-8c851638-f838-11ea-afa2-4287c4b9a283";

    //your UUID
    public string myID;

    //UUID of last publisher
    public string lastSender = "";

    //these variables will display the value from the message
    public float xVal;
    public float yVal;

    // Start is called before the first frame update
    public static PubNub dataServer;
    public string subscribeChannel = "P2";
    public string publishChannel = "P1";

    public float speed;
    public Animator animator;
    public bool hasShell = false;
    public bool InShell = false;

    public Rigidbody2D rb2;
    private Vector2 moveVelocity;
    public GameObject Player1;
    Vector2 startpos = new Vector2(6.12f, -1.31f);
    void Start()
    {
        PNConfiguration pnConfiguration = new PNConfiguration();

        pnConfiguration.SubscribeKey = subKey;
        pnConfiguration.PublishKey = pubKey;
        pnConfiguration.UUID = myID;
        pnConfiguration.Secure = true;

        dataServer = new PubNub(pnConfiguration);

        if (myID == "player1")
        {
            ListenForPlayers2 script1 = Player1.GetComponent<ListenForPlayers2>();
            script1.enabled = true;

            ListenForPlayers script2 = this.GetComponent<ListenForPlayers>();
            script2.enabled = false;

        }
        else if (myID == "player2")
        {
            ListenForPlayers2 script1 = Player1.GetComponent<ListenForPlayers2>();
            script1.enabled = true;

            ListenForPlayers script2 = this.GetComponent<ListenForPlayers>();
            script2.enabled = false;

        }
        else
        {

            ListenForPlayers script2 = this.GetComponent<ListenForPlayers>();
            script2.enabled = true;

            ListenForPlayers2 script1 = Player1.GetComponent<ListenForPlayers2>();
            script1.enabled = false;

        }
        rb2.MovePosition(startpos);

        dataServer.Subscribe()
          .Channels(new List<string>() { subscribeChannel })
          .Execute();

               dataServer.SubscribeCallback += (sender, e) =>

        {
            SubscribeEventEventArgs inMessage = e as SubscribeEventEventArgs;
            if (inMessage.MessageResult != null)    //error check to insure the message has contents
            {
                //save the UUID of the last sender
                lastSender = inMessage.MessageResult.IssuingClientId;
                //save the message into a Dictionary
                Dictionary<string, object> msg = inMessage.MessageResult.Payload as Dictionary<string, object>;
                //retrieve and convert the value you need using the key name 
                /*
                convert to integer -  (int)msg["keyName"]
                convert to float -  (float)msg["keyName"]
                convert to string -  (string)msg["keyName"]

                */
                xVal = (float) (double)msg["x"];
                yVal = (float) (double)msg["y"];

                Debug.Log($"x:{msg["x"]} y:{msg["y"]}");
            

            }

        }; 

    }

    // Update is called once per frame
    void Update()
    {

        animator.SetFloat("Horizontal2", xVal);
        animator.SetFloat("Vertical2", yVal);
        Vector2 moveInput = new Vector2(xVal, yVal);
        moveVelocity = moveInput.normalized * speed;
        if (hasShell == true)
        {
            animator.SetBool("hasShell2", true);
            InShell = true;
        }

        if (InShell == true)
        {
            if (hasShell == false)
            {
                animator.SetBool("InShell2", false);
            }
            else
            {
                animator.SetBool("InShell2", true);
            }

        }

        rb2.transform.position = new Vector3(xVal, yVal, 0);

      
    }

    private void FixedUpdate()
    {
        rb2.MovePosition(rb2.position + moveVelocity);
    }
}
