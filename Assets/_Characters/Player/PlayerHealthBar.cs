using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters
{
    [RequireComponent(typeof(Image))]
    public class PlayerHealthBar : MonoBehaviour
    {
        Image healthOrd;
        Player player;

        // Use this for initialization
        void Start()
        {
            player = FindObjectOfType<Player>();
            healthOrd = GetComponent<Image>();
        }

        // Update is called once per frame
        void Update()
        {
            healthOrd.fillAmount = player.healthAsPercentage;
        }
    }
}