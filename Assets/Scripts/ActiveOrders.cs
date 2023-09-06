using System;
using System.Collections.Generic;
using UnityEngine;

public class ActiveOrders : MonoBehaviour
{
    private class ActiveOrder
    {
        public readonly Order Order;
        public readonly OrderDisplay Display;

        public ActiveOrder(Order order, OrderDisplay display)
        {
            Order = order;
            Display = display;
            
            Display.Show(Order);
        }
    }

    [SerializeField] private PlayerScore score;
    [SerializeField] private OrderDisplay displayPrefab;
    [SerializeField] private Transform ordersDisplay;
    [SerializeField] private int maxConcurrentOrders = 5;
    private readonly List<ActiveOrder> _active = new();

    public bool IsFull => _active.Count >= maxConcurrentOrders;

    private void Start()
    {
        _active.Clear();
    }

    public void Add(Order order)
    {
        if (IsFull) return;
        
        var display = Instantiate(displayPrefab, ordersDisplay);
            
        _active.Add(new ActiveOrder(order, display));
    }
    
    public bool Remove(Food food)
    {
        foreach (var order in _active)
        {
            if (!food.IsPlated) continue;
            if (order.Order.finalItem.Id != food.Id) continue;
            
            _active.Remove(order);
            
            score.Score(order.Order.price);
            return true;
        }
        
        score.Miss();
        return false;
    }
}
