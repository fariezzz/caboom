using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkingSprites : MonoBehaviour
{
    public SpriteRenderer spriteRenderer1; // Sprite pertama
    public SpriteRenderer spriteRenderer2; // Sprite kedua
    public float blinkInterval = 2f; // Waktu interval berkedip (dalam detik)

    private bool isSprite1Active = true;

    private void Start()
    {
        // Memulai berkedip secara berkala
        InvokeRepeating("ToggleSprites", 0f, blinkInterval);
    }

    private void ToggleSprites()
    {
        isSprite1Active = !isSprite1Active;

        // Mengatur visibilitas sprite pertama dan kedua berdasarkan nilai isSprite1Active
        spriteRenderer1.enabled = isSprite1Active;
        spriteRenderer2.enabled = !isSprite1Active;
    }
}
