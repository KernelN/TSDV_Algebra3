using System.Collections;
using System.Collections.Generic;
using CustomMath;
using MathDebbuger;
using UnityEngine;
using UnityEngine.Serialization;

public class EjerciciosVec3 : MonoBehaviour
{
    enum Ejercicio { Uno, Dos, Tres, Cuatro, Cinco, Seis, Siete, Ocho, Nueve, Diez}
    
    [Header("Inputs")]
    [SerializeField] Vec3[] inputs = new Vec3[2];
    [SerializeField] float floatInput;
    
    [Header("Ejercicios")]
    [SerializeField] Ejercicio ejercicio;
    [SerializeField] Vec3[] resultados = new Vec3[10];

    bool[] vectoresPrendidos = new[] { true, true, true };
    int ejerActual = 0;
    float t;

    void Start()
    {
        Vector3Debugger.AddVector(inputs[0], Color.blue, "elAzul");
        Vector3Debugger.EnableEditorView("elAzul");
        Vector3Debugger.AddVector(inputs[1], Color.green, "elVerde");
        Vector3Debugger.EnableEditorView("elVerde");
        Vector3Debugger.AddVector(resultados[ejerActual], Color.red, "elRojo");
        Vector3Debugger.EnableEditorView("elRojo");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ActivarVector(0, "elRojo");
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            ActivarVector(1, "elVerde");
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            ActivarVector(2, "elAzul");
        }

        CalcularEjercicio();
        
        if ((int)ejercicio != ejerActual)
        {
            t = 0;
            ejerActual = (int)ejercicio;
        }
        else if (!Input.GetKeyDown(KeyCode.U)) return;
                    
        StartCoroutine(UpdateVector());
    }

    void ActivarVector(int vector, string nombre)
    {
        if (vectoresPrendidos[vector])
        {
            Vector3Debugger.TurnOffVector(nombre);
        }
        else
        {
            Vector3Debugger.TurnOnVector(nombre);
        }

        vectoresPrendidos[vector] = !vectoresPrendidos[vector];
    }

    void CalcularEjercicio()
    {
        switch (ejercicio)
        {
            case Ejercicio.Uno:
                resultados[0] = inputs[0] + inputs[1];
                break;
            case Ejercicio.Dos:
                resultados[1] = -inputs[0] + inputs[1];
                break;
            case Ejercicio.Tres:
                resultados[2] = inputs[0];
                resultados[2].Scale(inputs[1]);
                break;
            case Ejercicio.Cuatro:
                resultados[3] = Vec3.Cross(inputs[0], inputs[1]);
                break;
            case Ejercicio.Cinco:
                t += Time.deltaTime;
                if (t > 1) t -= 1;
                resultados[4] = Vec3.Lerp(inputs[0], inputs[1], t);
                break;
            case Ejercicio.Seis:
                resultados[5] = Vec3.Max(inputs[0], inputs[1]);
                break;
            case Ejercicio.Siete:
                resultados[6] = Vec3.Project(inputs[0], inputs[1]);
                break;
            case Ejercicio.Ocho:
                Vec3 r8 = Vec3.Max(inputs[0], inputs[1]);
                resultados[7] = (inputs[0] + inputs[1]).normalized * r8.magnitude;
                break;
            case Ejercicio.Nueve:
                resultados[8] = Vec3.Reflect(inputs[0], inputs[1].normalized);
                break;
            case Ejercicio.Diez:
                t += Time.deltaTime;
                if (t >= 10) t -= 10;
                resultados[9] = Vec3.LerpUnclamped(inputs[0], inputs[1], -t);
                break;
        }
    }

    IEnumerator UpdateVector()
    {
        for (int i = 0; i < 100; i++)
        {
            Vector3Debugger.UpdatePosition("elAzul", inputs[0]);
            Vector3Debugger.UpdatePosition("elVerde", inputs[1]);
            Vector3Debugger.UpdatePosition("elRojo", resultados[ejerActual]);
            yield return new WaitForSeconds(0.2f);
        }
    }
}
