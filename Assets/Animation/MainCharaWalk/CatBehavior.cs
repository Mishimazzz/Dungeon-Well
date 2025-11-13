using UnityEngine;

public class AutoWalk : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float moveTime = 2f;
    public float waitTime = 1f;
    private float timer;
    private int dir = -1;  // 初始向左
    private Animator anim;
    private SpriteRenderer sr;

    void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        timer = moveTime;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer > 0)
        {
            transform.Translate(Vector3.right * dir * moveSpeed * Time.deltaTime);
            if (anim) anim.SetBool("isWalking", true);
            if (sr) sr.flipX = (dir > 0); // 右走时翻转
        }
        else
        {
            if (anim) anim.SetBool("isWalking", false);

            if (timer < -waitTime)
            {
                dir *= -1; // 反向
                timer = moveTime;
            }
        }
    }
}
