using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Utilities.Gltf.Serialization;
using Siccity.GLTFUtility;
using UnityEngine;

namespace Assets.Scripts
{
    public class GlbLoader
    {
        private const string LOADING_TITLE = "模型載入流程";
        private const string LOADING_REQUEST_DATA = "向遠端主機請求模型資料...";
        private const string LOADING_REQUEST_FAILED = "遠端主機沒有回應";
        private const string LOADING_RESPONSE_FAILED = "資料請求時發生錯誤";
        private const string LOADING_MODEL = "模型建構中...";
        private const string LOADING_MODEL_FAILED = "模型建構失敗";
        private const string LOADING_MODEL_NULL = "模型不存在";
        private const string LOADING_SUCCESS_TITLE = "模型載入成功";
        private const string LOADING_FAILED_TITLE = "模型載入失敗";
        private const string LOADING_FAILED_MESSAGE = "請檢查網路連線或聯絡管理員修正錯誤\nexample@gmail.com";

        private GlbModelManager _modelManager;
        private PopupDialog _dialogController;
        private int _cacheFurnitureID;
        private Vector3 _modelSize;
        private Vector3 _modelCenter;


        // Set dialog controller 
        public void SetPopupDialog(PopupDialog dialogController)
        {
            _dialogController = dialogController;
        }

        // Set model manager
        public void SetModelManager(GlbModelManager modelManager)
        {
            _modelManager = modelManager;
        }

        // Set cache furniture ID
        public void SetFurnitureID(int id)
        {
            _cacheFurnitureID = id;
        }

        // Using MeshRenderer to calculate 3D object (and its child object) size and center by bounds

        private void AddColliderToModel(GameObject model)
        {
            MeshCollider rootCollider = model.AddComponent<MeshCollider>();
            MeshRenderer[] meshRenderers = model.GetComponentsInChildren<MeshRenderer>();
            Bounds combinedBounds = new Bounds();
            rootCollider.convex = true;
            foreach (MeshRenderer meshRenderer in meshRenderers)
            {
                GameObject meshObject = meshRenderer.gameObject;
                MeshCollider childCollider = meshObject.AddComponent<MeshCollider>();
                Bounds childBounds = meshRenderer.bounds;
                childCollider.convex = true;
                combinedBounds.Encapsulate(childBounds);
            }
            _modelSize = combinedBounds.size;
            _modelCenter = combinedBounds.center;
        }

        // Configure 3D object's components to grabbable and axis constraint
        private void ConfigureModelComponents(GameObject model)
        {
            ObjectManipulator objectManipulator = model.AddComponent<ObjectManipulator>();
            RotationAxisConstraint rotationConstraint = model.AddComponent<RotationAxisConstraint>();
            model.AddComponent<NearInteractionGrabbable>();
            rotationConstraint.ConstraintOnRotation = AxisFlags.XAxis | AxisFlags.ZAxis;
            objectManipulator.TwoHandedManipulationType = TransformFlags.Move | TransformFlags.Rotate;
        }

        // Configure 3D object's position to in front of the player
        private void ConfigureModelPosition(GameObject model)
        {
            const float SAFE_DISTANCE = 3.0f;
            const float OFFSET_MULTIPLIER = 0.5f;
            Vector3 playerPosition = Camera.main.transform.position;
            Vector3 playerForward = Camera.main.transform.forward;
            Vector3 targetPosition = playerPosition + playerForward * SAFE_DISTANCE;
            RaycastHit hit;
            if (Physics.Raycast(targetPosition, playerForward, out hit, SAFE_DISTANCE))
                targetPosition = hit.point - _modelSize.magnitude * OFFSET_MULTIPLIER * playerForward;
            Vector3 finalPosition = targetPosition + Vector3.down * _modelCenter.y;
            Transform modelTransform = model.transform;
            modelTransform.position = finalPosition;
            modelTransform.LookAt(playerPosition);
            modelTransform.rotation = Quaternion.Euler(0f, modelTransform.rotation.eulerAngles.y, 0f);
        }

        // When the 3D object import finished, configure attribute of the 3D object.
        private void ConfigureModelBehavior(GameObject model)
        {
            _dialogController.LoadingDialog(LOADING_TITLE + " OnFinish");
            if (model != null)
            {
                AddColliderToModel(model);
                ConfigureModelComponents(model);
                ConfigureModelPosition(model);
                _modelManager.Add(_cacheFurnitureID, model);
                _ = _dialogController.DelayCloseDialog(LOADING_SUCCESS_TITLE);
            }
            else
            {
                Debug.LogError("Failed to import model.");
                _dialogController.ConfirmDialog(LOADING_FAILED_TITLE, LOADING_FAILED_MESSAGE);
            }
        }

        // Load model by uri
        public async Task LoadModelUri(string uri)
        {
            Response response = new Response();
            try
            {
                _dialogController.LoadingDialog(LOADING_TITLE, LOADING_REQUEST_DATA);
                response = await Rest.GetAsync(uri, readResponseData: true);
            }
            catch (Exception e)
            {
                _dialogController.ConfirmDialog(LOADING_FAILED_TITLE, LOADING_REQUEST_FAILED);
                Debug.LogError(e.Message);
            }
            if (!response.Successful)
            {
                _dialogController.ConfirmDialog(LOADING_FAILED_TITLE, LOADING_RESPONSE_FAILED);
                Debug.LogError($"Failed to get glb model from {uri}");
                return;
            }
            var gltfObject = GltfUtility.GetGltfObjectFromGlb(response.ResponseData);
            try
            {
                _dialogController.LoadingDialog(LOADING_TITLE, LOADING_MODEL);
                await gltfObject.ConstructAsync();
            }
            catch (Exception e)
            {
                Debug.LogError($"{e.Message}\n{e.StackTrace}");
                _dialogController.ConfirmDialog(LOADING_FAILED_TITLE, LOADING_MODEL_FAILED);
                return;
            }
            if (gltfObject != null)
            {
                GameObject model = gltfObject.GameObjectReference;
                ConfigureModelBehavior(model);
            }
            else
            {
                _dialogController.ConfirmDialog(LOADING_MODEL_NULL);
            }
        }
    }
}