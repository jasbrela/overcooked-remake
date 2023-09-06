using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Level : MonoBehaviour
{
    [Serializable]
    public class OrderSpawnData
    {
        public Order order;
        public float weight;
    }

    [SerializeField] private ActiveOrders activeOrders;
    [SerializeField] private List<OrderSpawnData> possibleOrders = new();
    [SerializeField] private Timer timer;
    [SerializeField] private float secondsBetweenOrders = 1f;
    [SerializeField] private int totalSeconds = 60 * 3;
    
    private float _totalProbability;
    private Coroutine _spawn;
    
    private void Start()
    {
        foreach (var possible in possibleOrders)
        {
            _totalProbability += possible.weight;
        }
        
        timer.Time(totalSeconds);
        _spawn = StartCoroutine(Order());
        
        timer.OnFinish += OnFinish;
    }

    private void OnFinish() => StopCoroutine(_spawn);
    private void OnDisable() => StopCoroutine(_spawn);

    private IEnumerator Order()
    {
        while (true)
        {
            yield return new WaitForSeconds(secondsBetweenOrders);
            
            activeOrders.Add(Randomize());
            while (activeOrders.IsFull) yield return null;
        }
    }

    private Order Randomize()
    {
        float randomValue = Random.Range(0f, _totalProbability);
        float cumulativeProbability = 0f;

        foreach (var possible in possibleOrders)
        {
            cumulativeProbability += possible.weight;
            if (randomValue < cumulativeProbability)
            {
                return possible.order;
            }
        }

        return possibleOrders[0].order;
    }
}
