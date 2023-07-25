using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using System.Threading;
// using System.Timers;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    public float jumpforce;
    public Collider2D coll;
    public LayerMask ground;
    public Text text;

    public Tilemap tilemap;
    public Tile tile;

    public int score;

    private bool temp = false;


    private bool flg = false;



    void Update()
    {
        Movement();
        if (flg)
        {
            rb.rotation += 70;
        }
        if (rb.position.y >= 3000 && !temp)
        {
            jumpforce += 10;
            temp = true;
        }
    }

    void Movement()
    {
        float horizonmentalmove = Input.GetAxis("Horizontal");
        float facedirection = Input.GetAxisRaw("Horizontal") * -1;
        if (horizonmentalmove != 0)
        {
            rb.velocity = new Vector2(horizonmentalmove * speed, rb.velocity.y);
        }
        if (facedirection != 0)
        {
            transform.localScale = new Vector2(facedirection * Mathf.Abs(transform.localScale.x), 1 * Mathf.Abs(transform.localScale.y));
        }
        if (Input.GetButtonDown("Jump") || Input.GetKeyDown("w") || Input.GetKeyDown("up"))
        {
            if (coll.IsTouchingLayers(ground))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpforce);
            }
        }
        
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Base")
        {
            // Destroy(collision.gameObject);
            // collision.gameObject.SetActive(false);
            //Tile collidedTile = collision.collider.GetComponent<Tile>();
            //collidedTile.SetActive(false);
            Vector2 coll_pos = collision.collider.transform.position;
            // tilemap.ClearTile(coll_pos.x, coll_pos.y, 0);
            // Vector3Int coll_pos_3 = new Vector3Int((int)coll_pos.x, (int)coll_pos.y, 0);
            // tilemap.SetTile(coll_pos_3, null);

        }
        // rb.velocity = new Vector2(rb.velocity.x, jumpforce);
        

        
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpforce);


        flg = true;
        // Thread.Sleep(1000);
        System.Timers.Timer timer = new System.Timers.Timer(500);
        timer.Elapsed += new System.Timers.ElapsedEventHandler((object source, System.Timers.ElapsedEventArgs e) => { flg = false; });
        timer.AutoReset = false;
        timer.Enabled = true;

        score += 1;
        text.text = score.ToString();

        // flg = false;

        for (int i = 0; i < 5; i++)
        {
            
            addTile();


        }
    }
    void addTile()
    {
        int current_x = (int)rb.position.x + Random.Range(-7, 7);
        int current_y = (int)rb.position.y + Random.Range(0, 20);
        Vector3Int pos = new Vector3Int(current_x, current_y, 0);
        tilemap.SetTile(pos, tile);
        // tiles.Add(tile);
        // Array.Resize(ref tiles, tiles.Length + 1);
        // tiles[tiles.Length - 1] = tile;

    }
    public void Hurt()
    {
        rb.AddForce(0, 0, 10f);
    }
}
