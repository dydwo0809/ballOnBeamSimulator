using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicGraph : MonoBehaviour
{
    public GameObject DynamicP;
    public GameObject DynamicI;
    public GameObject DynamicD;

    private float LengthP;
    private float LengthI;
    private float LengthD;

    private float LengthAdjustment = 190f;

    public GameObject PIDButton;

    public GameObject ball;

    private float DynamicGraphDegree;
    public GameObject beam;

    servo servo;

    // Start is called before the first frame update
    void Start()
    {
        servo = PIDButton.GetComponent<servo>();
    }

    // Update is called once per frame
    void Update()
    {
        LengthP = servo.pterm / LengthAdjustment;
        LengthI = servo.iterm / LengthAdjustment;
        LengthD = servo.dterm / LengthAdjustment;

        DynamicP.transform.localScale = new Vector3(Mathf.Abs(LengthP), 0.25f, 10.0f);
        DynamicI.transform.localScale = new Vector3(Mathf.Abs(LengthI), 0.25f, 10.0f);
        DynamicD.transform.localScale = new Vector3(Mathf.Abs(LengthD), 0.25f, 10.0f);

        if (LengthP > 0)
        {
            DynamicP.transform.localPosition = new Vector3(LengthP / 2 + 0.5f, 0.25f, 0f);
        }
        else if (LengthP < 0)
        {
            DynamicP.transform.localPosition = new Vector3(LengthP / 2 - 0.5f, 0.25f, 0f);
        }
        else
        {
            DynamicP.transform.localPosition = new Vector3(LengthP / 2, 0.25f, 0f);
        }
        if (LengthI > 0)
        {
            DynamicI.transform.localPosition = new Vector3(LengthI / 2 + 0.5f, 0f, 0f);
        }
        else if(LengthI < 0)
        {
            DynamicI.transform.localPosition = new Vector3(LengthI / 2 - 0.5f, 0f, 0f);
        }
        else
        {
            DynamicI.transform.localPosition = new Vector3(LengthI / 2, 0f, 0f);
        }
        if (LengthD > 0)
        {
            DynamicD.transform.localPosition = new Vector3(LengthD / 2 + 0.5f, -0.25f, 0f);
        }
        else if(LengthD < 0)
        {
            DynamicD.transform.localPosition = new Vector3(LengthD / 2 - 0.5f, -0.25f, 0f);
        }
        else
        {
            DynamicD.transform.localPosition = new Vector3(LengthD / 2, -0.25f, 0f);
        }

        transform.position = ball.transform.position;
        transform.eulerAngles = beam.transform.eulerAngles;
    }
}
