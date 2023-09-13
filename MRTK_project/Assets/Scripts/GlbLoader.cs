using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;
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
        private Vector3 _modelSize;
        private Vector3 _modelCenter;
        private int _cacheFurnitureID;

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
            rootCollider.convex = true;
            var meshRenderers = model.GetComponentsInChildren<MeshRenderer>();
            var childWithMeshList = meshRenderers.Select(renderer => renderer.gameObject).ToList();
            childWithMeshList.Remove(model);
            childWithMeshList.ForEach(meshObject => {
                var childCollider = meshObject.AddComponent<MeshCollider>();
                childCollider.convex = true;
            });
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
        private void OnFinishAsync(GameObject model, AnimationClip[] animations)
        {
            Debug.LogWarning(model);
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
                var response = await Rest.GetAsync(uri, readResponseData: true);
                if (!response.Successful)
                {
                    _dialogController.ConfirmDialog(LOADING_FAILED_TITLE, LOADING_FAILED_MESSAGE);
                    return;
                }
                Importer.ImportGLBAsync(response.ResponseData, new ImportSettings(), OnFinishAsync);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                _dialogController.ConfirmDialog(LOADING_FAILED_TITLE, LOADING_FAILED_MESSAGE);
            }
        }
    }
}