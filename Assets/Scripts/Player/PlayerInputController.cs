using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    private PlayerController _playerController;
    private PlayerInput _playerInput;

    private void OnEnable()
    {
        _playerController = GetComponent<PlayerController>();

        _playerInput = new PlayerInput();
        _playerInput.PlayerMovement.Movement.performed += value => _playerController.MovementInput(value.ReadValue<Vector2>()); //Add Character movement
        _playerInput.PlayerMovement.Aim.performed += value => _playerController.AimRotation(value.ReadValue<Vector2>()); //Rotate Character by cursor
        _playerInput.PlayerMovement.Dash.performed += value => _playerController.Dashing(); //Press shift to dash
        _playerInput.PlayerMovement.Fire.performed += value => _playerController.WeaponShoot(); //Gun fire

        _playerInput.Enable();
    }
}
