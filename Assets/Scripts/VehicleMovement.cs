using UnityEngine;

public class VehicleMovement : MonoBehaviour
{
    //Referencias
    protected Rigidbody RB;

    //Fuerzas
    protected float AccelerationForce = 3.0f;
    protected float BreakForce = 15.0f;
    protected float SteerForce = 5.0f;
    protected float MaxSteerVelocity = 2.0f;
    protected float MaxForwardVelocity = 15.0f;

    //Variables para el giro
    protected float BaseSteer;
    protected float SpeedToTurn = 10.0f; //velocidad a la que tienes que ir para girar


    //Extras
    protected bool IsExploded;


    protected virtual void Accelerate()
    {

        if (IsExploded) return;

        RB.linearDamping = 0;

        //Stay within the speed limit

        if (RB.linearVelocity.z >= MaxForwardVelocity)
            return;
    }

    protected virtual void Break(int button)
    {
        if (RB.linearVelocity.z <= 0)
            return;
    }

    protected void Steer(float Input)
    {
        //Debug.Log("Steer Input: " + Input);
        if (Mathf.Abs(Input) > 0) //if there is some steering input
        {
            //Si vamos muy despacio, no te deja girar 
            BaseSteer = RB.linearVelocity.z / SpeedToTurn;
            BaseSteer = Mathf.Clamp01(BaseSteer); //limita el movimiento con clamp

            RB.AddForce(transform.right * SteerForce * Input * BaseSteer);

            //normalize x velocity 
            float normalized = RB.linearVelocity.x / MaxSteerVelocity;

            //make sure it doesn't exceed 1 in magnitd
            normalized = Mathf.Clamp(normalized, -1.0f, 1.0f);

            //make sure we stay within the turn speed limit
            RB.linearVelocity = new Vector3(normalized * MaxSteerVelocity, 0, RB.linearVelocity.z);
        }
        else
        {

            //auto center car when not steering, but only on the x axis, so it doesn't affect forward speed
            RB.linearVelocity = Vector3.Lerp(RB.linearVelocity, new Vector3(0, 0, RB.linearVelocity.z), Time.fixedDeltaTime * 3);
        }
    }


}
