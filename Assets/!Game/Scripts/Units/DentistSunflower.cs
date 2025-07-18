using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DentistSunflower : Unit
{
   [Header("Dentist Settings")]
    public int teethPerGeneration = 25;
    
    protected override void PerformAction()
    {
        ResourceManager.Instance.AddGoldenTeeth(teethPerGeneration);
        
        // Визуальный эффект генерации зубов
        SpawnTeethEffect();
    }
    
    private void SpawnTeethEffect()
    {
        // Создаем визуальный эффект падающих зубов
        GameObject teethEffect = GameObject.CreatePrimitive(PrimitiveType.Cube);
        teethEffect.transform.position = transform.position + Vector3.up * 2f;
        teethEffect.transform.localScale = Vector3.one * 0.1f;
        teethEffect.GetComponent<Renderer>().material.color = Color.yellow;
        
        // Простая анимация падения
        Destroy(teethEffect, 1f);
    }
}
