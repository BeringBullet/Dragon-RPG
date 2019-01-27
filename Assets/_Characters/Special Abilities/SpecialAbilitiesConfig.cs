using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{

    abstract public class SpecialAbilitiesConfig : ScriptableObject
    {
        [Header("Special Ability General")]
        [SerializeField] float energyCost = 10f;

        protected ISpecialAbilities behaviour;

        abstract public void AttachComponentTo(GameObject gameObjectToAttachTo);

        public void Use()
        {
            behaviour.Use();
        }
    }
}
