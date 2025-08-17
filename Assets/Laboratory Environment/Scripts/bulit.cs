using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class bulit : MonoBehaviour
{
    private bool rotat=false,move=false;
    private int move_dirction = 1;
    [SerializeField] private float rotation_speed,force,move_speed;
    [SerializeField] private TextMeshProUGUI angle,hight,vertical_dis;
    [SerializeField]private Transform tip,origin,gunpoint;
    private float the_dgree;
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform floor_point;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        origin.position = transform.position;
        hight.text=(transform.position.y- floor_point.position.y).ToString();
        if (move&& (transform.position.y - floor_point.position.y)>0)
        {
           
            transform.Translate(Vector2.down * Time.deltaTime * move_speed*move_dirction,Space.World);
        }
        if (rotat)
        {
            if (the_dgree<90)
            {
                Vector2 dif=transform.position-tip.position;
                the_dgree = transform.eulerAngles.x + 16;
                angle.text = the_dgree.ToString();
                transform.Rotate(Vector3.right, rotation_speed * Time.deltaTime);
                Debug.Log(the_dgree);
           
            }
            else
            {
                transform.rotation = quaternion.identity;
                the_dgree=0;
                angle.text = the_dgree.ToString();
                Debug.Log("above");
            }
        }
    }
    public void move_it(int i)
    {
        if (i > 0)
        {
            move_dirction = 1;
        }
        else
        {
            move_dirction = -1;
        }
        if (move)
        {
            move = false;
        }
        else
        {
         move = true;
        }
    }
    public void stop_moving()
    {
        move = false;
    }
    public void start_rotat()
    {
        rotat = true;
    }
    public void end_rotat()
    {
        rotat = false;
    }
    public void lunch_ball()
    {
       GameObject new_bullit= Instantiate(prefab, gunpoint.position, Quaternion.identity);
        Vector3 dif = transform.position - tip.position;
        new_bullit.GetComponent<Rigidbody>().AddForce(-dif.normalized*force, ForceMode.Impulse);
        new_bullit.GetComponent<projectile>().set_texts(vertical_dis, floor_point.position);
    }
}
