using UnityEngine;
using PurrNet.Prediction;

public class SetCamera : PredictedIdentity<SetCamera.CameraState>
{
    public struct CameraState : IPredictedData<CameraState>
    {
        public bool alreadyDone;
        public void Dispose() {}
    }

    protected override CameraState GetInitialState()
    {
        return new CameraState {
            alreadyDone = false,
        };
    }

    protected override void Simulate(ref CameraState state, float dt)
    {
        if (!state.alreadyDone)
        {
            if (isOwner)
            {
                Camera[] cameras = new Camera[100];
                Camera.GetAllCameras(cameras);

                foreach (Camera theCamera in cameras)
                {
                    if (theCamera != null)
                        theCamera.enabled = false;
                }

                GetComponent<Camera>().enabled = true;
            }
            state.alreadyDone = true;
        }
    }
}
