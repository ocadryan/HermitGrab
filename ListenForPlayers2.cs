using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubNubAPI;




public class ListenForPlayers2 : MonoBehaviour
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
    public string myID = "player2";

    //UUID of last publisher
    public string lastSender = "";

    //these variables will display the value from the message
    public float xVal;
    public float yVal;

    // Start is called before the first frame update
    public static PubNub dataServer;
    public string subscribeChannel = "P3";
    public string publishChannel = "P4";

    public float speed;
    public Animator animator;
    public bool hasShell = false;
    public bool InShell = false;

    public Rigidbody2D rb;
    private Vector2 moveVelocity;
    public GameObject Player2;
    Vector2 startpos = new Vector2(-5.56f, -1.24f);
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
            
            ListenForPlayers script1 = Player2.GetComponent<ListenForPlayers>();
            script1.enabled = true;

            ListenForPlayers2 script2 = this.GetComponent<ListenForPlayers2>();
            script2.enabled = false;

        }
        else if (myID == "player2")
        {

            ListenForPlayers script1 = Player2.GetComponent<ListenForPlayers>();
            script1.enabled = true;

            ListenForPlayers2 script2 = this.GetComponent<ListenForPlayers2>();
            script2.enabled = false;

        }
        else
        {

            ListenForPlayers2 script1 = this.GetComponent<ListenForPlayers2>();
            script1.enabled = true;

            ListenForPlayers script2 = Player2.GetComponent<ListenForPlayers>();
            script2.enabled = false;

        }

        rb.MovePosition(startpos);

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

        animator.SetFloat("Horizontal", xVal);
        animator.SetFloat("Vertical", yVal);
        Vector2 moveInput = new Vector2(xVal, yVal);
        moveVelocity = moveInput.normalized * speed;
        if (hasShell == true)
        {
            animator.SetBool("hasShell", true);
            InShell = true;
        }

        if (InShell == true)
        {
            if (hasShell == false)
            {
                animator.SetBool("InShell", false);
            }
            else
            {
                animator.SetBool("InShell", true);
            }

        }

        rb.transform.position = new Vector3(xVal, yVal, 0);

      
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVelocity);
    }
}
