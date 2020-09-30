using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubNubAPI;
using System.Threading.Tasks;

public class PlayerController : MonoBehaviour
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
    public string subscribeChannel = "P2";
    public string publishChannel = "P1";

    public float speed;
    public Animator animator;
    public bool hasShell = false;
    public bool InShell = false;

    public Rigidbody2D rb;
    private Vector2 moveVelocity;
    public GameObject Player2;

    public float xVal;
    public float yVal;
    Vector2 startpos = new Vector2(-5.56f, -1.24f);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player2"))
       {
           if(hasShell == true){
                hasShell = false;
                collision.gameObject.GetComponent<ListenForPlayers>().hasShell = true;
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

        rb = GetComponent<Rigidbody2D>();
        if (myID == "player1")
        {
            PlayerController script1 = this.GetComponent<PlayerController>();
            script1.enabled = true;

            Player2Controller script2 = Player2.GetComponent<Player2Controller>();
            script2.enabled = false;


        } else if (myID == "player2")
        {
            PlayerController script1 = this.GetComponent<PlayerController>();
            script1.enabled = true;

            Player2Controller script2 = Player2.GetComponent<Player2Controller>();
            script2.enabled = false;


        }
        else
        {
            Player2Controller script2 = Player2.GetComponent<Player2Controller>();
            script2.enabled = true;

            PlayerController script1 = this.GetComponent<PlayerController>();
            script1.enabled = false;
        }
        rb.MovePosition(startpos);

    }



    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
        animator.SetFloat("Vertical", Input.GetAxis("Vertical"));
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
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

        Dictionary<string, object> message = new Dictionary<string, object>();
        message.Add("x", this.transform.position.x);
        message.Add("y", this.transform.position.y);
        dataServer.Publish().Channel("P1").Message(message)
           .Async((a, b) => { });

    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
    }

}
