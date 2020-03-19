using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllable : WalkingEntity
{
    //aca vamos a usar esta  jerarquia para cancelar el movimiento del player y 
    //poder usar el character como un NPC, nos puede servir para mini cinematicas
    //o tal vez para reposicionar el character antes de comentar una animacion

    protected override void OnFixedUpdate() { }
    protected override void OnPause() { }
    protected override void OnResume() { }
    protected override void OnTurnOff() { }
    protected override void OnTurnOn() { }
    protected override void OnUpdateEntity() { }
}
