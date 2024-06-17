using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer), typeof(Rigidbody))]
public class Cube : MonoBehaviour
{
    private Renderer _renderer;
    private Rigidbody _rigidbody;
    private bool _isChanged;
    private Color _defaltColor;

    public event Action<Cube> Returned;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _rigidbody = GetComponent<Rigidbody>();
        _defaltColor = _renderer.material.color;
    }

    private void OnEnable()
    {
        _isChanged = false;
        _renderer.material.color = _defaltColor;
    }

    private void OnDisable()
    {
        Returned?.Invoke(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Platform platform) && _isChanged == false)
        {
            SetRandomColor();
            _isChanged = true;
            StartCoroutine(ReturnPool());
        }
    }    

    private IEnumerator ReturnPool()
    {
        int minLifeTime = 2;
        int maxLifeTime = 6;

        yield return new WaitForSeconds(UnityEngine.Random.Range(minLifeTime, maxLifeTime));
        gameObject.SetActive(false);        
    }

    private void SetRandomColor()
    {
        _renderer.material.color = UnityEngine.Random.ColorHSV();
    }
}
