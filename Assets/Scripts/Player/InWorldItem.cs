using UnityEngine;
using PurrNet.Prediction;

// I don't know if this is a good or a bad way to handle in-world items
// I think it works, though
[RequireComponent(typeof(PredictedTransform))]
[RequireComponent(typeof(PredictedRigidbody))]
public class InWorldItem : PredictedIdentity<InWorldItem.InWorldItemState>
{
    // You gotta drag and drop the model
    // It should be a child of the GameObject this script is attached to
    [SerializeField] private GameObject model;
    [SerializeField] private ItemObject _item;
    public ItemObject item { get => _item; }

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

    private Collider collider;
    private PredictedRigidbody predictedRigidbody;

    protected override void LateAwake()
    {
        collider = GetComponent<Collider>();
        predictedRigidbody = GetComponent<PredictedRigidbody>();
    }

    protected override void Simulate(ref InWorldItemState state, float delta)
    {
        model.SetActive(!state.hidden);
        collider.enabled = !state.hidden;
        predictedRigidbody.isKinematic = state.hidden;
    }
}
