using UnityEngine;
using PurrNet.Prediction;

// I don't know if this is a good or a bad way to handle in-world items
// I think it works, though
public class InWorldItem : PredictedIdentity<InWorldItem.InWorldItemState>
{
    // You gotta drag and drop the model
    // It should be a child of the GameObject this script is attached to
    [SerializeField] private GameObject model;

    public struct InWorldItemState : IPredictedData<InWorldItemState>
    {
        public bool hidden;
        
        public void Dispose() {}
    }

    protected override InWorldItemState GetInitialState()
    {
        return new InWorldItemState{
            hidden = false,
        };
    }

    // I don't know how to delete stuff with PurrNet's prediction
    // so this is what we're doing
    protected override void Simulate(ref InWorldItemState state, float delta)
    {
        model.SetActive(!state.hidden);
    }
}
