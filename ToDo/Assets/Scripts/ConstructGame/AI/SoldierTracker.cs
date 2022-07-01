using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierTracker : MonoBehaviour
{
    [SerializeField]
    private Player player = null;

    [SerializeField]
    private List<Soldier> soldiers;

    private void Awake() {
        soldiers = new List<Soldier>();
    }

    private void OnEnable() {
        Player.OnSoldierSpawned += HandleSoldierSpawned;
        Player.OnSoldierDespawned += HandleSoldierDespawned;
    }

    private void OnDisable() {
        Player.OnSoldierSpawned -= HandleSoldierSpawned;
        Player.OnSoldierDespawned -= HandleSoldierDespawned;
    }

    private void HandleSoldierSpawned(Soldier soldier) {
        soldiers.Add(soldier);
    }

    private void HandleSoldierDespawned(Soldier soldier) {
        soldiers.Remove(soldier);
    }

    public List<Soldier> GetSoldiers{
        get {
            return soldiers;
        }
    }
}
