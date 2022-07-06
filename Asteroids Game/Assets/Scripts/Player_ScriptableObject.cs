using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// klasa zawierająca wszystkie podstawowe stałe dane statku gracza
[CreateAssetMenu(menuName = "Spaceship", fileName = "New Spaceship")]
public class Player_ScriptableObject : ScriptableObject
{
    [Range(1, 4)] public int hp;
    [Min(0)] public float attackSpeed; 
    [Min(0)] public float movementSpeed;
    public Color spaceShipParticlesColor;
    public Sprite spaceShipSprite;

    public GameObject bulletObject;

    public virtual void Shoot(Player_Mechanics pm)
    {
        pm.StartCoroutine(ShootCoolDown(pm));
        ToolClass.PlayAudioClip(ToolClass.AudioLibrary.shoot);
        Quaternion rotation = Quaternion.Euler(0, 0, pm.GetAngle());
        switch (pm.shoot_PowerUpLevel)
        {
            case 0:
                Instantiate(bulletObject, pm.shootPosMain.position, rotation);
                break;
            case 1:
                Instantiate(bulletObject, pm.shootPosLeft.position, rotation);
                Instantiate(bulletObject, pm.shootPosRight.position, rotation);
                break;
            case 2:
                Instantiate(bulletObject, pm.shootPosMain.position, rotation);
                Instantiate(bulletObject, pm.shootPosLeft.position, rotation);
                Instantiate(bulletObject, pm.shootPosRight.position, rotation);
                break;
        }
    }
    private IEnumerator ShootCoolDown(Player_Mechanics playerMechanics)
    {
        playerMechanics.canShoot = false;
        yield return new WaitForSeconds(1f / playerMechanics.player_attackSpeed);
        playerMechanics.canShoot = true;
    }
}
