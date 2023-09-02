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
        private const string LOADING_MODEL_TITLE = "載入模型中...";
        private const string LOADING_MODEL_FAILED_TITLE = "模型載入失敗";
        private const string LOADING_MODEL_FAILED_MESSAGE = "請聯絡管理員修正錯誤\nexample@gmail.com";

        private PopupDialog _dialogController;
        private Vector3 _modelSize;
        private Vector3 _modelCenter;

        // Set dialog controller 
        public void SetPopupDialog(PopupDialog dialogController)
        {
            _dialogController = dialogController;
        }

        // Using MeshRenderer to calculate 3D object (and its child object) size and center by bounds

        private void CalculateModelSizeAndCenter(GameObject model)
        {
            var meshRenderers = model.GetComponentsInChildren<MeshRenderer>();
            var boundsList = meshRenderers.Select(renderer => renderer.bounds).ToList();
            var combinedBounds = boundsList.Aggregate((combined, nextBounds) => {
                combined.Encapsulate(nextBounds);
                return combined;
            });
            _modelSize = combinedBounds.size;
            _modelCenter = combinedBounds.center;

        }

        // Configure 3D object's components to grabbable and axis constraint
        private void ConfigureModelComponents(GameObject model)
        {
            var boxCollider = model.AddComponent<BoxCollider>();
            var objectManipulator = model.AddComponent<ObjectManipulator>();
            var rotationConstraint = model.AddComponent<RotationAxisConstraint>();
            model.AddComponent<NearInteractionGrabbable>();
            rotationConstraint.ConstraintOnRotation = AxisFlags.XAxis | AxisFlags.ZAxis;
            objectManipulator.TwoHandedManipulationType = TransformFlags.Move | TransformFlags.Rotate;
            boxCollider.size = new Vector3(_modelSize.x, _modelSize.y, _modelSize.z);
            boxCollider.center = new Vector3(_modelCenter.x, _modelCenter.y, _modelCenter.z);
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
            if (model != null)
            {
                CalculateModelSizeAndCenter(model);
                ConfigureModelComponents(model);
                ConfigureModelPosition(model);
                _dialogController.CloseDialog();
            }
            else
            {
                Debug.LogError("Failed to import model.");
                _dialogController.ConfirmDialog(LOADING_MODEL_FAILED_TITLE, LOADING_MODEL_FAILED_MESSAGE);
            }
        }

        // Load 3D object to scene by uri
        public async Task LoadModelUri(string uri)
        {
            _dialogController.LoadingDialog(LOADING_MODEL_TITLE);
            try
            {
                var response = await Rest.GetAsync(uri, readResponseData: true);
                if (!response.Successful)
                {
                    _dialogController.ConfirmDialog(LOADING_MODEL_FAILED_TITLE, LOADING_MODEL_FAILED_MESSAGE);
                    return;
                }
                Importer.ImportGLBAsync(response.ResponseData, new ImportSettings(), OnFinishAsync);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                _dialogController.ConfirmDialog(LOADING_MODEL_FAILED_TITLE, LOADING_MODEL_FAILED_MESSAGE);
            }
        }
    }
}