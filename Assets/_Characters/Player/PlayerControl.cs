using UnityEngine;
using System.Collections;
using RPG.CameraUI;
using RPG.Characters.Weapons;

namespace RPG.Characters
{
    [RequireComponent(typeof(SpecialAbilities))]
    public class PlayerControl : MonoBehaviour
    {

        SpecialAbilities abilities;
        Character character;
        WeaponSystem weaponSystem;

        void Start()
        {
            weaponSystem = GetComponent<WeaponSystem>();
            character = GetComponent<Character>();
            abilities = GetComponent<SpecialAbilities>();

            RegisterForMouseEvents();
        }

        private void RegisterForMouseEvents()
        {
            CameraRaycaster cameraRaycaster = FindObjectOfType<CameraRaycaster>();
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
            var gameObject = enemy.gameObject;
            if (Input.GetMouseButton(0) && IsTargetInRange(gameObject))
            {
                weaponSystem.AttackTarget(gameObject);
            }
            else if (Input.GetMouseButton(0) && !IsTargetInRange(gameObject))
            {
                StartCoroutine(MoveAndAttack(gameObject));
            }
            else if (Input.GetMouseButtonDown(1) && IsTargetInRange(gameObject))
            {
                abilities.AttemptSpecialAbility(0, gameObject); 
            }
            else if (Input.GetMouseButtonDown(1) && IsTargetInRange(gameObject))
            {
                StartCoroutine(MoveAndPowerAttack(gameObject));
            }
        }

        IEnumerator MoveToTarget(GameObject target)
        {
            character.SetDestination(target.transform.position);
            while(!IsTargetInRange(target))
            {
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForEndOfFrame();
        }
        IEnumerator MoveAndAttack(GameObject enemy)
        {
            yield return StartCoroutine(MoveToTarget(enemy));
            weaponSystem.AttackTarget(enemy.gameObject);

        }
        IEnumerator MoveAndPowerAttack(GameObject enemy)
        {
            yield return StartCoroutine(MoveToTarget(enemy));
            abilities.AttemptSpecialAbility(0, enemy.gameObject);
        }

        private bool IsTargetInRange(GameObject enemy)
        {
            float distanceToTarget = (enemy.transform.position - transform.position).magnitude;
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