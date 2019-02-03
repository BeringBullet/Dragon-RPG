using UnityEngine;

using RPG.CameraUI;
using RPG.Characters.Weapons;
using UnityEngine.Assertions;

namespace RPG.Characters
{
    [RequireComponent(typeof(SpecialAbilities))]
    public class PlayerMovement : MonoBehaviour
    {
        EnemyAI currentEnemy;
        CameraRaycaster cameraRaycaster;
        SpecialAbilities abilities;
        Character character;
        WeaponSystem weaponSystem;

        void Start()
        {
            weaponSystem = GetComponent<WeaponSystem>();
            character = GetComponent<Character>();
            abilities = GetComponent<SpecialAbilities>();

            cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += onMouseOverEnemy;
            cameraRaycaster.onMouseOverWalkable += onMouseOverWalkable;
        }

        private void onMouseOverWalkable(Vector3 destination)
        {
            if (Input.GetMouseButton(0))
            {
                character.SetDestination(destination);
            }
        }

        private void onMouseOverEnemy(EnemyAI enemy)
        {
            currentEnemy = enemy;
            if (Input.GetMouseButton(0) && IsTargetInRange())
            {
                weaponSystem.AttackTarget(currentEnemy.gameObject);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                abilities.AttemptSpecialAbility(0, currentEnemy.gameObject);
            }
        }
        private bool IsTargetInRange()
        {
            float distanceToTarget = (currentEnemy.transform.position - transform.position).magnitude;
            return distanceToTarget <= weaponSystem.CurrectWeaponConfig.MaxAttackRange;
        }

        private void Update()
        {
            SnanForAbilityKeyDown();         
        }


        private void SnanForAbilityKeyDown()
        {
            for (int i = 0; i < abilities.Length; i++)
            {
                if (Input.GetKeyDown(i.ToString()))
                {
                    abilities.AttemptSpecialAbility(i);
                }
            }
        }
    }
}