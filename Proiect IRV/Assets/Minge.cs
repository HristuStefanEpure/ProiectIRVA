using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Minge : MonoBehaviour
{

    public Rigidbody rb;
    public Slider direction;
    public Slider height;
    public Slider power;

    public Text goal;

    float timer;

    public GameObject player;
    public GameObject goalkeeper;

    int state; /*   0-directie
                *   1-inaltime
                *   2-putere
                *   3-lovire
                */

    float ball_direction;
    float ball_height;
    float ball_power;

    // Start is called before the first frame update

    void InitValues()
    {
        timer = 0;
        rb = GetComponent<Rigidbody>();
        state = 0;
        direction.maxValue = 10;
        height.maxValue = 10;
        power.maxValue = 10;
    }
    void Start()
    {

        InitValues();
    }

    

    // Update is called once per frame
    void Update()
    {
        float timeslider = Time.time;

        switch (state)
        {
            case 0:
                direction.value = Mathf.PingPong(timeslider * direction.maxValue, direction.maxValue);
                break;
            case 1:
                height.value = Mathf.PingPong(timeslider * height.maxValue, height.maxValue);
                break;
            case 2:
                power.value = Mathf.PingPong(timeslider * power.maxValue, power.maxValue);
                break;
            case 3:
                if(timer > 0.7)
                {
                    Launch();
                    if(direction.value > direction.maxValue / 2)
                    {
                        goalkeeper.transform.localScale = new Vector3(-goalkeeper.transform.localScale.x, goalkeeper.transform.localScale.y, goalkeeper.transform.localScale.z);
                    }
                    goalkeeper.GetComponent<Animator>().SetTrigger("Jump");
                    timer = 0;
                    state = 4;
                }
                timer += Time.deltaTime;
                break;
            case 4:
                if(timer > 3)
                {
                    Application.LoadLevel(0);
                }
                timer += Time.deltaTime;
                break;

        }


        if (Input.GetKeyDown("space"))
        {
            switch (state)
            {
                case 0:
                    ball_direction = direction.value;
                    state = 1;
                    break;
                case 1:
                    ball_height = height.value;
                    state = 2;
                    break;
                case 2:
                    ball_power = power.value;
                    state = 3;
                    //Launch();
                    player.GetComponent<Animator>().SetTrigger("Kick");
                    break;
                case 3:
                    break;
                case 4:
                    //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    Application.LoadLevel(0);
                    break;
            }
        }

    }

    void Launch()
    {
        rb.AddForce(-direction.value + direction.maxValue / 2, height.value, -power.value, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("GOOOOOL");
        goal.gameObject.SetActive(true);
        timer = 0;
    }
}
