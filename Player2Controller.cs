using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubNubAPI;
using System.Threading.Tasks;

public class Player2Controller : MonoBehaviour
{
    

    //publish and subscribe keys
    public string pubKey = "pub-c-2fa8dff9-af20-4bda-ab8e-b4edfdecb436";
    public string subKey = "sub-c-8c851638-f838-11ea-afa2-4287c4b9a283";


    //your UUID
    public string myID = "player1";

    //UUID of last publisher
    public string lastSender = "";

    // Start is called before the first frame update
    public static PubNub dataServer;
    public string subscribeChannel = "P3";
    public string publishChannel = "P4";

    public float speed;
    public Animator animator;
    public bool hasShell = false;
    public bool InShell = false;

    public Rigidbody2D rb2;
    private Vector2 moveVelocity;
    public GameObject Player1;
    public float xVal;
    public float yVal;
    Vector2 startpos = new Vector2(6.12f, -1.31f);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player1"))
       {
           if(hasShell == true){
                hasShell = false;
                collision.gameObject.GetComponent<ListenForPlayers2>().hasShell = true;
            }
        }
    }
    
    // Start is called before the first frame update


    void Start()
    {
        PNConfiguration pnConfiguration = new PNConfiguration();

        pnConfiguration.SubscribeKey = subKey;
        pnConfiguration.PublishKey = pubKey;
        pnConfiguration.UUID = myID;
        pnConfiguration.Secure = true;

        dataServer = new PubNub(pnConfiguration);

        rb2 = GetComponent<Rigidbody2D>();
        if (myID == "player1")
        {
            Player2Controller script1 = this.GetComponent<Player2Controller>();
            script1.enabled = true;

            PlayerController script2 = Player1.GetComponent<PlayerController>();
            script2.enabled = false;


        }
        else if (myID == "player2")
        {
            Player2Controller script1 = this.GetComponent<Player2Controller>();
            script1.enabled = true;

            PlayerController script2 = Player1.GetComponent<PlayerController>();
            script2.enabled = false;


        }
        else
        {
            PlayerController script2 = Player1.GetComponent<PlayerController>();
            script2.enabled = true;

            Player2Controller script1 = this.GetComponent<Player2Controller>();
            script1.enabled = false;
        }

        rb2.MovePosition(startpos);


    }



    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Horizontal2", Input.GetAxis("Horizontal"));
        animator.SetFloat("Vertical2", Input.GetAxis("Vertical"));
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Debug.Log(moveInput);
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

        Dictionary<string, object> message = new Dictionary<string, object>();
        message.Add("x", this.transform.position.x);
        message.Add("y", this.transform.position.y);
        dataServer.Publish().Channel("P4").Message(message)
           .Async((a, b) => { });

    }

    private void FixedUpdate()
    {
        rb2.MovePosition(rb2.position + moveVelocity * Time.fixedDeltaTime);
    }

}
