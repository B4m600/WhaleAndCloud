using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Threading;

using UnityEngine.Tilemaps;


public struct JoyStickData //�ṹ�塪��ҡ������
{
    public Vector2 dir; //�ƶ�����
    public float radius; //�ƶ��뾶


    public JoyStickData(Vector2 d, float r)
    {
        this.dir = d;
        this.radius = r;
    }
    public Vector2 GetPos()
    {
        return dir * radius;
    }

}
public class JoyStick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler    //�����ӿڣ������̧����ק
{
    public RectTransform bound; //��Ȧ
    public RectTransform center; //��Ȧ
    public float radius; //�ƶ����Ƶİ뾶

    public Rigidbody2D rb;
    public float speed;
    public float jumpforce;
    public Collider2D coll;
    public LayerMask ground;

    public Tilemap tilemap;
    public Tile tile;

    public Text Height;

    private bool super = false;



    void Update()
    {
        Height.text = ((int)rb.position.y).ToString();
        if (rb.position.y >= 2000)
        {
            Vector3Int pos = new Vector3Int((int)rb.position.x, (int)rb.position.y, 0);
            tilemap.SetTile(pos, tile);
        }


    }

    private JoyStickData HandleEventData(PointerEventData eventData)
    {
        Vector2 dir;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(bound, eventData.position, eventData.pressEventCamera, out dir);
        float r = dir.magnitude;
        dir = dir.normalized;
        r = Mathf.Clamp(r, 0, radius);

        return new JoyStickData(dir, r);
    }
    public void OnDrag(PointerEventData eventData)
    {
        var data = HandleEventData(eventData);
        center.localPosition = data.GetPos();
        onJoystickMove(data.dir, data.radius);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        var data = HandleEventData(eventData);
        center.localPosition = data.GetPos();
        onJoystickDown(data.dir, data.radius);

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        var data = HandleEventData(eventData);
        center.localPosition = Vector2.zero; //̧���ص�0����
        onJoystickUp(data.dir, data.radius);
    }
    public virtual void onJoystickDown(Vector2 V, float R)
    {

    }
    public virtual void onJoystickUp(Vector2 V, float R)
    {
        rb.velocity = Vector2.zero;
    }
    public virtual void onJoystickMove(Vector2 V, float R)
    {
        Movement(V);
    }




    void Movement(Vector2 V)
    {
        float var_x = V.x;
        float var_y = V.y;
        float horizonmentalmove = var_x;
        float facedirection = temp(var_x) * -1;
        if (horizonmentalmove != 0)
        {
            rb.velocity = new Vector2(horizonmentalmove * speed, rb.velocity.y);
        }
        if (facedirection != 0)
        {
            rb.transform.localScale = new Vector2(facedirection * Mathf.Abs(rb.transform.localScale.x), 1 * Mathf.Abs(rb.transform.localScale.y));
        }
        if (var_y >= 0.8)
        {
            if (coll.IsTouchingLayers(ground))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpforce);
            }
        }
        else if (var_y <= -0.8 && rb.position.y <= 700 && !super)
        {
            Vector3Int pos = new Vector3Int((int)rb.position.x, (int)rb.position.y, 0);
            tilemap.SetTile(pos, tile);
            super = true;
            System.Timers.Timer timer = new System.Timers.Timer(1000);
            timer.Elapsed += new System.Timers.ElapsedEventHandler((object source, System.Timers.ElapsedEventArgs e) => { super = false; });
            timer.AutoReset = false;
            timer.Enabled = true;

        }
        else if (var_y <= -0.8 && rb.position.y > 700)
        {
            Vector3Int pos = new Vector3Int((int)rb.position.x, (int)rb.position.y, 0);
            tilemap.SetTile(pos, tile);
            super = true;
        }

    }
    int temp(float num)
    {
        if (num < 0) { return -1; }
        else if (num == 0) { return 0; }
        else if (num > 1) { return 1; }
        else { return 0; }
    }

}

