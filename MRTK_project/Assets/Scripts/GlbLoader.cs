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
        private const string LOADING_TITLE = "載入模型中...";
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
            var rootCollider = model.AddComponent<MeshCollider>();
            var meshRenderers = model.GetComponentsInChildren<MeshRenderer>();
            var combinedBounds = new Bounds();
            rootCollider.convex = true;
            foreach (var meshRenderer in meshRenderers)
            {
                var meshObject = meshRenderer.gameObject;
                var childCollider = meshObject.AddComponent<MeshCollider>();
                var childBounds = meshRenderer.bounds;
                childCollider.convex = true;
                combinedBounds.Encapsulate(childBounds);
            }
            _modelSize = combinedBounds.size;
            _modelCenter = combinedBounds.center;
        }

        // Configure 3D object's components to grabbable and axis constraint
        private void ConfigureModelComponents(GameObject model)
        {
            var objectManipulator = model.AddComponent<ObjectManipulator>();
            var rotationConstraint = model.AddComponent<RotationAxisConstraint>();
            model.AddComponent<NearInteractionGrabbable>();
            rotationConstraint.ConstraintOnRotation = AxisFlags.XAxis | AxisFlags.ZAxis;
            objectManipulator.TwoHandedManipulationType = TransformFlags.Move | TransformFlags.Rotate;
        }

        // Configure 3D object's position to in front of the player
        private void ConfigureModelPosition(GameObject model)
        {
            const float SAFE_DISTANCE = 3.0f;
            const float OFFSET_MULTIPLIER = 0.5f;
            var playerPosition = Camera.main.transform.position;
            var playerForward = Camera.main.transform.forward;
            var targetPosition = playerPosition + playerForward * SAFE_DISTANCE;
            RaycastHit hit;
            if (Physics.Raycast(targetPosition, playerForward, out hit, SAFE_DISTANCE))
                targetPosition = hit.point - _modelSize.magnitude * OFFSET_MULTIPLIER * playerForward;
            var finalPosition = targetPosition + Vector3.down * _modelCenter.y;
            var modelTransform = model.transform;
            modelTransform.position = finalPosition;
            modelTransform.LookAt(playerPosition);
            modelTransform.rotation = Quaternion.Euler(0f, modelTransform.rotation.eulerAngles.y, 0f);
        }

        // When the 3D object import finished, configure attribute of the 3D object.
        private void OnFinish(GameObject model, AnimationClip[] animations)
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

        // Load 3D object to scene by uri
        public async Task LoadModelUri(string uri)
        {
            _dialogController.LoadingDialog(LOADING_TITLE);
            try
            {
                _dialogController.LoadingDialog(LOADING_TITLE + " await Rest.GetAsync()");
                var response = await Rest.GetAsync(uri, readResponseData: true);
                _dialogController.LoadingDialog(LOADING_TITLE + " done Rest.GetAsync()");
                if (!response.Successful)
                {
                    _dialogController.ConfirmDialog(LOADING_FAILED_TITLE, LOADING_FAILED_MESSAGE);
                    return;
                }
                _dialogController.LoadingDialog(LOADING_TITLE + " Importer.ImportGLBAsync");
                Importer.ImportGLBAsync(response.ResponseData, new ImportSettings(), OnFinish);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                _dialogController.ConfirmDialog(LOADING_FAILED_TITLE, LOADING_FAILED_MESSAGE);
            }
        }

        public async Task LoadModelMRTKUri(string uri)
        {
            _dialogController.LoadingDialog(LOADING_TITLE);
            Response response = new Response();

            try
            {
                _dialogController.LoadingDialog(LOADING_TITLE + " Rest.GetAsync");
                response = await Rest.GetAsync(uri, readResponseData: true);
            }
            catch (Exception e)
            {
                _dialogController.ConfirmDialog(LOADING_FAILED_TITLE, $"Failed Rest.GetAsync\n{e.Message}");
                Debug.LogError(e.Message);
            }

            if (!response.Successful)
            {
                Debug.LogError($"Failed to get glb model from {uri}");
                _dialogController.ConfirmDialog(LOADING_FAILED_TITLE, $"Failed to get glb model from {uri}");
                return;
            }

            var gltfObject = GltfUtility.GetGltfObjectFromGlb(response.ResponseData);

            try
            {
                _dialogController.LoadingDialog(LOADING_TITLE + " gltfObject.ConstructAsync");
                await gltfObject.ConstructAsync();
            }
            catch (Exception e)
            {
                Debug.LogError($"{e.Message}\n{e.StackTrace}");
                _dialogController.ConfirmDialog(LOADING_FAILED_TITLE, $"Failed gltfObject.ConstructAsync\n{e.Message}\n{e.StackTrace}");
                return;
            }

            if (gltfObject != null)
            {
                GameObject model = gltfObject.GameObjectReference;
                Debug.Log("Import successful");
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
            else
            {
                _dialogController.ConfirmDialog(LOADING_FAILED_TITLE);
            }
        }
    }
}