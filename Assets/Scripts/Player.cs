using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Properties")]
    public int health = 100;
    [Tooltip("Player movement speed value.")]
    public float moveSpeed = 10f;
    [Tooltip("Padding for Player object's half don't go outside of MainCamera.")]
    public Vector2 padding = new Vector2(0.3f, 1f);

    [Header("Player Cannon")]
    [Tooltip("Time in seconds that Ship waits until shoot again.")]
    public float fireRate = 0.1f;

    [Header("Audio Source")]
    public AudioSource audioSource;
    public AudioClip shootSound;
    [Range(0, 1)]
    public float shootSoundVolume = 0.5f;

    public GameObject deathFX;

    // Private Variables
    Coroutine firingCoroutine;
    float xMin;
    float xMax;
    float yMin;
    float yMax;

    // Start is called before the first frame update
    void Start()
    {
        SetupMoveBoundaries();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }

    IEnumerator FireAtWill()
    {
        while (true)
        {
            GameObject laser = ObjectPool.SharedInstance.GetPooledObject("PlayerLaser");
            if (laser != null)
            {
                laser.transform.position = transform.position;
                laser.transform.rotation = Quaternion.identity;
                laser.SetActive(true);
                audioSource.PlayOneShot(shootSound, shootSoundVolume);
            }
            yield return new WaitForSeconds(fireRate);
        }
    }

    private void Fire()
    {
        // Disparo automático
        if (firingCoroutine == null) // Garante que apenas um disparo esteja ativo
        {
            firingCoroutine = StartCoroutine(FireAtWill());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();

        if (damageDealer)
        {
            TakeDamage(damageDealer.GetDamage());

            damageDealer.Hit();
        }

    }

    private void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Load the GameOver Scene
        FindObjectOfType<Level>().LoadGameOver();

        deathFX.transform.position = transform.position;
        deathFX.transform.rotation = Quaternion.identity;
        deathFX.SetActive(true);

        Destroy(gameObject);
    }

    private Vector2 targetPosition;

    private void Move()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Detecta início do toque ou movimento
            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
            {
                // Atualiza a posição alvo com base no toque
                targetPosition = Camera.main.ScreenToWorldPoint(touch.position);
            }
        }

        // Movimenta a nave suavemente para a posição alvo
        Vector2 currentPosition = transform.position;
        Vector2 newPosition = Vector2.MoveTowards(
            currentPosition,
            targetPosition,
            moveSpeed * Time.deltaTime // Velocidade constante definida por moveSpeed
        );

        // Restringe o movimento aos limites da tela
        float clampedX = Mathf.Clamp(newPosition.x, xMin, xMax);
        float clampedY = Mathf.Clamp(newPosition.y, yMin, yMax);

        // Atualiza a posição da nave
        transform.position = new Vector2(clampedX, clampedY);
    }


    private void SetupMoveBoundaries()
    {
        // Get the Scene's mainCamera object
        Camera gameCamera = Camera.main;

        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding.x;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding.x;

        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding.y;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding.y;
    }
}
