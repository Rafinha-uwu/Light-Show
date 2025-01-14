using UnityEngine;

namespace SpaceFusion.SF_Portals.Scripts {
    /// <summary>
    /// player controller inspired by the unity docs: https://docs.unity3d.com/ScriptReference/CharacterController.Move.html
    /// </summary>
    public class PlayerController : Portable {

        public Camera cam;
        public float speed = 5;
        public float jumpHeight = 1.0f;
        public float mouseSpeed = 10;

        public bool rotateOnlyOnRightMouseHold;
        private CharacterController controller;
        public float viewDirection;

        private readonly float _gravity = 9.81f;
        private float _jumpForce;
        private Vector3 _playerVelocity;
        private bool _grounded;


      private   void Start() {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            controller = GetComponent<CharacterController>();
            viewDirection = transform.eulerAngles.y;
            _jumpForce = Mathf.Sqrt(jumpHeight * 3.0f * _gravity);
        }

       private  void Update() {
            _grounded = controller.isGrounded;
            if (_grounded && _playerVelocity.y < 0) {
                _playerVelocity.y = 0f;
            }

            // use TransformDirection to make sure we move in the right direction even if our view/player is rotated
            var worldDirection =
                transform.TransformDirection(new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")));
            var moveWorldDirection = worldDirection * speed;
            // apply gravity to the vertical velocity
            _playerVelocity.y -= _gravity * Time.deltaTime;
            // combine move direction (x & z) with the jump option (y velocity)
            _playerVelocity = new Vector3(moveWorldDirection.x, _playerVelocity.y, moveWorldDirection.z);
            // move the player based on the calculated playerVelocity
            controller.Move(_playerVelocity * Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.Space) && _grounded) {
                // apply jump force to vertical velocity
                _playerVelocity.y = _jumpForce;
            }

            // Unity Editor registers mouse input already before the game view loads completely
            // so the movement from the "start game" button to the game view will be registered as rotation, which is pretty annoying
            // for this reason I added a option to only activate the rotation if the right mouse button is hold
            // if you do not like this you can set the rotateOnlyOnRightMouseHold property to false
            var mouseInput = Vector2.zero;
            if (!rotateOnlyOnRightMouseHold || IsRightMouseButtonDown()) {
                mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
            }
        

            // the up/down rotation is applied directly to the camera, we only want the view to rotate and not the whole player itself
            cam.transform.Rotate(Vector3.right * (-mouseInput.y * mouseSpeed));


            // handle viewDirection globally in script
            // because the teleport function will adapt the viewDirection based on the pos and rot of the calculated transformation matrix between the 2 portals and the player
            viewDirection += mouseInput.x * mouseSpeed;
            // Convert the viewDirection to a quaternion and apply the rotation to the transform
            transform.rotation = Quaternion.Euler(0, viewDirection, 0);
        }

        public override void Teleport(Transform from, Transform to, Vector3 pos, Quaternion rot) {
            transform.position = pos;
            viewDirection += Mathf.DeltaAngle(viewDirection, rot.eulerAngles.y);
            transform.rotation = Quaternion.Euler(0, viewDirection, 0);
            _playerVelocity = to.TransformVector(from.InverseTransformVector(_playerVelocity));
            Physics.SyncTransforms();
        }

        /// <summary>
        /// checks if the right mouse button is being pressed or being hold
        /// </summary>
        private bool IsRightMouseButtonDown() {
            return Input.GetMouseButton(1);
        }

    }
}