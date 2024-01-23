using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputIcons
{
    public class II_InputActions_Controller : MonoBehaviour
    {

        private II_InputActions_Player inputActions;
        public II_Player player;

        private void Awake()
        {
            inputActions = new II_InputActions_Player();
            inputActions.PlatformerControls.Enable();
        }

        private void OnEnable()
        {
            inputActions.PlatformerControls.Attack.performed += On_Plattformer_Attack;
        }

        private void OnDisable()
        {
            inputActions.PlatformerControls.Attack.performed -= On_Plattformer_Attack;
        }

        public void On_Plattformer_Attack(InputAction.CallbackContext context)
        {
            player.OnAttack(context);
        }
    }
}

