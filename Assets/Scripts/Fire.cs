using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fire : MonoBehaviour
{
    public enum GunType { Pistol, Shotgun, MachineGun }
    public GunType currentGunType = GunType.Pistol;

    public GameObject bullet;

    public int pistolMagazineSize = 7;   // 권총의 탄창 크기
    public int pistolTotalAmmo = 14;     // 권총의 전체 탄약 크기

    public int shotgunMagazineSize = 2;  // 산탄총의 탄창 크기
    public int shotgunTotalAmmo = 20;    // 산탄총의 전체 탄약 크기

    public int machineGunMagazineSize = 30; // 기관총의 탄창 크기
    public int machineGunTotalAmmo = 60;    // 기관총의 전체 탄약 크기

    private int currentPistolAmmo;       // 현재 권총 탄창에 남은 총알 수
    private int currentShotgunAmmo;      // 현재 산탄총 탄창에 남은 총알 수
    private int currentMachineGunAmmo;   // 현재 기관총 탄창에 남은 총알 수

    private bool isReloading = false; // 재장전 중인지 여부를 나타내는 변수
    private bool isShooting = false; // 총을 발사 중인지 여부를 나타내는 변수

    public Text ammoText;

    // Start is called before the first frame update
    void Start()
    {
        currentPistolAmmo = pistolMagazineSize;
        currentShotgunAmmo = shotgunMagazineSize;
        currentMachineGunAmmo = machineGunMagazineSize;

        SwitchGunType(currentGunType);
        UpdateAmmoText();
    }

    // Update is called once per frame
    void Update()
    {
        if (isReloading)
            return;

        if (Input.GetMouseButton(0) && GetCurrentAmmo() > 0 && currentGunType == GunType.MachineGun)
        {
            if (!isShooting)
            {
                StartCoroutine(ShootContinuously());
            }
        }
        else if (Input.GetMouseButtonDown(0) && GetCurrentAmmo() > 0)
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R) && GetCurrentAmmo() < GetCurrentMagazineSize() && GetCurrentTotalAmmo() > 0)
        {
            StartCoroutine(Reload());
        }

        // Switch gun type with mouse scroll
        if (Input.GetAxis("Mouse ScrollWheel") > 0.0f)
        {
            SwitchToNextGunType();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0.0f)
        {
            SwitchToPreviousGunType();
        }
    }

    void Shoot()
    {
        switch (currentGunType)
        {
            case GunType.Pistol:
                Instantiate(bullet, transform.position + transform.forward, transform.rotation);
                currentPistolAmmo--;
                break;
            case GunType.Shotgun:
                for (int i = 0; i < 5; i++)
                {
                    Vector3 spread = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                    Instantiate(bullet, transform.position + transform.forward + spread, transform.rotation);
                }
                currentShotgunAmmo--;
                break;
            case GunType.MachineGun:
                Instantiate(bullet, transform.position + transform.forward, transform.rotation);
                currentMachineGunAmmo--;
                break;
        }

        UpdateAmmoText();
    }

    IEnumerator ShootContinuously()
    {
        isShooting = true;
        while (GetCurrentAmmo() > 0 && Input.GetMouseButton(0))
        {
            Shoot();
            yield return new WaitForSeconds(0.1f); // 연사 간격
        }
        isShooting = false;
    }

    IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(GetReloadTime()); // 장전하는데 필요한 시간만큼 대기

        int bulletsToReload = GetCurrentMagazineSize() - GetCurrentAmmo();
        if (GetCurrentTotalAmmo() >= bulletsToReload)
        {
            SetCurrentAmmo(GetCurrentMagazineSize());
            SetCurrentTotalAmmo(GetCurrentTotalAmmo() - bulletsToReload);
        }
        else
        {
            SetCurrentAmmo(GetCurrentAmmo() + GetCurrentTotalAmmo());
            SetCurrentTotalAmmo(0);
        }
        UpdateAmmoText();
        isReloading = false;
    }

    void UpdateAmmoText()
    {
        ammoText.text = GetCurrentAmmo().ToString() + '/' + GetCurrentTotalAmmo().ToString();
    }

    void SwitchGunType(GunType type)
    {
        currentGunType = type;
        switch (type)
        {
            case GunType.Pistol:
                SetCurrentAmmo(currentPistolAmmo);
                break;
            case GunType.Shotgun:
                SetCurrentAmmo(currentShotgunAmmo);
                break;
            case GunType.MachineGun:
                SetCurrentAmmo(currentMachineGunAmmo);
                break;
        }

        UpdateAmmoText(); // 총을 바꿀 때마다 남은 탄약수를 업데이트하여 화면에 표시
    }

    void SwitchToNextGunType()
    {
        int nextGunType = ((int)currentGunType + 1) % System.Enum.GetValues(typeof(GunType)).Length;
        SwitchGunType((GunType)nextGunType);
    }

    void SwitchToPreviousGunType()
    {
        int previousGunType = ((int)currentGunType - 1 + System.Enum.GetValues(typeof(GunType)).Length) % System.Enum.GetValues(typeof(GunType)).Length;
        SwitchGunType((GunType)previousGunType);
    }

    int GetCurrentAmmo()
    {
        switch (currentGunType)
        {
            case GunType.Pistol:
                return currentPistolAmmo;
            case GunType.Shotgun:
                return currentShotgunAmmo;
            case GunType.MachineGun:
                return currentMachineGunAmmo;
            default:
                return 0;
        }
    }

    int GetCurrentMagazineSize()
    {
        switch (currentGunType)
        {
            case GunType.Pistol:
                return pistolMagazineSize;
            case GunType.Shotgun:
                return shotgunMagazineSize;
            case GunType.MachineGun:
                return machineGunMagazineSize;
            default:
                return 0;
        }
    }

    int GetCurrentTotalAmmo()
    {
        switch (currentGunType)
        {
            case GunType.Pistol:
                return pistolTotalAmmo;
            case GunType.Shotgun:
                return shotgunTotalAmmo;
            case GunType.MachineGun:
                return machineGunTotalAmmo;
            default:
                return 0;
        }
    }

    void SetCurrentAmmo(int value)
    {
        switch (currentGunType)
        {
            case GunType.Pistol:
                currentPistolAmmo = value;
                break;
            case GunType.Shotgun:
                currentShotgunAmmo = value;
                break;
            case GunType.MachineGun:
                currentMachineGunAmmo = value;
                break;
        }
    }

    void SetCurrentTotalAmmo(int value)
    {
        switch (currentGunType)
        {
            case GunType.Pistol:
                pistolTotalAmmo = value;
                break;
            case GunType.Shotgun:
                shotgunTotalAmmo = value;
                break;
            case GunType.MachineGun:
                machineGunTotalAmmo = value;
                break;
        }
    }

    float GetReloadTime()
    {
        switch (currentGunType)
        {
            case GunType.Pistol:
                return 1f;
            case GunType.Shotgun:
                return 3f;
            case GunType.MachineGun:
                return 2f;
            default:
                return 1f;
        }
    }
}