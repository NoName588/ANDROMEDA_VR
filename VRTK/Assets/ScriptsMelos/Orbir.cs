using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbir : MonoBehaviour
{
    public GameObject centerObject; // Objeto alrededor del cual orbitarán los objetos
    public int numberOfOrbiters = 10;
    public float orbitSpeed = 50f;
    public float attractionForce = 5f;

    private List<Mover3D> movers = new List<Mover3D>();
    private Attractor3D attractor;

    // Start is called before the first frame update
    void Start()
    {
        attractor = new Attractor3D(centerObject);

        for (int i = 0; i < numberOfOrbiters; i++)
        {
            Vector3 randomLocation = new Vector3(Random.Range(-7f, 7f), Random.Range(-7f, 7f), Random.Range(-7f, 7f));
            Vector3 randomVelocity = new Vector3(Random.Range(0f, 5f), Random.Range(0f, 5f), Random.Range(0f, 5f));
            Mover3D mover = new Mover3D(Random.Range(0.2f, 1f), randomVelocity, randomLocation);
            movers.Add(mover);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (Mover3D mover in movers)
        {
            Vector3 force = attractor.Attract(mover.body);
            mover.ApplyForce(force);
            mover.CalculatePosition();
        }
    }
}

public class Attractor3D
{
    private float radius;
    private float mass;
    private float G;
    private GameObject attractor;

    public Attractor3D(GameObject centerObject)
    {
        attractor = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Object.Destroy(attractor.GetComponent<Collider>());
        Renderer renderer = attractor.GetComponent<Renderer>();
        attractor.transform.position = centerObject.transform.position;

        radius = 2;
        attractor.transform.localScale = 2 * radius * Vector3.one;

        mass = 1; // Adjust the mass as needed
        attractor.GetComponent<Rigidbody>().mass = mass;
        attractor.GetComponent<Rigidbody>().useGravity = false;
        attractor.GetComponent<Rigidbody>().isKinematic = true;

        renderer.material = new Material(Shader.Find("Diffuse"));
        renderer.material.color = Color.red;

        G = 9.8f; // Adjust the gravitational constant as needed
    }

    public Vector3 Attract(Rigidbody m)
    {
        Vector3 force = attractor.transform.position - m.position;
        float distance = force.magnitude;
        distance = Mathf.Clamp(distance, 5f, 25f);
        force.Normalize();
        float strength = (G * attractor.GetComponent<Rigidbody>().mass * m.mass) / (distance * distance);
        force *= strength;
        return force;
    }
}

public class Mover3D
{
    public Rigidbody body;
    private Transform transform;
    private GameObject mover;
    private Vector3 maximumPos;

    public Mover3D(float randomMass, Vector3 initialVelocity, Vector3 initialPosition)
    {
        mover = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Object.Destroy(mover.GetComponent<Collider>());
        transform = mover.transform;
        mover.AddComponent<Rigidbody>();
        body = mover.GetComponent<Rigidbody>();
        body.useGravity = false;

        Renderer renderer = mover.GetComponent<Renderer>();
        renderer.material = new Material(Shader.Find("Diffuse"));
        mover.transform.localScale = new Vector3(randomMass, randomMass, randomMass);

        body.mass = 1;
        body.position = initialPosition;
        body.velocity = initialVelocity;
        FindWindowLimits();
    }

    public void ApplyForce(Vector3 force)
    {
        body.AddForce(force, ForceMode.Force);
    }

    public void CalculatePosition()
    {
        CheckEdges();
    }

    private void CheckEdges()
    {
        Vector3 velocity = body.velocity;
        if (transform.position.x > maximumPos.x || transform.position.x < -maximumPos.x)
        {
            velocity.x *= -1 * Time.deltaTime;
        }
        if (transform.position.y > maximumPos.y || transform.position.y < -maximumPos.y)
        {
            velocity.y *= -1 * Time.deltaTime;
        }
        if (transform.position.z > maximumPos.z || transform.position.z < -maximumPos.z)
        {
            velocity.z *= -1 * Time.deltaTime;
        }
        body.velocity = velocity;
    }

    private void FindWindowLimits()
    {
        Camera.main.orthographic = false;
        Camera.main.transform.position = new Vector3(0, 0, -10);
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }
}

