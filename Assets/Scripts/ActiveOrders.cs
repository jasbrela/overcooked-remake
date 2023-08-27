using System;
using System.Collections.Generic;
using UnityEngine;

public class ActiveOrders : MonoBehaviour
{
    private class ActiveOrder
    {
        public Order Order;
        public OrderDisplay Display;

        public ActiveOrder(Order order, OrderDisplay display)
        {
            Order = order;
            Display = display;
            
            Display.Show(Order);
        }
    }
    
    [SerializeField] private OrderDisplay displayPrefab;
    [SerializeField] private Transform ordersDisplay;
    [SerializeField] private int maxConcurrentOrders = 5;
    private readonly List<ActiveOrder> _active = new();

    public Action<Order> OnRemove;
    public Action OnAdd;

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
        OnAdd?.Invoke();
    }
    
    public bool Remove(Food food)
    {
        foreach (var order in _active)
        {
            if (order.Order.finalItem.Id != food.Id) continue;
            
            _active.Remove(order);
            
            Destroy(order.Display.gameObject);
            OnRemove?.Invoke(order.Order);
            return true;
        }
        return false;
    }
}
