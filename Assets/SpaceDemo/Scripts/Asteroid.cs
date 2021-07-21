using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceDemo
{
    public class Asteroid : MonoBehaviour
    {
        // Private
        private Vector2 direction;

        // Public
        public float moveSpeed = 1;
        public float rotateSpeed = 1;

        public Asteroid[] spawnAsteroids;
        public int spawnAmount = 5;

        // Properties
        public Vector2 Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        // Methods
        public void Update()
        {
            // Move asteroid
            transform.position += (Vector3)direction * Time.deltaTime;

            // Rotate asteroid
            transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
        }

        public void Randomize()
        {
            moveSpeed = Random.Range(moveSpeed - 0.5f, moveSpeed + 0.5f);
            rotateSpeed = Random.Range(rotateSpeed - 20.5f, rotateSpeed + 20.5f);

            if (Random.Range(0f, 1f) > 0.5) moveSpeed = -moveSpeed;
            if (Random.Range(0f, 1f) > 0.5) rotateSpeed = -rotateSpeed;
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Bullet") == true)
            {
                Destroy(gameObject);

                for (int i = 0; i < spawnAmount; i++)
                {
                    // Select random asteroid
                    Asteroid spawnAsteroid = spawnAsteroids[Random.Range(0, spawnAsteroids.Length)];

                    // Create instance
                    Asteroid spawned = Instantiate(spawnAsteroid, transform.position, Quaternion.identity);

                    // Set random direction
                    spawned.direction = new Vector2(
                        Random.Range(-1, 1),
                        Random.Range(-1, 1));

                    // Randomize movement 
                    spawned.Randomize();
                }
            }
        }
    }
}
