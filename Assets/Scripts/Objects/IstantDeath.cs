using UnityEngine;

public class IstantDeath : MonoBehaviour
{
    public bool isLiquid = false;
    public float timerLiquid;
    //private float elapsedTime;
    //private void Start()
    //{
    //    elapsedTime = timerLiquid;
    //}
    private void Update()
    {
        if (isLiquid == true)
        {
            timerLiquid -= Time.deltaTime;
            if (timerLiquid <= 0)
            {
                Destroy(gameObject);
            }
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>())
        {
            collision.GetComponent<Player>().Death();
        }
    }
}
