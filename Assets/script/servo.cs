using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class servo : MonoBehaviour
{
    public HingeJoint servoHinge;
    private float speed = 100;
    private int PID = 0;
    public float P;
    public float I;
    public float D;

    public float pterm = 0;
    public float dterm = 0;
    public float iterm = 0;

    private float currentDegree;

    public float targetDist;
    public float currentDist;
    private float prevDist;

    private float error_curr;
    private float control;

    public GameObject lowArm;
    public GameObject ball;
    public GameObject sensor;

    public TMP_InputField P_input;
    public TMP_InputField I_input;
    public TMP_InputField D_input;
    public TMP_InputField target_input;

    public TMP_Text currentDistText;
    public TMP_Text ptermText;
    public TMP_Text itermText;
    public TMP_Text dtermText;
    public TMP_Text controlText;


    private void Start()
    {
        JointLimits limits = servoHinge.limits;
        limits.min = -90;
        limits.bounciness = 0;
        limits.bounceMinVelocity = 0;
        limits.max = 90;
        servoHinge.limits = limits;

        P = PlayerPrefs.GetFloat("P");
        I = PlayerPrefs.GetFloat("I");
        D = PlayerPrefs.GetFloat("D");

        targetDist = PlayerPrefs.GetFloat("targetDist");

        currentDistText.color = Color.black;
        ptermText.color = Color.black;
        itermText.color = Color.black;
        dtermText.color = Color.black;
        controlText.color = Color.black;
    }

    private void FixedUpdate()
    {
        if (PID == 1) 
        {
            targetDist = PlayerPrefs.GetFloat("targetDist");

            currentDist = Vector3.Distance(ball.transform.position, sensor.transform.position) - 8.15f;
            currentDist *= 10; //cm -> mm 단위 변환
            error_curr = targetDist - currentDist;


            pterm = error_curr * P;
            dterm = (prevDist - currentDist) * D;
            iterm += error_curr * I;

            control = 1580 + pterm + dterm + iterm; //1580일때 수평

            WriteMicroseconds(control);

            prevDist = currentDist;

            currentDist = Mathf.Round(currentDist * 10) * 0.1f;
            pterm = Mathf.Round(pterm * 10) * 0.1f;
            iterm = Mathf.Round(iterm * 1000) * 0.001f;
            dterm = Mathf.Round(dterm * 1000) * 0.001f;
            control = Mathf.Round(control * 10) * 0.1f;

            currentDistText.text = "" + currentDist;
            ptermText.text = "" + pterm;
            itermText.text = "" + iterm;
            dtermText.text = "" + dterm;
            controlText.text = "" + control;
        }
        else
        {
            pterm = 0;
            iterm = 0;
            dterm = 0;

            JointMotor motor = servoHinge.motor;
            motor.targetVelocity = 0;

            if (Input.GetKey(KeyCode.UpArrow))
            {
                motor.targetVelocity = -speed;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                motor.targetVelocity = speed;
            }
            servoHinge.motor = motor;

            currentDist = Vector3.Distance(ball.transform.position, sensor.transform.position) - 8.15f;
            currentDist *= 10; //cm -> mm 단위 변환
            currentDist = Mathf.Round(currentDist * 10) * 0.1f;
            currentDistText.text = "" + currentDist;

            currentDegree = lowArm.transform.eulerAngles.z;
            if (currentDegree > 180)
            {
                currentDegree -= 360;
            }
            currentDegree = Mathf.Round(currentDegree * 10) * 0.1f;
            controlText.text = "" + currentDegree;
        }
    }
    private void WriteMicroseconds(float duty)
    {
        float degree = 9 * duty / 100 - 135; //500 ~ 2500 범위 -> -90 ~ 90 보정
        Write(degree);
    }
    

    private void Write(float degree)
    {
        if (degree > 90)
        {
            degree = 90;
        }
        if (degree < -90)
        {
            degree = -90;
        }

        JointMotor motor = servoHinge.motor;
        JointLimits limits = servoHinge.limits;
        currentDegree = lowArm.transform.eulerAngles.z;

        if (currentDegree > 180)
        {
            currentDegree -= 360;
        }

        if (currentDegree > degree)
        {
            limits.min = degree;
            limits.max = 90;
            motor.targetVelocity = -speed;
        }
        else
        {
            limits.max = degree;
            limits.min = -90;
            motor.targetVelocity = speed;
        }
        servoHinge.limits = limits;
        servoHinge.motor = motor; 
    }

    public void togglePID()
    {
        PID = 1 - PID;

        speed = 1000f;

        if (PID == 0)
        {
            JointLimits limits = servoHinge.limits;
            limits.min = -90;
            limits.max = 90;
            servoHinge.limits = limits;

            speed = 100;
        }
    }
}
