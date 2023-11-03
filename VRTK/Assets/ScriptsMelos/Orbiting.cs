using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbiting : MonoBehaviour
{
    [SerializeField] GameObject centerObject;
    [SerializeField] GameObject hexagonPrefab;
    [SerializeField] int numberOfHexagons = 3;
    [SerializeField] float orbitRadius = 3f;
    [SerializeField] float orbitSpeed = 30f; // Velocidad de órbita en grados por segundo
    [SerializeField] float maxRotationChangePerSecond = 10f; // Máximo cambio de rotación por segundo

    private List<GameObject> hexagons = new List<GameObject>();
    private List<Vector3> rotationSpeeds = new List<Vector3>();

    void Start()
    {
        for (int i = 0; i < numberOfHexagons; i++)
        {
            float angle = i * (360f / numberOfHexagons);
            Vector3 hexagonPosition = centerObject.transform.position + Quaternion.Euler(0, angle, 0) * Vector3.forward * orbitRadius;

            GameObject hexagon = Instantiate(hexagonPrefab, hexagonPosition, Quaternion.identity);
            hexagons.Add(hexagon);

            // Inicializar velocidades de rotación aleatorias en los ejes X, Y y Z
            float randomXSpeed = Random.Range(-maxRotationChangePerSecond, maxRotationChangePerSecond);
            float randomYSpeed = Random.Range(-maxRotationChangePerSecond, maxRotationChangePerSecond);
            float randomZSpeed = Random.Range(-maxRotationChangePerSecond, maxRotationChangePerSecond);
            rotationSpeeds.Add(new Vector3(randomXSpeed, randomYSpeed, randomZSpeed));
        }
    }

    void Update()
    {
        for (int i = 0; i < hexagons.Count; i++)
        {
            hexagons[i].transform.RotateAround(centerObject.transform.position, Vector3.up, orbitSpeed * Time.deltaTime);

            // Aplicar cambios en los ejes X, Y y Z al hexágono
            Vector3 eulerAngles = hexagons[i].transform.eulerAngles;
            eulerAngles.x += rotationSpeeds[i].x * Time.deltaTime;
            eulerAngles.y += rotationSpeeds[i].y * Time.deltaTime;
            eulerAngles.z += rotationSpeeds[i].z * Time.deltaTime;
            hexagons[i].transform.eulerAngles = eulerAngles;
        }
    }
}


