using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public abstract class AbilityBehaviour : MonoBehaviour
    {
        AbilityConfig config;

        abstract public void Use(AbilityUseParams value);

    }
}
