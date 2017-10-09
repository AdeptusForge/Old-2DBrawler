using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ComboGaugeManager : MonoBehaviour {

    public static int hitNumber;
    public float resetTime;
    public static float gaugeResetTime;
    public static float currentGaugeTime;
    Text text;

    void Start()
    {
        text = GetComponent<Text>();
        resetTime = 5f;
        gaugeResetTime = resetTime;
        hitNumber = 0;
    }

    void Update()
    {
        if (hitNumber < 0)
        {
            hitNumber = 0;
        }
    text.text = hitNumber.ToString();
    GaugeTimer();
    }

    public static void AddHit()
    {
        //Debug.Log("hit added");
        hitNumber += 1;
        currentGaugeTime = gaugeResetTime;
    }
    private void GaugeTimer()
    {
        if (currentGaugeTime > 0)
        {
            currentGaugeTime -= Time.deltaTime;
        }
        if(currentGaugeTime < 0)
        {
            currentGaugeTime = 0;
        }
        if(currentGaugeTime == 0)
        {
            GaugeBreak();
        }  
    }
    public void GaugeBreak()
    {
        //Debug.Log("Gauge Reset");
        hitNumber = 0;
    }
    
}
