using System;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public int hitPoints = 1;

    private SpriteRenderer spriteRenderer;

    public ParticleSystem DestroyEffect;

    public static event Action<Brick> OnBrickDestruction;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = BrickManager.Instance.sprites[hitPoints - 1];
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Ball ball = other.gameObject.GetComponent<Ball>();

        ApplyCollisionLogic(ball);
    }

    private void ApplyCollisionLogic(Ball ball)
    {
        hitPoints--;

        if (hitPoints == 0)
        {
            OnBrickDestruction?.Invoke(this);
            SpawnDestroyEffect();
            Destroy(this.gameObject);
        }
        else
        {
            spriteRenderer.sprite = BrickManager.Instance.sprites[hitPoints - 1];
        }
    }

    private void SpawnDestroyEffect()
    {
        Vector3 brickPosition = gameObject.transform.position;
        Vector3 spawnPosition = new Vector3(brickPosition.x, brickPosition.y, brickPosition.z - 0.2f);
        GameObject effect = Instantiate(DestroyEffect.gameObject, spawnPosition, Quaternion.identity);

        ParticleSystem.MainModule mm = effect.GetComponent<ParticleSystem>().main;
        mm.startColor = spriteRenderer.color;

        Destroy(effect, DestroyEffect.main.startLifetime.constant);
    }
}
