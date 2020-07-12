using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToPlayShow : MonoBehaviour
{
    public List<GameObject> slides;
    private int index = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        slides[0].SetActive(true);
        index = 0;
    }

    public void next()
    {
        slides[index].SetActive(false);
        
        slides[++index].SetActive(true);
        if(index == slides.Count-1)
        {
            this.gameObject.SetActive(false);
        }
    }
}
