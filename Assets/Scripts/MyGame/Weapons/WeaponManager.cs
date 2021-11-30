using System;
using UnityEngine;
using Utils;

public class WeaponManager
{
    private Transform _gunHolder;
    private Transform _rayCastPoint;
    
    private BaseWeapon _currentWeapon;
    private string _previousWeaponName;

    private float _weaponLifeTime;
    private Timer _timer = new Timer();
    
    public WeaponManager(Transform gunHolder, Transform rayCastPoint)
    {
        _gunHolder = gunHolder;
        _rayCastPoint = rayCastPoint;
        LoadWeapon("M1911");
    }

    public void Tick()
    {
        DeactivateWeaponByTimer();
    }

    private BaseWeapon LoadWeapon(string weaponName)
    {
        string path = "Weapons/" + weaponName;
        
        BaseWeapon weapon = GameObject.Instantiate(Resources.Load<BaseWeapon>(path)).GetComponent<BaseWeapon>();
        SetWeaponSettings(weapon);
        _currentWeapon = weapon;
        return weapon;
    }

    private void SetWeaponSettings(BaseWeapon weapon)
    {
        weapon.Init(_rayCastPoint);
        weapon.transform.SetParent(_gunHolder);
        weapon.transform.position = _gunHolder.position;
        weapon.transform.rotation = _gunHolder.rotation;
    }

    public void SwitchWeapon(string weaponName, float deactivationTime)
    {
        if (GetWeaponName(_currentWeapon) == weaponName)
        {
            SetTimer(deactivationTime);
            return;
        }
        
        SetTimer(deactivationTime);
        _previousWeaponName = GetWeaponName(_currentWeapon);
        GameObject.Destroy(_currentWeapon.gameObject);
        _currentWeapon = LoadWeapon(weaponName);
    }

    private void SetTimer(float time)
    {
        _weaponLifeTime = time;
        _timer.Stop();
        _timer.Start();
    }

    private string GetWeaponName(BaseWeapon weapon)
    {
        return weapon.name.Replace("(Clone)", ""); // name(clone) => name
    }

    public void DeactivateWeaponByTimer()   
    {
        if(_timer.Time <= _weaponLifeTime)
            return;
        
        _timer.Stop();
        GameObject.Destroy(_currentWeapon.gameObject);
        LoadWeapon(_previousWeaponName);
    }

    public void Shoot()
    {
        _currentWeapon.Shoot();
    }


    // в конструкторе передаем ганХолдер
    // на старре тэйк веарон из ресуррсес лоад 
    // запоминаем в переменную и получаем бейзвеапон.
    // у него вызываем нужный метод
}



