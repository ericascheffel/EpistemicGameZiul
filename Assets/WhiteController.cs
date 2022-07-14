using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteController : MonoBehaviour
{
    //A linha baixo e necessaria para uso do Asset Joystick Pack
    public FixedJoystick moveJoystick; 
    [SerializeField] float speed;
    private float moveH, moveV;

    SpriteRenderer spriteRenderer;
    //O codigo abaixo vai fazer o objeto se mover com forca da fisica e nao so por posicao
    Rigidbody2D rigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        //E necessario iniciar o GetComponent do que estamos usando
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movePlayer();
    }

    void movePlayer()
    {
        //moveH = Input.GetAxis("Horizontal");
        //moveV = Input.GetAxis("Vertical");
        moveH = moveJoystick.Horizontal;
        moveV = moveJoystick.Vertical;
        Vector3 direction = new Vector3(moveH, 0, moveV);
        rigidbody2D.velocity = new Vector3(moveH * speed, moveV * speed);
       
        if( direction != Vector3.zero)
        {
            transform.LookAt(transform.position + direction);
        }
    }
        

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //E necessario verificar o que colidiu com o nosso objeto
        //Para isso, coloque uma Tag no sprite. A tag colocada no quadrado rosa foi BlocoRosaParado
        if (collision.gameObject.CompareTag("BlocoRosaParado"))
        {
            spriteRenderer.color = new Color32(255, 12, 137, 255);            
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        //E necessario verificar o que colidiu com o nosso objeto
        //Para isso, coloque uma Tag no sprite. A tag colocada no quadrado rosa foi BlocoRosaParado
        if (collision.gameObject.CompareTag("BlocoRosaParado"))
        {
            spriteRenderer.color = new Color32(255, 255, 255, 255);
        }
    }
}
