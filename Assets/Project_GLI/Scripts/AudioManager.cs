using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource sfxSource;   // For shots, explosions
    public AudioSource musicSource; // For background music

    [Header("Clips")]
    public AudioClip shootSound;
    public AudioClip reloadSound;
    public AudioClip shotBarrierSound;
    public AudioClip enemyDeathSound;
    public AudioClip explosionSound;
    public AudioClip baseHitSound;

    void Awake()
    {
        Instance = this;
    }

    public void PlayShoot() => sfxSource.PlayOneShot(shootSound);
    public void PlayReload() => sfxSource.PlayOneShot(reloadSound);
    public void PlayShotBarrier() => sfxSource.PlayOneShot(shotBarrierSound);
    public void PlayEnemyDeath() => sfxSource.PlayOneShot(enemyDeathSound);
    public void PlayBaseHit() => sfxSource.PlayOneShot(baseHitSound);
    public void PlayExplosion() => sfxSource.PlayOneShot(explosionSound);
}