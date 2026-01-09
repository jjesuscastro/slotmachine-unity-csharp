using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SymbolsCatalog : MonoBehaviour
{
    public GameObject symbolPrefab;

    [SerializeField]
    private List<Symbol> symbols;

    #region Singleton
    public static SymbolsCatalog i;
    void Awake()
    {
        if (i != null)
        {
            Debug.LogWarning("Multiple Symbols(s) found!");
            Destroy(gameObject);
        }
        else
        {
            i = this;
        }

        symbols = Resources.LoadAll<Symbol>("Symbols").OfType<Symbol>().ToList();
    }
    #endregion 

    public List<Symbol> GetSymbols()
    {
        return symbols;
    }
}
