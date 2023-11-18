using System;
using Taichi.Soft2D.Generated;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace Taichi.Soft2D.Plugin
{
    /// <summary>
    ///     �p�[�e�B�N���̃V�F�[�_�[�p�����[�^���w�肷��\����
    /// </summary>
    public enum ShaderType
    {
        Unlit, // 2D�A�����b�g�V�F�[�_�[
        PBR, // PBR�V�F�[�_�[�iURP�Ή��j
        Blinn_Phong, // Blinn_Phong�V�F�[�_�[�iURP�Ή��j
        Custom // ���[�U�[�J�X�^���V�F�[�_�[
    }

    [HelpURL("https://docs.unity3d.com/ScriptReference/Graphics.DrawMeshInstancedIndirect.html")]
    public class Soft2DManager : Singleton<Soft2DManager>
    {
        [HideInInspector]
        [Tooltip("�p�[�e�B�N���C���X�^���X�̃��b�V��")]
        public Mesh instanceMesh;

        [HideInInspector]
        [Tooltip("�p�[�e�B�N���C���X�^���X�̃}�e���A��")]
        public Material instanceMaterial;

        [HideInInspector]
        [Tooltip("�p�[�e�B�N���C���X�^���X�̃T�C�Y")]
        public float instanceSize;

        [HideInInspector] public Color emissionColor;
        [HideInInspector] public float smoothness;
        [HideInInspector] public float metallic;
        [HideInInspector] public float occlusionSize;

        [HideInInspector] public int layerIndex;

        [HideInInspector]
        [Tooltip("Soft2D�V�~�����[�V�����̈ꎞ��~")]
        public bool pause;

        [HideInInspector]
        [Tooltip("�ʒu�̃V�~�����[�V�����̈�")]
        public Vector2 worldOffset;

        [HideInInspector]
        [Tooltip("�X�P�[���̃V�~�����[�V�����̈�")]
        public Vector2 worldExtent;

        [HideInInspector]
        [Tooltip("�d�͂Ƃ��ăW���C���X�R�[�v��L���ɂ���")]
        public bool enableGyro;

        [HideInInspector]
        [Tooltip("�W���C���X�R�[�v�̏d�̓X�P�[��")]
        public int gyroScale;

        [HideInInspector]
        [Tooltip("�t�H�[�X�t�B�[���h��L���ɂ���")]
        public bool enableForceField;

        [HideInInspector]
        [Tooltip("�t�H�[�X�t�B�[���h�̃X�P�[��")]
        public float forceFieldScale;

        [HideInInspector]
        [Tooltip("�d�͂̃X�P�[���ƕ���")]
        public Vector2 gravity;

        [HideInInspector]
        [Tooltip("Soft2D�̃V�~�����[�V�����^�C���X�e�b�v")]
        public float timeStep;

        [HideInInspector]
        [Tooltip("�f�o�b�O�c�[����L���ɂ���")]
        public bool enableDebuggingTools;

        [HideInInspector]
        [Tooltip("�p�[�e�B�N���̃����_�����O���[�h")]
        public ShaderType shaderType;

        [HideInInspector]
        [Tooltip("���[���h���E��L���ɂ���")]
        public bool enableWorldBoundary;

        [HideInInspector]
        [Tooltip("���E�̏Փ˃^�C�v")]
        public CollisionType collisionType;

        [HideInInspector][Tooltip("���E�̖��C�W��")] public float frictionCoefficient;
        [HideInInspector][Tooltip("���E�̔����W��")] public float restitutionCoefficient;
        private readonly uint[] _args = { 0, 0, 0, 0, 0 };

        private int _subMeshIndex;

        private ComputeBuffer argsBuffer;

        private IntPtr argsBufferPtr;
        public UnityAction bodyAction;

        public UnityAction colliderAction;
        [Tooltip("�p�[�e�B�N����ID���i�[����NdArray")] private NdArray<int> idArray;
        private ComputeBuffer idBuffer;
        private IntPtr idBufferPtr;

        [Tooltip("UnityActions�����ɌĂяo���ꂽ���ǂ���")] private bool isInvoked;
        [Tooltip("�p�[�e�B�N���̈ʒu���i�[����NdArray")] private NdArray<float> positionArray;
        private ComputeBuffer positionBuffer;
        private IntPtr positionBufferPtr;
        [Tooltip("�p�[�e�B�N���̐��ʂ��i�[����NdArray")] private NdArray<int> quantityArray;
        [Tooltip("�p�[�e�B�N���̃^�O���i�[����NdArray")] private NdArray<int> tagArray;
        private ComputeBuffer tagBuffer;
        private IntPtr tagBufferPtr;
        public UnityAction triggerAction;
        [Tooltip("�p�[�e�B�N���̑��x���i�[����NdArray")] private NdArray<float> velocityArray;
        private ComputeBuffer velocityBuffer;
        private IntPtr velocityBufferPtr;

        private void Start()
        {
            if (enableWorldBoundary) colliderAction += AddWorldBoundary;

            if (LayerMask.LayerToName(layerIndex) == "") layerIndex = 0;
        }

        private void Update()
        {
            quantityArray = World.GetParticleNumBuffer();
            positionArray = World.GetParticlePositionBuffer();
            velocityArray = World.GetParticleVelocityBuffer();
            tagArray = World.GetParticleTagBuffer();
            idArray = World.GetParticleIdBuffer();

#if UNITY_EDITOR
            colliderPosArray = World.GetGridColliderNumFineBuffer();
            triggerPosArray = World.GetGridTriggerIdFineBuffer();
#endif
            if (!isInvoked)
            {
                colliderAction?.Invoke();
                bodyAction?.Invoke();
                triggerAction?.Invoke();
                isInvoked = true;
            }

            if (enableGyro)
            {
                if (SystemInfo.supportsGyroscope)
                {
                    World.SetGravity(Input.gyro.gravity * gyroScale);
                }
                else
                {
                    Debug.LogWarning("Device failed to support gyroscope!");
                    World.SetGravity(gravity);
                }
            }
            else
            {
                World.SetGravity(gravity);
            }

            if (enableForceField)
                MouseDown();

            // ���݂̃p�[�e�B�N������ (arg �o�b�t�@�[�̑�� uint ������) �R�s�[���܂�
            quantityArray.CopyToNativeBufferRangeAsync(argsBufferPtr, 0, 4, 4);
            positionArray.CopyToNativeBufferAsync(positionBufferPtr);
            velocityArray.CopyToNativeBufferAsync(velocityBufferPtr);
            tagArray.CopyToNativeBufferAsync(tagBufferPtr);
            idArray.CopyToNativeBufferAsync(idBufferPtr);

            Runtime.Submit();

            SetMaterialProperties();
            Graphics.DrawMeshInstancedIndirect(instanceMesh, _subMeshIndex, instanceMaterial, new Bounds(
                new Vector3(0.0f, 0.0f, 0.0f),
                new Vector3(500.0f, 500.0f, 500.0f)), argsBuffer, 0, null, ShadowCastingMode.On, true, layerIndex);
            
#if UNITY_EDITOR
            if (enableDebuggingTools)
            {
                var offset = World.GetWorldOffset();
                debugQuad.transform.position = new Vector3(offset.x + worldExtent.x / 2, offset.y + worldExtent.y / 2,
                    debugQuad.transform.position.z);
                colliderPosArray.CopyToNativeBufferAsync(colliderPosBufferPtr);
                triggerPosArray.CopyToNativeBufferAsync(triggerPosBufferPtr);
                debugShader.Dispatch(kernelIndex, resolution.x / 8, resolution.y / 8, 1);
            }
#endif
        }

        private void FixedUpdate()
        {
            if (!pause) World.Step(timeStep);
        }

        private void OnEnable()
        {
            if (worldExtent is { x: <= 0, y: <= 0 })
            {
                Debug.LogError("World Extent should above 0!");
                return;
            }

            gameObject.transform.hideFlags = HideFlags.HideInInspector;
            Application.targetFrameRate = (int)(1.0f / timeStep);
            UpdateWorldConfig();
            World.Reset();
            World.SetWorldExtent(worldExtent);
            World.SetSubstepTimeStep(1.6e-4f);

#if UNITY_EDITOR
            if (enableDebuggingTools) SetDebugTools();
#endif

            argsBuffer = new ComputeBuffer(_args.Length, sizeof(uint), ComputeBufferType.IndirectArguments);
            UpdateBuffers();

            argsBufferPtr = argsBuffer.GetNativeBufferPtr();
            positionBufferPtr = positionBuffer.GetNativeBufferPtr();
            velocityBufferPtr = velocityBuffer.GetNativeBufferPtr();
            tagBufferPtr = tagBuffer.GetNativeBufferPtr();
            idBufferPtr = idBuffer.GetNativeBufferPtr();

            Input.gyro.enabled = true;
        }

        private void OnDisable()
        {
            // ���ׂẴo�b�t�@���������
            positionBuffer?.Release();
            positionBuffer = null;

            velocityBuffer?.Release();
            velocityBuffer = null;

            tagBuffer?.Release();
            tagBuffer = null;

            idBuffer?.Release();
            idBuffer = null;

            argsBuffer?.Release();
            argsBuffer = null;

#if UNITY_EDITOR
            colliderPosBuffer?.Release();
            colliderPosBuffer = null;

            triggerPosBuffer?.Release();
            triggerPosBuffer = null;
#endif
        }

        /// <summary>
        ///     Soft2DManager �������p.
        ///     �o�b�t�@�[�� instanceMaterial �ɍX�V���܂��B
        ///     OnEnable() �ŌĂяo����܂��B
        /// </summary>
        private void UpdateBuffers()
        {
            // �T�u���b�V�� �C���f�b�N�X���͈͓��ɂ��邱�Ƃ��m�F
            if (instanceMesh != null)
                _subMeshIndex = Mathf.Clamp(_subMeshIndex, 0, instanceMesh.subMeshCount - 1);

            // �ʒu
            positionBuffer?.Release();
            positionBuffer = new ComputeBuffer((int)World.GetWorldMaxParticleNum(), sizeof(float) * 2);
            instanceMaterial.SetBuffer(PositionBuffer, positionBuffer);

            // ���x
            velocityBuffer?.Release();
            velocityBuffer = new ComputeBuffer((int)World.GetWorldMaxParticleNum(), sizeof(float) * 2);
            instanceMaterial.SetBuffer(VelocityBuffer, velocityBuffer);

            // �^�O
            tagBuffer?.Release();
            tagBuffer = new ComputeBuffer((int)World.GetWorldMaxParticleNum(), sizeof(int));
            instanceMaterial.SetBuffer(TagBuffer, tagBuffer);

            // ID
            idBuffer?.Release();
            idBuffer = new ComputeBuffer((int)World.GetWorldMaxParticleNum(), sizeof(int));
            instanceMaterial.SetBuffer(IDBuffer, idBuffer);

            // �C���f�B���N�g����
            if (instanceMesh != null)
            {
                _args[0] = instanceMesh.GetIndexCount(_subMeshIndex);
                _args[1] = 0;
                _args[2] = instanceMesh.GetIndexStart(_subMeshIndex);
                _args[3] = instanceMesh.GetBaseVertex(_subMeshIndex);
                _args[4] = 0;
            }
            else
            {
                _args[0] = _args[1] = _args[2] = _args[3] = _args[4] = 0;
            }

            argsBuffer.SetData(_args);
        }

        /// <summary>
        ///     Soft2DManager �������p.
        ///     instanceMaterial �̃p�����[�^��ݒ肵�܂��B
        ///     shaderType �� custom �ɐݒ肳��Ă��Ȃ��ꍇ�� Update() �ŌĂяo����܂��B
        /// </summary>
        private void SetMaterialProperties()
        {
            if (shaderType != ShaderType.Custom)
            {
                instanceMaterial.SetFloat(InstanceSize, instanceSize);
                if (shaderType != ShaderType.Unlit) instanceMaterial.SetFloat(Smoothness, smoothness);

                if (shaderType == ShaderType.PBR)
                {
                    instanceMaterial.SetFloat(Metallic, metallic);
                    instanceMaterial.SetFloat(Occlusion, occlusionSize);
                    instanceMaterial.SetColor(EmissionColor, emissionColor);
                }
            }
        }

#if UNITY_EDITOR
        /// <summary>
        ///     Soft2DManager �������p.
        ///     DebugTool �̃p�����[�^��ݒ肵�܂��B
        ///     OnEnable() �ŌĂяo����AUnity Editor �ł̂ݗ��p�\�ł��B
        /// </summary>
        private void SetDebugTools()
        {
            // Compute Shader �����[�h
            debugShader =
                AssetDatabase.LoadAssetAtPath<ComputeShader>(PathInitializer.MainPath +
                                                             "Materials/DebugTools/DebugComputeShader.compute");


            // Debug Quad �����[�h
            debugQuad = Instantiate(
                AssetDatabase.LoadAssetAtPath<GameObject>(PathInitializer.MainPath + "Prefabs/DebugQuad.prefab"),
                new Vector3(worldExtent.x / 2, worldExtent.y / 2, -0.2f), Quaternion.identity);
            debugQuad.transform.localScale = new Vector3(worldExtent.x, worldExtent.y, 1);
            debugMaterial = debugQuad.GetComponent<MeshRenderer>().material;
            debugMaterial.SetColor(CColor, colliderCol);
            debugMaterial.SetColor(TColor, triggerCol);

            // �𑜓x���v�Z
            resolution = World.GetWorldFineGridResolution();
            var count = resolution.x * resolution.y;
            uint ratio = 4;

            // Render Texture �����[�h
            outputRT = new RenderTexture(resolution.x, resolution.y, 0)
            {
                enableRandomWrite = true,
                useMipMap = false
            };
            outputRT.Create();
            debugMaterial.SetTexture(MainTex, outputRT);

            // Debug �o�b�t�@�����[�h
            kernelIndex = debugShader.FindKernel("DrawValidationImage");

            // �R���C�_�[�ƃg���K�[�̃o�b�t�@�[��������
            colliderPosBuffer?.Release();
            colliderPosBuffer = new ComputeBuffer(count, sizeof(int));
            colliderPosBufferPtr = colliderPosBuffer.GetNativeBufferPtr();
            debugShader.SetBuffer(kernelIndex, "colliderBuffer", colliderPosBuffer);

            triggerPosBuffer?.Release();
            triggerPosBuffer = new ComputeBuffer(count, sizeof(int));
            triggerPosBufferPtr = triggerPosBuffer.GetNativeBufferPtr();
            debugShader.SetBuffer(kernelIndex, "triggerBuffer", triggerPosBuffer);

            debugShader.SetTexture(kernelIndex, "ResultTexture", outputRT);
            debugShader.SetInt("resolutionY", resolution.y);
            debugShader.SetInt("ratio", (int)ratio);
        }
#endif

        /// <summary>
        ///     Soft2DManager �������p.
        ///     �p�[�e�B�N���ɒǉ������͂��v�Z���܂��B
        ///     enableForceField �� true �̏ꍇ�� Update() �ŌĂяo����܂��B
        /// </summary>
        private void MouseDown()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var pos3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var pos = new Vector2(pos3.x, pos3.y);
                if (!onDrag)
                {
                    lastPos = pos;
                    onDrag = true;
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                onDrag = false;
            }

            if (onDrag)
            {
                var pos3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var pos = new Vector2(pos3.x, pos3.y);
                if (!(pos.x < 0.0f || pos.x > 4.0f || pos.y < 0.0f || pos.y > 2.0f))
                {
                    var force = (pos - lastPos) * forceFieldScale;
                    World.AddForceFieldTransient(force, pos, 0.03f);
                }

                lastPos = pos;
            }
        }

        /// <summary>
        ///     Soft2DManager �������p.
        ///     �v���W�F�N�g�ݒ肩��p�����[�^���X�V���܂��B
        ///     OnEnable() �ŌĂяo����܂��B
        /// </summary>
        private void UpdateWorldConfig()
        {
            WorldConfig.config.enable_debugging = enableDebuggingTools ? (uint)1 : 0;
            WorldConfig.config.gravity = new S2Vec2(gravity.x, gravity.y);
            WorldConfig.config.extent = new S2Vec2(worldExtent.x, worldExtent.y);
            WorldConfig.config.offset = new S2Vec2(worldOffset.x, worldOffset.y);

            WorldConfig.config.max_allowed_particle_num = (uint)S2MaxParticleNum;
            WorldConfig.config.max_allowed_body_num = (uint)S2MaxBodyNum;
            WorldConfig.config.max_allowed_trigger_num = (uint)S2MaxTriggerNum;
            WorldConfig.config.grid_resolution = (uint)S2GridResolution;
            WorldConfig.config.substep_dt = S2Substep;
            WorldConfig.config.mesh_body_force_scale = S2MeshBodyForceScale;
            WorldConfig.config.collision_penalty_force_scale_along_normal_dir = S2NormalForceScale;
            WorldConfig.config.collision_penalty_force_scale_along_velocity_dir = S2VelocityForceScale;
            WorldConfig.config.enable_world_query = S2EnableWorldQuery ? (uint)1 : 0;

            if (S2FineGridScale <= 0)
                Utils.Assert("Fine Grid Scale should above 0!");
            else
                WorldConfig.config.fine_grid_scale = (uint)S2FineGridScale;
        }

        /// <summary>
        ///     Soft2DManager �������p.
        ///     enableWorldBoundary �� true �̏ꍇ�A���E�̋��E�Ƃ��ăR���C�_�[���쐬���܂��B
        ///     Start() �ŌĂяo����܂��B
        /// </summary>
        private void AddWorldBoundary()
        {
            var parameter = new S2CollisionParameter
            {
                collision_type = Utils.CollisionTypeDictionary[collisionType],
                friction_coeff = frictionCoefficient,
                restitution_coeff = restitutionCoefficient
            };
            var kinematicsInfo = new KinematicsInfo
            {
                center = new Vector2(worldOffset.x + worldExtent.x / 2, worldOffset.y + worldExtent.y),
                mobility = S2Mobility.S2_MOBILITY_STATIC
            };
            var kinematics = Utils.CreateKinematics(kinematicsInfo);
            var shape = Utils.CreateBoxShape(worldExtent.x / 2, 0.01f);
            World.CreateCollider(kinematics, shape, parameter);

            kinematicsInfo.center = new Vector2(worldOffset.x + worldExtent.y / 2, worldOffset.y);
            kinematics = Utils.CreateKinematics(kinematicsInfo);
            shape = Utils.CreateBoxShape(worldExtent.x / 2, 0.01f);
            World.CreateCollider(kinematics, shape, parameter);

            kinematicsInfo.center = new Vector2(worldOffset.x, worldOffset.y + worldExtent.y / 2);
            kinematics = Utils.CreateKinematics(kinematicsInfo);
            shape = Utils.CreateBoxShape(0.01f, worldExtent.y / 2);
            World.CreateCollider(kinematics, shape, parameter);

            kinematicsInfo.center = new Vector2(worldOffset.x + worldExtent.x, worldOffset.y + worldExtent.y / 2);
            kinematics = Utils.CreateKinematics(kinematicsInfo);
            shape = Utils.CreateBoxShape(0.01f, worldExtent.y / 2);
            World.CreateCollider(kinematics, shape, parameter);
        }
        
#if UNITY_EDITOR
        [Tooltip("�R���C�_�[�f�[�^���i�[����NdArray")] private NdArray<int> colliderPosArray;
        [Tooltip("�g���K�[�f�[�^���i�[����NdArray")] private NdArray<int> triggerPosArray;

        [Tooltip("�R���C�_�[�f�[�^���i�[����ComputeBuffer")]
        private ComputeBuffer colliderPosBuffer;

        [Tooltip("�g���K�[�f�[�^���i�[����ComputeBuffer")] private ComputeBuffer triggerPosBuffer;
        [Tooltip("colliderBuffer�ւ�IntPtr")] private IntPtr colliderPosBufferPtr;
        [Tooltip("triggerBuffer�ւ�IntPtr")] private IntPtr triggerPosBufferPtr;

        private ComputeShader debugShader;
        private Material debugMaterial;
        private RenderTexture outputRT;
        private int kernelIndex;
        private Vector2Int resolution;

        [HideInInspector]
        [Tooltip("Quad screening Debugging Tools")]
        public GameObject debugQuad;

        [HideInInspector]
        [Tooltip("�f�o�b�O�c�[���ɕ�������R���C�_�[�̐F")]
        public Color colliderCol = new(93 / 255f, 231 / 255f, 0, 1);

        [HideInInspector]
        [Tooltip("�f�o�b�O�c�[���ɕ�������g���K�[�̐F")]
        public Color triggerCol = new(246 / 255f, 238 / 255f, 6 / 255f, 1);
#endif

        #region ForceField INTERNAL Parameters

        private bool onDrag;
        private Vector2 lastPos;

        #endregion


        #region Soft2D Project Settings

        [HideInInspector] public int S2MaxParticleNum;
        [HideInInspector] public int S2MaxBodyNum;
        [HideInInspector] public int S2MaxTriggerNum;
        [HideInInspector] public int S2GridResolution;
        [HideInInspector] public float S2Substep;
        [HideInInspector] public float S2MeshBodyForceScale;
        [HideInInspector] public float S2NormalForceScale;
        [HideInInspector] public float S2VelocityForceScale;
        [HideInInspector] public int S2FineGridScale;
        [HideInInspector] public bool S2EnableWorldQuery = true;

        #endregion

        #region Shader Properties To ID

        private static readonly int MainTex = Shader.PropertyToID("_MainTex");
        private static readonly int PositionBuffer = Shader.PropertyToID("positionBuffer");
        private static readonly int VelocityBuffer = Shader.PropertyToID("velocityBuffer");
        private static readonly int TagBuffer = Shader.PropertyToID("tagBuffer");
        private static readonly int CColor = Shader.PropertyToID("_CColor");
        private static readonly int TColor = Shader.PropertyToID("_TColor");
        private static readonly int InstanceSize = Shader.PropertyToID("_InstanceSize");
        private static readonly int Smoothness = Shader.PropertyToID("_Smoothness");
        private static readonly int Metallic = Shader.PropertyToID("_Metallic");
        private static readonly int Occlusion = Shader.PropertyToID("_Occlusion");
        private static readonly int EmissionColor = Shader.PropertyToID("_Emission");
        private static readonly int IDBuffer = Shader.PropertyToID("IDBuffer");

        #endregion
    }
}