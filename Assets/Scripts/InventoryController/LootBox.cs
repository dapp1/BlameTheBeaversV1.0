using System.Collections;
using UnityEngine;

public class LootBox : MonoBehaviour
{
    [SerializeField] 
    private Vector2 _forceRange;
    
    [SerializeField] 
    private Vector2 _torqueRange;
    
    [SerializeField] 
    private Transform _render;
    
    [SerializeField] 
    private Transform _particles;

    public InventoryItemObject ItemPrefab;
    
    private Animator _anim;
    private Rigidbody2D _rb;
    
    
    private void Start()
    {
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        
        
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(3);

        for (int i = 0; i < 2; i++)
        {
            _rb.AddForce(new Vector2(0, Random.Range(_forceRange.x, _forceRange.y)));
            _rb.AddTorque(Random.Range(_torqueRange.x, _torqueRange.y));
            
            yield return new WaitForSeconds(1f);
        }
        
        yield return new WaitForSeconds(1f);
        
        _render.gameObject.SetActive(false);
        _particles.gameObject.SetActive(true);
        _anim.Play("Explode");
        
        Instantiate(ItemPrefab, transform.position, Quaternion.identity);
        
        yield return new WaitForSeconds(3);
        
        Destroy(gameObject);
    }
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("ground"))
        {
            StartCoroutine(Explode());
        }
    }
}
