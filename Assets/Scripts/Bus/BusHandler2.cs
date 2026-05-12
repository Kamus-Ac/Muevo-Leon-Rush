using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class BusHandler2 : VehicleMovement
{

    [SerializeField]
    private Transform _model;


    float MinumumCombo = 1f;
    private float _input;
    const float ROTATIONFORCE = 5.0f;


    void OnEnable()
    {
        Referee.Mademistake += Break;
    }

    private void Start()
    {   
        RB = GetComponent<Rigidbody>();

    }



    private void Update()
    {
        if(IsExploded) return;

        _model.transform.rotation = Quaternion.Euler(0, RB.linearVelocity.x * ROTATIONFORCE, 0); //se rota el modelo del coche en funcion de la velocidad lateral, para que se incline al girar
        //_input = InputHandler.Instance.TurnInput; 
    }

    private void FixedUpdate()
    {

        if (IsExploded)
        {
            RB.linearDamping = RB.linearVelocity.z * 0.1f;
            RB.linearDamping = Mathf.Clamp(RB.linearDamping, 1.5f, 10f);
            //slowly move car to the center of the lane as it explodes, so it doesn't fly off to the sides as much
            RB.MovePosition(Vector3.Lerp(transform.position, new Vector3(0, 0, transform.position.z), Time.fixedDeltaTime * 0.5f));
            RB.linearVelocity = new Vector3(0, 0, 0);
        } 

        //if (!GameManager.Instance.IsGamePlaying()) return; //si el jugador no presiona la barra espaciadora, nada se mueve

        Accelerate();

        //RB.linearDamping = 0.2f; //"no esta pisando el acelerador"


        //Steer(_input); 


        if (RB.linearVelocity.z <= 0) //no hay reversa
            RB.linearVelocity = Vector3.zero; //aqui hay una idea para darle drift al retroceder o avanzar, mientras se queda que los tres ejes son 0.

    }




    void OnDestroy()
    {
        Referee.Mademistake -= Break;
    }


    protected override void Accelerate()
    {
        base.Accelerate();
        /* if (GameManager.Instance.combo == 0)
        {
            RB.AddForce(transform.forward * (AccelerationForce * MinumumCombo / 10));
        } */
        RB.AddForce(transform.forward * AccelerationForce);
    }

    protected override void Break(int button)
    {
        base.Break(button);
        if (GameManager.Instance.combo == 0)
        {
            RB.AddForce(transform.forward * -BreakForce);
            return;
        }
    }

    public Vector3 GetVelocity()
    {
        return RB.linearVelocity;
    }

    public float GetSpeedPercentage()
    {
       float busMaxSpeedPercentage = RB.linearVelocity.z / MaxForwardVelocity;
       return busMaxSpeedPercentage;
    }


   /*  private void OnCollisionEnter(Collision collision)
    {
        if(!IsExploded && GameManager.Instance.IsGamePlaying())
            GameManager.Instance.GameOver();
            IsExploded = true;
    } */




}
