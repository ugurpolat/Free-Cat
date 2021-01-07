using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public GameObject player;
    public float speed;

    float offSetx;
    Material mat;
    PlayerCtrl playerCtrl;
    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Renderer>().material;
        playerCtrl = player.GetComponent<PlayerCtrl>();
    }

    // Update is called once per frame
    void Update()
    {
        //offSetx += 0.005f;

        if (!playerCtrl.isStuck)
        {
            offSetx += Input.GetAxisRaw("Horizontal") * speed;

            if (playerCtrl.leftPressed)
                offSetx += -speed;
            else if (playerCtrl.rightPressed)
                offSetx += speed;            
            mat.SetTextureOffset("_MainTex", new Vector2(offSetx, 0));
        }
        
    }
}
