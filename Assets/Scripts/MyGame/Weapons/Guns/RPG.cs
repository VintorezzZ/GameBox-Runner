using Com.MyCompany.MyGame;
using UnityEngine;

public class RPG : BaseWeapon
{
    public override void Init(Transform rayCastPoint)
    {
        base.Init(rayCastPoint);
    }

    public override void Shoot()
    {
        base.Shoot();
        ProcessShoot();
    }

    private void ProcessShoot()
    {
       InstantiateBullet();
    }
    
    
    private void InstantiateBullet()
    {
        Shell shell = PoolManager.Get(PoolType.Rockets).GetComponent<Shell>();
        
        SetShellSettings(shell);
    }
    
    private void SetShellSettings(Shell shell)                        // уточнить 
    {
        shell.PlayerVelocity = RoomController.Instance.localPlayer.moveController.Speed;
        shell.transform.position = transform.position + -transform.forward + Vector3.up;
        shell.transform.rotation = RoomController.Instance.localPlayer.transform.rotation;
        shell.Init(targetDirection);
        shell.gameObject.SetActive(true);
    }
}
