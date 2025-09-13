using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class Cube : MonoBehaviour
{
    private Renderer _renderer;
    private Color _defaultColor = Color.white;
    private int _minimumValueLifetime = 2;
    private int _maximumValueLifetime = 6;
    public BoxCollider Collider { get; private set; }
    public Rigidbody Rigidbody { get; private set; }
    public Color Color { get; private set; }
    public bool IsTouched { get; private set; }
    public event Action<Cube> CubeFallenDown;

    private void Awake()
    {
        IsTouched = false;
        _renderer = GetComponent<Renderer>();
        Collider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        _renderer.material.color = _defaultColor;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (IsTouched != true && collision.gameObject.CompareTag("Plane"))
        {
            IsTouched = true;
            ChangeColor();
            StartCoroutine(DetermineLifetime());
        }
    }

    private IEnumerator DetermineLifetime()
    {
        var wait = new WaitForSeconds(Random.Range(_minimumValueLifetime, _maximumValueLifetime));

        yield return wait;

        CubeFallenDown?.Invoke(this);
    }

    public void ChangeColor()
    {
        _renderer.material.color = Random.ColorHSV();
    }

    public void ResetCondition()
    {
        _renderer.material.color = _defaultColor;
        IsTouched = false;
    }
}