using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    public delegate void PlayAudioDelegate(EPlayerAudio _playerAudio);
    private PlayAudioDelegate playAudioCallback = null;
    public void Init(PlayerData _playerData, VoidIntDelegate _spUpdateCallback, VoidFloatDelegate _hpUpdateCallback, PlayAudioDelegate _playAudioCallback, VolumeProfile _volumeProfile)
    {
        playerData = _playerData;
        volumeProfile = _volumeProfile;
        moveCtrl = GetComponentInChildren<PlayerMovementController>();
        rotCtrl = GetComponentInChildren<PlayerRotateController>();
        animCtrl = GetComponentInChildren<PlayerAnimationController>();
        colCtrl = GetComponentInChildren<PlayerCollisionController>();
        statHp = GetComponentInChildren<PlayerStatusHp>();
        virtualMouse = GetComponentInChildren<VirtualMouse>();
        missileCamCtrl = GetComponentInChildren<PlayerMissileCam>();


        playerMesh = GetComponentInChildren<MeshRenderer>();

        camShake = CameraShake.Instance;

        playerData.tr = transform;
        playAudioCallback = _playAudioCallback;

        moveCtrl.Init(playerData,_spUpdateCallback);
        rotCtrl.Init(playerData);
        animCtrl.Init();
        colCtrl.Init(ChangeCollisionCondition, ChangeCollisionCondition, KnockBack, playerData, BoundaryTrigger);
        statHp.Init(IsDead, _hpUpdateCallback, volumeProfile);
        missileCamCtrl.Init();
        virtualMouse.Init(playerData);

    }

    private void BoundaryTrigger(bool _isExit)
    {
        Debug.Log($"PlayerExitBoundary : {_isExit}");

        if (_isExit)
        {
            volumeProfile.TryGet(out ColorAdjustments colorAd);
            colorAd.colorFilter.overrideState = true;
            StartCoroutine("BoundaryCoroutine");
        }
        else
        {
            volumeProfile.TryGet(out ColorAdjustments colorAd);
            colorAd.colorFilter.overrideState = false;
            StopCoroutine("BoundaryCoroutine");
        }
    }

    private IEnumerator BoundaryCoroutine()
    {
        
        yield return new WaitForSeconds(immediateDeadDelay);

        IsDead();
    }

    private void IsDead()
    {
        Debug.Log("�÷��̾� ���");
        playAudioCallback?.Invoke(EPlayerAudio.PLAYER_DEAD);
        //Time.timeScale = 0f;
    }

    private void ChangeCollisionCondition(Collision _coli)
    {
        moveCtrl.ChangeCollisionCondition(_coli, true);
    }

    private void ChangeCollisionCondition()
    {
        moveCtrl.ChangeCollisionCondition(null, false);
    }

    private void KnockBack(GameObject _colliderGo)
    {
        if (gameObject.layer.Equals(LayerMask.NameToLayer("PlayerInvincible")))
            if (_colliderGo.GetComponent<AttackableObject>())
                return;

        Vector3 knockBackDir = (transform.position - _colliderGo.transform.position).normalized;
        float knockBackAmount = 0f;
        float knockBackDelay = 2f;

        if (_colliderGo.CompareTag("CannonBall"))
        {
            knockBackAmount = 50f;
            knockBackDir = Vector3.down;
            Debug.Log("Hit");
        }
        else if (_colliderGo.CompareTag("GatlingGunBullet"))
        {
            knockBackAmount = 50f;
            knockBackDir = _colliderGo.transform.forward;
            Debug.Log("Hit");
        }
        else if (_colliderGo.CompareTag("ShakeBodyCollider"))
        {
            knockBackAmount = 500f;
            knockBackDir = Vector3.up;
            knockBackDelay = 4f;
        }
        else if (_colliderGo.CompareTag("WindBlow"))
        {
            knockBackAmount = 250f;
            knockBackDir = Vector3.up;
            knockBackDelay = 4f;
        }
        else if (_colliderGo.CompareTag("WindBlowForPattern"))
        {
            knockBackAmount = 1000f;
            knockBackDir = Vector3.up;
            knockBackDelay = 3f;
        }
        else if (_colliderGo.CompareTag("CrossLaser"))
        {
            knockBackAmount = 100f;
            knockBackDir = _colliderGo.transform.forward;
            Debug.Log("Hit");
        }
        else if (_colliderGo.CompareTag("BossShield"))
        {
            knockBackAmount = 50f;
        }
        else if (_colliderGo.CompareTag("AirPush"))
        {
            knockBackAmount = 1000f;
            knockBackDir = new Vector3(transform.position.x, 0f, transform.position.z).normalized;
            knockBackDelay = 4f;
        }
        else if (_colliderGo.CompareTag("Obstacle") || _colliderGo.CompareTag("Boss") || _colliderGo.CompareTag("BossBody") || _colliderGo.CompareTag("Floor"))
        {
            moveCtrl.ReduceSpeed();
            return;
        }

        playAudioCallback?.Invoke(EPlayerAudio.PLAYER_HIT);
        playerMesh.material.SetFloat("_isDamaged", 1);
        StopCoroutine("ResetPlayerDamagedBollean");
        StartCoroutine("ResetPlayerDamagedBollean", knockBackDelay);
        //Invoke("ResetPlayerDamagedBollean", 2f);

        moveCtrl.KnockBack(knockBackDir.normalized * knockBackAmount, knockBackDelay);
    }

    private IEnumerator ResetPlayerDamagedBollean(float _invincibleTime)
    {
        yield return new WaitForSeconds(_invincibleTime);
        playerMesh.material.SetFloat("_isDamaged", 0);
    }

    private void Update()
    {
        DebugColorChange();
        moveCtrl.PlayerDodge(playerData.input.InputQ, playerData.input.InputE);
        virtualMouse.UpdateMouseInput();
        animCtrl.SetSpeedFloat(playerData.currentMoveSpeed);
    }

    private void FixedUpdate()
    {
        virtualMouse.FixedUpdateMouseInput();
        rotCtrl.PlayerRotate();
        rotCtrl.PlayerFixedRotate();
        moveCtrl.CalcPlayerMove(playerData.input.InputZ, playerData.input.InputShift);
        moveCtrl.PlayerMove();

        //Debug.Log($"Player Speed: {moveCtrl.MoveSpeed}");

        if (moveCtrl.IsDash)
            screenMat.SetFloat("_isDash", 1);
        else
            screenMat.SetFloat("_isDash", 0);
    }

    private void AnimatorTest()
    {
    }


    private void DebugColorChange()
    {
        if (playerData.isCrash == true)
        {
            playerMesh.material.SetColor("_BaseColor", Color.red);
        }
        else if(gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            playerMesh.material.SetColor("_BaseColor", Color.yellow);
        }
        else if (playerData.isDash == true)
        {
            playerMesh.material.SetColor("_BaseColor", Color.blue);
        }
        else
        {
            playerMesh.material.SetColor("_BaseColor", Color.white);
        }
    }

    private PlayerMovementController moveCtrl = null;
    private PlayerRotateController rotCtrl = null;
    private PlayerAnimationController animCtrl = null;
    private PlayerCollisionController colCtrl = null;
    private PlayerStatusHp statHp = null;
    private VolumeProfile volumeProfile = null;
    private PlayerMissileCam missileCamCtrl = null;

    private MeshRenderer playerMesh = null;

    private VirtualMouse virtualMouse = null;

    private PlayerData playerData = null;
    private CameraShake camShake = null;

    [SerializeField]
    private Material screenMat = null;
    [SerializeField]
    private float immediateDeadDelay = 10f;
}
