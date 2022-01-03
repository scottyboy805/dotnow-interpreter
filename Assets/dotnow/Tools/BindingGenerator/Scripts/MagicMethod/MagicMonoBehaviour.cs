
namespace dotnow.BindingGenerator.MagicMethod
{
    public class MagicMonoBehaviour
    {
        public void Awake() { }
        public void Start() { }
        public void OnEnable() { }
        public void OnDisable() { }
        public void OnDestroy() { }
        public void Update() { }
        public void LateUpdate() { }
        public void FixedUpdate() { }
        public void OnGUI() { }

        public void OnAnimatorIK() { }
        public void OnAnimatorMove() { }

        public void OnApplicationFocus(bool focus) { }
        public void OnApplicationPause(bool pause) { }
        public void OnApplicationQuit() { }
        public void OnAudioFilterRead(float[] data, int channels) { }
        public void OnBecameInvisible() { }
        public void OnBecameVisible() { }

        public void OnCollisionEnter(UnityEngine.Collision collision) { }
        public void OnCollisionStay(UnityEngine.Collision collision) { }
        public void OnCollisionExit(UnityEngine.Collision collision) { }
        public void OnTriggerEnter(UnityEngine.Collider other) { }
        public void OnTriggerStay(UnityEngine.Collider other) { }
        public void OnTriggerExit(UnityEngine.Collider other) { }
        public void OnTriggerEnter2D(UnityEngine.Collider2D collision) { }
        public void OnTriggerStay2D(UnityEngine.Collider2D collision) { }
        public void OnTriggerExit2D(UnityEngine.Collider2D collision) { }
        public void OnCollisionEnter2D(UnityEngine.Collision2D collision) { }
        public void OnCollisionStay2D(UnityEngine.Collision2D collision) { }
        public void OnCollisionExit2D(UnityEngine.Collision2D collision) { }
        public void OnControllerColliderHit(UnityEngine.ControllerColliderHit hit) { }        
        
        public void OnMouseDown() { }
        public void OnMouseUp() { }
        public void OnMouseDrag() { }
        public void OnMouseExit() { }
        public void OnMouseOver() { }

        public void OnPreCull() { }
        public void OnPreRender() { }
        public void OnRenderImage(UnityEngine.RenderTexture source, UnityEngine.RenderTexture destination) { }
        public void OnRenderObject() { }
        public void OnWillRenderObject() { }
        public void OnPostRender() { }

#if UNITY_INCLUDE_EDITORCALLBACKS
        public void OnDrawGizmos() { }
        public void OnDrawGizmosSelected() { }
#endif
    }
}
