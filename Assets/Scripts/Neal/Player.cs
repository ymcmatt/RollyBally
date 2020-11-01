using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Player : MonoBehaviour
{
    //Game controller
    public MessageHandler MH;
    public SinglePlayerUI ui;
    public bool inGame;

    //player status
    public int playerIndex;
    float toggleTimer;
    public bool isReady;

    //camera movement
    public CinemachineFreeLook vcam;
    public Transform Cam;
    public float TurnSmoothTime = 0.2f;
    int Turning;
    float TurningTimer;

    //movement
    public float MaxSpeed = 5f;
    public float BaseAccel = 2f;
    public Vector3 SpawnLocation;
    Vector3 CenterPoint = new Vector3(0, 12, 0);
    public Vector3[] PotionLocation;
    Rigidbody RB;
    Vector3 Force;

    //game status
    public int MaxHealth = 3;
    public bool isPredator;
    public int health;
    public float PredatorDuration = 15f;
    float PredatorTimer;
    public bool isInvicible;
    public float InvicibleDuration = 1f;
    float InvicibleTimer;

    //appearance
    public GameObject prey;
    public GameObject predator;
    public GameObject magicPotion;

    //ground check variables
    public float groundDistance = 1f;
    public LayerMask groundLayer;

    //Audio
    public AudioSource RollingPrey;
    public AudioSource RollingPredator;
    public AudioSource Collision;
    public AudioSource Damage;

    //public AudioSource Death;
    public AudioSource Transition;
    public AudioSource PotionCollected;
    public AudioSource PotionSpawn;
    public AudioSource Jump;

    bool preHitGround;

    // Start is called before the first frame update
    void Start()
    {
        RB = gameObject.GetComponent<Rigidbody>();
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (inGame) {
            bool isGrounded = Physics.CheckSphere(transform.position, groundDistance, groundLayer);

            //check instruction change
            if (MH.hasHunter) {
                if (isPredator) {
                    ui.UpdateInstruction(2);
                } else {
                    ui.UpdateInstruction(3);
                }
            } else {
                ui.UpdateInstruction(1);
            }

            //check invicible
            if (InvicibleTimer > 0) {
                InvicibleTimer -= Time.deltaTime;
                if (InvicibleTimer <= 0) {
                    isInvicible = false;
                }
            }

            //check predator timer
            if (PredatorTimer > 0) {
                ui.UpdateCountDown(PredatorTimer.ToString("0.#"));
                PredatorTimer -= Time.deltaTime;
                if (PredatorTimer <= 0) {
                    //change status
                    isPredator = false;
                    prey.SetActive(true);
                    predator.SetActive(false);
                    Transition.Play();

                    //respawn potion
                    int randomInt = (int)Random.Range(0f,4.99f);
                    Vector3 position = PotionLocation[randomInt];
                    Instantiate(magicPotion, position, Quaternion.identity);
                    PotionSpawn.Play();

                    MH.hasHunter = false;
                }
            } else {
                ui.UpdateCountDown("");
            }

            //check whether fall from the plane
            if (transform.position.y <= -10)
            {
                ui.DecreaseHealth();
                health--;
                if (health == 0) {
                    MH.inGamePlayer--;
                    MH.Death.Play();
                    gameObject.SetActive(false);
                } else {
                    transform.position = SpawnLocation;
                    RB.velocity = new Vector3(0, 0, 0);
                    RB.angularVelocity = new Vector3(0, 0, 0);
                    transform.LookAt(CenterPoint);
                    Damage.Play();
                }
            }

            //toggle timer
            toggleTimer -= Time.deltaTime;

            //camera rotation
            if (Turning != 0) {
                float curAngle = Time.deltaTime / TurnSmoothTime * 90f * Turning;
                vcam.m_XAxis.Value += curAngle;
                TurningTimer -= Time.deltaTime;
                if (TurningTimer < 0) {
                    TurningTimer = 0;
                    Turning = 0;
                }
            }

            if (isGrounded) {
                RB.AddForce(Force * BaseAccel);
                if (!preHitGround) {
                    if (isPredator) {
                        if (!RollingPredator.isPlaying) {
                            RollingPredator.Play();
                        }
                    } else {
                        if (!RollingPrey.isPlaying) {
                            RollingPrey.Play();
                        }
                    }
                } 
            } else {
                RB.AddForce(Force);
            } 

            preHitGround = isGrounded;
        }    
    }

    public void AngleInput(Vector2 gAngle) {
        // clamp force and remap force magnitude
        float f_x = Mathf.Sin(Mathf.Clamp(gAngle.x * Mathf.Deg2Rad, -90f, 90f));
        float f_z = Mathf.Sin(Mathf.Clamp(gAngle.y * Mathf.Deg2Rad, -90f, 90f));

        // calculate force angle
        float ForceAngle = Mathf.Atan2(f_x, f_z) * Mathf.Rad2Deg + Cam.eulerAngles.y;

        // limit force magnitude
        float ForceMagnitude = Mathf.Clamp(new Vector3(f_x, 0f, f_z).magnitude, 0, 1);

        // get final force
        Force = Quaternion.Euler(0f, ForceAngle, 0f) * Vector3.forward * ForceMagnitude * RB.mass;
    }

    public void TurnLeft() {
        if (Turning == 0) {
            Turning = -1;
            TurningTimer = TurnSmoothTime;
        }
    }

    public void TurnRight() {
        if (Turning == 0) {
            Turning = 1;
            TurningTimer = TurnSmoothTime;
        }   
    }

    public bool ToggleReady() {
        if (toggleTimer <= 0) {
            isReady = !isReady;
            toggleTimer = 1f;
        }
        return isReady;
    }

    public void Reset() {
        transform.position = SpawnLocation;
        RB.velocity = new Vector3(0, 0, 0);
        RB.angularVelocity = new Vector3(0, 0, 0);
        transform.LookAt(CenterPoint);
        toggleTimer = 0f;
        PredatorTimer = 0f;
        InvicibleTimer = 0f;
        preHitGround = true;
        inGame = false;
        isReady = false; 
        isPredator = false;
        isInvicible = false;   
        health = MaxHealth;
        prey.SetActive(true);
        predator.SetActive(false);
        ui.Reset();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetComponent<Player>().isPredator && !isInvicible) {
                isInvicible = true;
                InvicibleTimer = InvicibleDuration;
                ui.DecreaseHealth();
                health--;
                if (health == 0) {
                    MH.inGamePlayer--;
                    MH.Death.Play();
                    gameObject.SetActive(false);
                } else {
                    Damage.Play();
                }
            } else {
                Collision.Play();
            }
        } 

        if (collision.gameObject.tag == "jumpPad")
        {
            RB.AddForce(0f, 20f, 0f, ForceMode.Impulse);
            Jump.Play();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "potion") {
            PotionCollected.Play();
            Transition.Play();
            Destroy(collision.gameObject);
            isPredator = true;
            PredatorTimer = PredatorDuration;
            prey.SetActive(false);
            predator.SetActive(true);
            MH.hasHunter = true;
        }      
    }
}
