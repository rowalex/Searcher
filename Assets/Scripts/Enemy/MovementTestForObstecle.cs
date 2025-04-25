using UnityEngine;

public class MovementTestForObstecle : MonoBehaviour
{
    [SerializeField] private Vector3 movementVector;
    [SerializeField] private float speed;
    [SerializeField] private Vector3 rotation;
    private float timer = 0 ;
    [SerializeField] private float time;
    private float index = 1;


    private void Start()
    {
        timer = 0;
        index = 1;
        time = 5;

        Debug.Log("kururing start timer = " + time);
    }

    public void Update()
    {
        timer += Time.deltaTime;

        if (timer > time)
        {
            timer = 0;
            index *= -1;
        }

        transform.position = transform.position + movementVector * index * speed * Time.deltaTime;

        transform.Rotate(rotation * Time.deltaTime);

        Debug.Log("kururing " + timer +" " +  timer);
    }
}
