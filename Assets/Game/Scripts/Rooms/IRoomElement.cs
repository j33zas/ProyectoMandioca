using UnityEngine;

public interface IZoneElement
{
    void OnDungeonGenerationFinallized();
    void OnPlayerEnterInThisRoom(Transform who);
    void OnPlayerExitInThisRoom();
    void OnUpdateInThisRoom();
    void OnPlayerDeath();
}
