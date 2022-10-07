using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[System.Serializable]
public class LocomotionManager : MonoBehaviour
{
    public XRController turnController;
    public XRController moveController;
    public DeviceBasedContinuousMoveProvider mover;

    private void Update()
    {
        bool snapTurn = ES3.Load<bool>("Snap Turn");
        bool inverted = ES3.Load<bool>("Controls Inverted");

        var snapTurnProvider = GetComponent<DeviceBasedSnapTurnProvider>();
        var smoothTurnProvider = GetComponent<DeviceBasedContinuousTurnProvider>();

        if ( inverted )
        {
            if ( smoothTurnProvider )
            {
                smoothTurnProvider.controllers.Clear();
                smoothTurnProvider.controllers.Add(moveController);
            }

            if ( snapTurnProvider )
            {
                snapTurnProvider.controllers.Clear();
                snapTurnProvider.controllers.Add(moveController);
            }

            mover.controllers.Clear();
            mover.controllers.Add(turnController);
        }

        else
        {
            if ( smoothTurnProvider )
            {
                smoothTurnProvider.controllers.Clear();
                smoothTurnProvider.controllers.Add(turnController);
            }

            if ( snapTurnProvider )
            {
                snapTurnProvider.controllers.Clear();
                snapTurnProvider.controllers.Add(turnController);
            }

            mover.controllers.Clear();
            mover.controllers.Add(moveController);
        }

        if ( snapTurn )
        {
            if ( smoothTurnProvider )
            {
                Destroy(smoothTurnProvider);

                gameObject.AddComponent<DeviceBasedSnapTurnProvider>().controllers.Add(turnController);
            }
        }

        else
        {
            if ( snapTurnProvider )
            {
                Destroy(snapTurnProvider);

                gameObject.AddComponent<DeviceBasedContinuousTurnProvider>().controllers.Add(turnController);
            }
        }
    }
}
