
using System; 
using UnityEngine;
//Attach to an object in the Unity editor. Add any object you wish to track the speed of in the _objectsToTrack array.
public class DisplaySpeedOfObjects : MonoBehaviour
{
    [SerializeField] float GUIUpdateTime = 0.1f;
    [SerializeField] Transform[] _objectsToTrack;
    [SerializeField] GUIStyle _GUIStyle;
    SpeedTracking[] _trackings;
    string GUIText; 
    private void Start()
    {
        if (_objectsToTrack != null)
        {
            _trackings = new SpeedTracking[_objectsToTrack.Length];
            for (int i = 0; i < _trackings.Length; i++)
            {
                _trackings[i] = new SpeedTracking(_objectsToTrack[i]);
            }
        }
    }
    private void FixedUpdate()
    {
        foreach (SpeedTracking tracking in _trackings)
        {
            tracking.Update(Time.fixedDeltaTime);
        } 
    } 
    private void OnGUI()
    {
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity,
           new Vector3(Screen.width / 1200.0f, Screen.height / 800.0f, 1.0f));
         
            GUIText = "SPEEDS\n";

            foreach (SpeedTracking tracking in _trackings)
            {
                GUIText += tracking.trackedObject.name + ": " + tracking.SpeedToString();
            } 

        GUILayout.Label(GUIText, _GUIStyle);
    }
    
    [Serializable]
    private class SpeedTracking
    {
        public SpeedTracking(Transform t)
        {
            trackedObject = t;
            _oldPosition = trackedObject.position;
        }
        public Transform trackedObject;
        private Vector3 _oldPosition;
        public float _speed;
        public void Update(float timeFrame)
        {
            _speed = Vector3.Distance(trackedObject.position, _oldPosition) / timeFrame;
            _oldPosition = trackedObject.position;
        }
        public string SpeedToString() => string.Format("Speed: {0:0.00} m/s\n", _speed);
    }
}
