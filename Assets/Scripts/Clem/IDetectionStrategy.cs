using System.Net.Http.Headers;
using UnityEngine;



namespace Platformer
{
    public interface IDetectionStrategy 
    {
        bool Execute (Transform player, Transform detector, CountdownTimer timer);

    }



}

