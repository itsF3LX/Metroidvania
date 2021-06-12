using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthmanager : MonoBehaviour
{   
    public Sprite fullHealth;
    public Sprite halfHealth;
    public Sprite fullTransition;
    public Sprite halfTransition;
    public Sprite noHealth;
    public GameObject firstHeart;
    public GameObject secondHeart;
    public GameObject thirdHeart;
    private Image rend;
    public GameObject[] leben;
    private int welches = 2;

    // Start is called before the first frame update
    void Start()
    {
        leben  = new GameObject[3];
        leben[0] = firstHeart;
        leben[1] = secondHeart;
        leben[2] = thirdHeart;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(){
        if(leben[welches].GetComponent<Image>().sprite == fullHealth){
            leben[welches].GetComponent<Image>().sprite = fullTransition;
            Invoke("fullTran", 0.1f);
        } else if (leben[welches].GetComponent<Image>().sprite == halfHealth){
            leben[welches].GetComponent<Image>().sprite = fullTransition;
            Invoke("halfTran", 0.1f);
            welches -= 1;
        }
    }

    void fullTran(){
        leben[welches].GetComponent<Image>().sprite = halfHealth;
    }

    void halfTran(){
        leben[welches+1].GetComponent<Image>().sprite = noHealth;
        if(welches < 0){
            welches = 2;
            for(int i = 0; i < leben.Length; i++){
                leben[i].GetComponent<Image>().sprite = fullHealth;
            }
        }
    }
}
